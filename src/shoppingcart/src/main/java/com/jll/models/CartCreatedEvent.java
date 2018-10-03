package com.jll.models;

import java.util.Collection;

public class CartCreatedEvent extends CartEvent {
    Collection<ItemForPurchase> itemsForPurchase;
}
