package com.jll.dtos;

import com.jll.models.Cart;

import java.util.Collection;
import java.util.UUID;
import java.util.stream.Collectors;

public class CartDto {
    public UUID Id;
    public Collection<CartItemDto> CartItems;
    public double GrossAmount;
    public double DiscountAmount;
    public double ShippingCost;
    public double TotalCost;
    public String CouponCode;

    public CartDto(Cart cart) {
        Id = cart.getId();
        CartItems = cart.getCartItems().stream()
                .map(cartItem -> new CartItemDto(cartItem))
                .collect(Collectors.toList());
        GrossAmount = cart.getCost().getGrossAmount();
        DiscountAmount = cart.getCost().getDiscountAmount();
        ShippingCost = cart.getCost().getShippingCost();
        TotalCost = cart.getCost().getTotalCost();
        CouponCode = cart.getOptionalCoupon()
                .map(x -> x.getCouponCode())
                .orElse("NONE");
    }
}
