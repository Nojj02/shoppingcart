package com.jll.model;

import com.jll.model.events.CartCreatedEvent;
import com.jll.model.events.CartEvent;
import com.jll.model.events.CouponAppliedToCartEvent;
import com.jll.model.events.ItemAddedToCartEvent;

import java.util.*;
import java.util.stream.Collectors;

public class Cart extends AggregateRoot<CartIdentity> {
    private List<CartItem> cartItems;
    private Coupon coupon;

    private final List<CartEvent> _cartEvents = new ArrayList<CartEvent>();
    private final List<CartEvent> _newCartEvents = new ArrayList<CartEvent>();

    private static HashMap<Class, CartEventOperator> _eventRouter = NewEventRouter();

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

    public int getVersion() {
        return _cartEvents.stream()
                .map(x -> x.version)
                .max(Comparator.comparing(Integer::valueOf))
                .orElse(-1);
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
        return new CartItem(itemForPurchase);
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

        addEvent(e, isNew);
    }

    private void apply(CouponAppliedToCartEvent e, boolean isNew) {
        this.coupon = e.coupon;

        addEvent(e, isNew);
    }

    private void apply(ItemAddedToCartEvent e, boolean isNew) {
        cartItems.add(newCartItem(e.itemForPurchase));
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

