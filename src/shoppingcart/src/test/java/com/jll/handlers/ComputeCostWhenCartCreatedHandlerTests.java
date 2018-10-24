package com.jll.handlers;

import com.jll.model.ItemIdentity;
import com.jll.model.events.CartCreatedEvent;
import com.jll.model.ItemForPurchase;
import org.junit.jupiter.api.Test;

import java.util.List;
import java.util.UUID;

public class ComputeCostWhenCartCreatedHandlerTests {
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
