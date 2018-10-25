package com.jll.services;

import com.jll.model.*;

import java.util.List;
import java.util.Optional;

public class CartCostCalculator {
    public CartCost compute(List<ComputeCostCartItem> cartItems) {
        return compute(cartItems, Optional.empty());
    }

    public CartCost compute(List<ComputeCostCartItem> cartItems, Optional<Coupon> optionalCoupon) {
        var cost = cartItems.stream()
                .map(x -> compute(x, optionalCoupon))
                .reduce(Cost.Zero, (a, b) -> a.add(b));
        return new CartCost(CartCostIdentity.generateNew(), cost);
    }

    private Cost compute(ComputeCostCartItem cartItem, Optional<Coupon> optionalCoupon)
    {
        var totalGrossAmount = cartItem.item.getPrice() * cartItem.quantity;
        var shippingCost = new ShippingCostCalculator().compute(cartItem.item.getWeight()) * cartItem.quantity;

        var discountAmount = computeTotalDiscountAmount(cartItem, optionalCoupon);
        return new Cost(totalGrossAmount, discountAmount, shippingCost);
    }

    private double computeHighestItemDiscountAmount(double itemPrice, Discount itemDiscount) {
        var percentageDiscount = itemDiscount.computeDiscount(itemPrice);
        var fixedAmountDiscount = itemDiscount.getFixedAmount();
        return Math.max(percentageDiscount, fixedAmountDiscount);
    }

    private double computeTotalDiscountAmount(
            ComputeCostCartItem cartItem,
            Optional<Coupon> optionalCoupon) {
        var totalGrossAmount = cartItem.item.getPrice() * cartItem.quantity;
        var perItemDiscountAmount = computeHighestItemDiscountAmount(cartItem.item.getPrice(), cartItem.item.getDiscount()) * cartItem.quantity;

        var couponDiscount =
                optionalCoupon
                        .filter(coupon -> coupon.appliesTo(cartItem.item.getItemType().getItemTypeCode()))
                        .map(coupon -> coupon.getDiscount())
                        .orElse(Discount.None);

        var couponDiscountAmount = couponDiscount.computeDiscount(totalGrossAmount);
        return Math.max(perItemDiscountAmount, couponDiscountAmount);
    }

    public static class ComputeCostCartItem {
        public Item item;
        public double quantity;

        public ComputeCostCartItem(Item item, int quantity) {
            this.item = item;
            this.quantity = quantity;
        }
    }
}
