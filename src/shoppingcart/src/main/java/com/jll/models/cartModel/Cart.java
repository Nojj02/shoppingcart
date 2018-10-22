package com.jll.models.cartModel;

import com.jll.models.*;
import com.jll.models.cartModel.events.CartCreatedEvent;
import com.jll.models.cartModel.events.CartEvent;
import com.jll.models.cartModel.events.CouponAppliedToCartEvent;
import com.jll.models.cartModel.events.ItemAddedToCartEvent;

import java.util.*;
import java.util.stream.Collectors;

public class Cart extends AggregateRoot<CartIdentity> {
    //private Cost cost;
    private List<CartItem> cartItems;

    public static Cart reconstitute(CartIdentity id, Collection<CartEvent> cartEvents) {
        var cart = new Cart(id);
        for (var e : cartEvents) {
            _eventRouter.get(e.getClass()).apply(cart, e);
        }
        return cart;
    }

    private Cart(CartIdentity id) {
        super(id);
    }

    public Cart(
            CartIdentity id,
            Collection<ItemForPurchase> itemsForPurchase
    ) {
        super(id);
        var e = new CartCreatedEvent();
        e.version = getVersion() + 1;
        e.itemsForPurchase = itemsForPurchase;
        apply(e, true);
    }

    /*public Cost getCost() {
        return cost;
    }*/

    public Collection<CartItem> getCartItems() {
        return cartItems;
    }

    /*public Optional<Coupon> getOptionalCoupon() { return Optional.ofNullable(this.coupon); }*/

    public void applyCoupon(Coupon coupon) {
        var e = new CouponAppliedToCartEvent();
        e.version = getVersion() + 1;
        e.coupon = coupon;
        //apply(e, true);
    }

    public void addItem(ItemForPurchase itemForPurchase) {
        var e = new ItemAddedToCartEvent();
        e.version = getVersion() + 1;
        e.itemForPurchase = itemForPurchase;
        apply(e, true);
    }

    /*private Cost computeCost(Collection<CartItem> cartItems) {
        return cartItems.stream()
                .map(x -> x.getCost())
                .reduce(Cost.Zero, (a, b) -> a.add(b));
    }*/

    private List<CartItem> newCartItems(Collection<ItemForPurchase> itemsForPurchase) {
        return newCartItems(itemsForPurchase, Optional.empty());
    }

    private List<CartItem> newCartItems(Collection<ItemForPurchase> itemsForPurchase, Optional<Coupon> optionalCoupon) {
        return itemsForPurchase.stream()
                .map(itemForPurchase -> newCartItem(itemForPurchase))
                .collect(Collectors.toList());
    }

    private CartItem newCartItem(ItemForPurchase itemForPurchase)
    {
        /*var totalGrossAmount = itemForPurchase.getPrice() * itemForPurchase.getQuantity();
        var shippingCost = new ShippingCostCalculator().compute(itemForPurchase.getWeight()) * itemForPurchase.getQuantity();

        var discountAmount = computeTotalDiscountAmount(itemForPurchase, optionalCoupon);
        var cost =  new Cost(totalGrossAmount, discountAmount, shippingCost);*/

        return new CartItem(itemForPurchase);
    }

    /*private double computeTotalDiscountAmount(
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
    }*/

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
        //this.cost = computeCost(this.getCartItems());

        addEvent(e, isNew);
    }

    /*private void apply(CouponAppliedToCartEvent e, boolean isNew) {
        for(var cartItem : this.getCartItems()) {
            var newDiscount = computeTotalDiscountAmount(cartItem.getItemForPurchase(), Optional.of(e.coupon));
            var cost = cartItem.getCost();
            cartItem.updateCost(new Cost(cost.getGrossAmount(), newDiscount, cost.getShippingCost()));
        }
        this.cost = computeCost(this.getCartItems());
        this.coupon = e.coupon;

        addEvent(e, isNew);
    }*/

    private void apply(ItemAddedToCartEvent e, boolean isNew) {
        cartItems.add(newCartItem(e.itemForPurchase));
        //this.cost = computeCost(this.getCartItems());
        addEvent(e, isNew);
    }

    private static HashMap<Class, CartEventOperator> NewEventRouter() {
        var eventRouter = new HashMap<Class, CartEventOperator>();

        eventRouter.put(CartCreatedEvent.class, (Cart c, CartEvent e) -> c.apply((CartCreatedEvent)e, false));
        //eventRouter.put(CouponAppliedToCartEvent.class, (Cart c, CartEvent e) -> c.apply((CouponAppliedToCartEvent)e, false));
        eventRouter.put(ItemAddedToCartEvent.class, (Cart c, CartEvent e) -> c.apply((ItemAddedToCartEvent)e, false));
        return eventRouter;
    }
}

