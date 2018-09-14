package com.jll.models;

import com.jll.services.ShippingCostCalculator;

import java.util.Collection;
import java.util.List;
import java.util.Optional;
import java.util.UUID;
import java.util.stream.Collectors;

public class Cart extends AggregateRoot {
    private Cost cost;
    private Collection<CartItem> cartItems;
    private Coupon coupon;

    public Cart(
            UUID id,
            Collection<ItemForPurchase> itemsForPurchase
    ) {
        super(id);
        this.cartItems = newCartItems(itemsForPurchase);
        this.cost = computeCost(this.getCartItems());
    }

    public Cost getCost() {
        return cost;
    }

    public Collection<CartItem> getCartItems() {
        return cartItems;
    }

    public void applyCoupon(Coupon coupon) {
        for(var cartItem : this.getCartItems()) {
            var newDiscount = computeTotalDiscountAmount(cartItem.getItemForPurchase(), Optional.of(coupon));
            var cost = cartItem.getCost();
            cartItem.updateCost(new Cost(cost.grossAmount, newDiscount, cost.shippingCost));
        }
        this.cost = computeCost(this.getCartItems());
        this.coupon = coupon;
    }

    private Cost computeCost(Collection<CartItem> cartItems) {
        return cartItems.stream()
                .map(x -> x.getCost())
                .reduce(new Cost(0, 0,0), (a, b) -> a.add(b));
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
}

