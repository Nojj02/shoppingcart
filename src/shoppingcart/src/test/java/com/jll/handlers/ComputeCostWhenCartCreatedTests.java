package com.jll.handlers;

import com.jll.models.ItemIdentity;
import com.jll.models.cartModel.events.CartCreatedEvent;
import com.jll.models.cartModel.ItemForPurchase;
import org.junit.jupiter.api.Test;

import java.util.List;
import java.util.UUID;

public class ComputeCostWhenCartCreatedTests {
    @Test
    public void cartCostSaved_newCartCreatedEvent() {
        var itemId = new ItemIdentity(UUID.randomUUID());
        var quantity = 5;
        var itemForPurchase = ItemForPurchase.createItemForPurchase(itemId, quantity);

        var itemsForPurchase = List.of(itemForPurchase);

        var cartCreatedEvent = new CartCreatedEvent();
        cartCreatedEvent.version = 1;
        cartCreatedEvent.itemsForPurchase = itemsForPurchase;

        var handler = new ComputeCostWhenCartCreatedHandler();
        handler.Handle(cartCreatedEvent);
    }
}
