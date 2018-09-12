package com.jll.dtos;

import com.jll.models.Cart;

import java.util.Collection;
import java.util.stream.Collectors;

public class CartDto {
    public Collection<CartItemDto> CartItems;

    public CartDto(Cart cart) {
        CartItems = cart.getCartItems().stream()
                .map(cartItem -> new CartItemDto(cartItem))
                .collect(Collectors.toList());
    }
}
