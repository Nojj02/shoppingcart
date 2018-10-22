package com.jll.dtos;

import com.jll.models.cartModel.Cart;

import java.util.Collection;
import java.util.UUID;
import java.util.stream.Collectors;

public class CartDto {
    public UUID Id;
    public Collection<CartItemDto> CartItems;

    public CartDto(Cart cart) {
        Id = cart.getId2();
        CartItems = cart.getCartItems().stream()
                .map(cartItem -> new CartItemDto(cartItem))
                .collect(Collectors.toList());
    }
}
