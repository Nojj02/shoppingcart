package com.jll.dtos;

import com.jll.models.CartItem;

import java.util.UUID;

public class CartItemDto {
    public UUID ItemId;
    public int Quantity;

    public CartItemDto(CartItem cartItem) {
        ItemId = cartItem.getItemForPurchase().getItemId();
        Quantity = cartItem.getItemForPurchase().getQuantity();
    }
}
