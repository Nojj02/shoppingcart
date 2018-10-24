package com.jll.handlers;

import com.jll.model.events.CartCreatedEvent;
import com.jll.repositories.CartCostRepository;
import com.jll.repositories.ItemRepository;

public class ComputeCostWhenCartCreatedHandler {
    private ItemRepository itemRepository;
    private CartCostRepository cartCostRepository;

    public ComputeCostWhenCartCreatedHandler(ItemRepository itemRepository, CartCostRepository cartCostRepository) {

        this.itemRepository = itemRepository;
        this.cartCostRepository = cartCostRepository;
    }

    public void Handle(CartCreatedEvent cartCreatedEvent) {
    }
}
