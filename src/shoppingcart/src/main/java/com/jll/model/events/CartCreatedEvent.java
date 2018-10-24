package com.jll.model.events;

import java.util.Collection;

public class CartCreatedEvent extends CartEvent {
    public Collection<com.jll.model.ItemForPurchase> itemsForPurchase;
}
