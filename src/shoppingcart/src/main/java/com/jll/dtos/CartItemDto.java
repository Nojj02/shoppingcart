package com.jll.dtos;

import com.jll.models.cartModel.CartItem;
import com.jll.models.cartModel.ItemForPurchase;

public class CartItemDto {
    public IdentityDto itemId;
    public int quantity;

    public CartItemDto(CartItem cartItem) {
        ItemForPurchase itemForPurchase = cartItem.getItemForPurchase();
        itemId = IdentityDto.fromIdentity(itemForPurchase.getItemId());
        quantity = itemForPurchase.getQuantity();
    }
}

