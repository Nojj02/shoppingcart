package com.jll.handlers;

import com.jll.model.Item;
import com.jll.model.ItemIdentity;
import com.jll.repositories.InMemoryCartCostRepository;
import com.jll.repositories.InMemoryItemRepository;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;
import com.jll.model.events.*;

import java.util.List;
import java.util.UUID;

public class ComputeCostWhenCartCreatedHandlerTests {
    @Test
    public void cartCostSaved_newCartCreatedEvent() {
        var cartCreatedEvent = new CartCreatedEvent();
        cartCreatedEvent.version = 1;

        var itemId = new ItemIdentity(UUID.randomUUID());
        var quantity = 5;
        var itemsForPurchase = com.jll.model.ItemForPurchase.createItemForPurchase(itemId, quantity);

        var eventItemsForPurchase = List.of(itemsForPurchase);
        cartCreatedEvent.itemsForPurchase = eventItemsForPurchase;

        var item =
                new Item(itemId,
                        "banana",
                        12);
        var items = List.of(item);
        var itemRepository = new InMemoryItemRepository(items);
        var cartCostRepository = new InMemoryCartCostRepository();

        var handler =
                new ComputeCostWhenCartCreatedHandler(
                        itemRepository,
                        cartCostRepository
                );
        handler.handle(cartCreatedEvent);

        var cartCosts = cartCostRepository.cartCosts;
        Assertions.assertEquals(1, cartCosts.size());
    }
}

