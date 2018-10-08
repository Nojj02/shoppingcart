package com.jll.models;

import java.util.Collection;
import java.util.UUID;

public class CartCreatedEvent extends CartEvent {
    public Collection<ItemForPurchase> itemsForPurchase;
}
