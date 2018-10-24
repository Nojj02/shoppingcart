package com.jll.model.events;

import com.jll.model.ItemForPurchase;

import java.util.Collection;

public class CartCreatedEvent extends CartEvent {
    public Collection<ItemForPurchase> itemsForPurchase;
}
