package com.jll.models.cartModel.events;

import com.jll.models.cartModel.ItemForPurchase;

import java.util.Collection;

public class CartCreatedEvent extends CartEvent {
    public Collection<ItemForPurchase> itemsForPurchase;
}
