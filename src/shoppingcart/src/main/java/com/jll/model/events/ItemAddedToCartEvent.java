package com.jll.model.events;

import com.jll.model.ItemForPurchase;

public class ItemAddedToCartEvent extends CartEvent {
    public ItemForPurchase itemForPurchase;
}
