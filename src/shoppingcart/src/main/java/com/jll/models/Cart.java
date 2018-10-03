package com.jll.models;

import com.jll.services.ShippingCostCalculator;

import java.util.*;
import java.util.stream.Collectors;

public class Cart extends AggregateRoot {
    private Cost cost;
    private List<CartItem> cartItems;
    private Coupon coupon;

    private Cart() {
        super(new UUID(0, 0));
    }

    private Cart(UUID id) {
        super(id);
    }

    public Cart(
            UUID id,
            Collection<ItemForPurchase> itemsForPurchase
    ) {
        super(id);
        var e = new CartCreatedEvent();
        e.version = getVersion() + 1;
        e.itemsForPurchase = itemsForPurchase;
        apply(e, true);
    }

    public Cost getCost() {
        return cost;
    }

    public Collection<CartItem> getCartItems() {
        return cartItems;
    }

    public Optional<Coupon> getOptionalCoupon() { return Optional.ofNullable(this.coupon); }

    public void applyCoupon(Coupon coupon) {
        var e = new CouponAppliedToCartEvent();
        e.version = getVersion() + 1;
        e.coupon = coupon;
        apply(e, true);
    }

    public void addItem(ItemForPurchase itemForPurchase) {
        var e = new ItemAddedToCartEvent();
        e.version = getVersion() + 1;
        e.itemForPurchase = itemForPurchase;
        apply(e, true);
    }

    private Cost computeCost(Collection<CartItem> cartItems) {
        return cartItems.stream()
                .map(x -> x.getCost())
                .reduce(Cost.Zero, (a, b) -> a.add(b));
    }

    private List<CartItem> newCartItems(Collection<ItemForPurchase> itemsForPurchase) {
        return newCartItems(itemsForPurchase, Optional.empty());
    }

    private List<CartItem> newCartItems(Collection<ItemForPurchase> itemsForPurchase, Optional<Coupon> optionalCoupon) {
        return itemsForPurchase.stream()
                .map(itemForPurchase -> newCartItem(itemForPurchase, optionalCoupon))
                .collect(Collectors.toList());
    }

    private CartItem newCartItem(ItemForPurchase itemForPurchase, Optional<Coupon> optionalCoupon)
    {
        var totalGrossAmount = itemForPurchase.getPrice() * itemForPurchase.getQuantity();
        var shippingCost = new ShippingCostCalculator().compute(itemForPurchase.getWeight()) * itemForPurchase.getQuantity();

        var discountAmount = computeTotalDiscountAmount(itemForPurchase, optionalCoupon);
        var cost =  new Cost(totalGrossAmount, discountAmount, shippingCost);

        return new CartItem(itemForPurchase, cost);
    }

    private double computeTotalDiscountAmount(
            ItemForPurchase itemForPurchase,
            Optional<Coupon> optionalCoupon) {
        var totalGrossAmount = itemForPurchase.getPrice() * itemForPurchase.getQuantity();
        var perItemDiscountAmount = computeHighestItemDiscountAmount(itemForPurchase.getPrice(), itemForPurchase.getDiscount()) * itemForPurchase.getQuantity();

        var couponDiscount =
                optionalCoupon
                        .filter(coupon -> coupon.appliesTo(itemForPurchase.getItemTypeCode()))
                        .map(coupon -> coupon.getDiscount())
                        .orElse(Discount.None);

        var couponDiscountAmount = couponDiscount.computeDiscount(totalGrossAmount);
        return Math.max(perItemDiscountAmount, couponDiscountAmount);
    }

    private double computeHighestItemDiscountAmount(double itemPrice, Discount itemDiscount) {
        var percentageDiscount = itemDiscount.computeDiscount(itemPrice);
        var fixedAmountDiscount = itemDiscount.getFixedAmount();
        return Math.max(percentageDiscount, fixedAmountDiscount);
    }

    private final List<CartEvent> _cartEvents = new ArrayList<CartEvent>();
    private final List<CartEvent> _newCartEvents = new ArrayList<CartEvent>();

    private static HashMap<Class, CartEventOperator> _eventRouter = NewEventRouter();

    public int getVersion() {
        return _cartEvents.stream()
                .map(x -> x.version)
                .max(Comparator.comparing(Integer::valueOf))
                .orElse(-1);
    }

    public static Cart reconstitute(Collection<CartEvent> cartEvents) {
        var cart = new Cart();
        for (var e : cartEvents) {
            _eventRouter.get(e.getClass()).apply(cart, e);
        }
        return cart;
    }

    public List<CartEvent> getEvents() {
        return Collections.unmodifiableList(_cartEvents);
    }

    public List<CartEvent> getNewCartEvents() {
        return Collections.unmodifiableList(_newCartEvents);
    }

    private void addEvent(CartEvent e, boolean isNew) {
        _cartEvents.add(e);
        if (isNew) {
            _newCartEvents.add(e);
        }
    }

    private void apply(CartCreatedEvent e, boolean isNew) {
        this.cartItems = newCartItems(e.itemsForPurchase);
        this.cost = computeCost(this.getCartItems());

        addEvent(e, isNew);
    }

    private void apply(CouponAppliedToCartEvent e, boolean isNew) {
        for(var cartItem : this.getCartItems()) {
            var newDiscount = computeTotalDiscountAmount(cartItem.getItemForPurchase(), Optional.of(e.coupon));
            var cost = cartItem.getCost();
            cartItem.updateCost(new Cost(cost.grossAmount, newDiscount, cost.shippingCost));
        }
        this.cost = computeCost(this.getCartItems());
        this.coupon = e.coupon;

        addEvent(e, isNew);
    }

    private void apply(ItemAddedToCartEvent e, boolean isNew) {
        cartItems.add(newCartItem(e.itemForPurchase, getOptionalCoupon()));
        this.cost = computeCost(this.getCartItems());
        addEvent(e, isNew);
    }

    private static HashMap<Class, CartEventOperator> NewEventRouter() {
        var eventRouter = new HashMap<Class, CartEventOperator>();

        eventRouter.put(CartCreatedEvent.class, (Cart c, CartEvent e) -> c.apply((CartCreatedEvent)e, false));
        eventRouter.put(CouponAppliedToCartEvent.class, (Cart c, CartEvent e) -> c.apply((CouponAppliedToCartEvent)e, false));
        eventRouter.put(ItemAddedToCartEvent.class, (Cart c, CartEvent e) -> c.apply((ItemAddedToCartEvent)e, false));
        return eventRouter;
    }
}

