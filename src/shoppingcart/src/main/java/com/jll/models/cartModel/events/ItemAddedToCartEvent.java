package com.jll.models.cartModel.events;

import com.jll.models.cartModel.ItemForPurchase;

public class ItemAddedToCartEvent extends CartEvent {
    public ItemForPurchase itemForPurchase;
}
