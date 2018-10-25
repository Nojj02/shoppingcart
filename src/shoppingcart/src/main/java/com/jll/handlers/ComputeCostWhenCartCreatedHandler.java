package com.jll.handlers;

import com.jll.model.CartCost;
import com.jll.model.CartCostIdentity;
import com.jll.model.events.CartCreatedEvent;
import com.jll.repositories.CartCostRepository;
import com.jll.repositories.ItemRepository;
import com.jll.services.CartCostCalculator;

import java.sql.SQLException;
import java.util.UUID;
import java.util.stream.Collectors;

public class ComputeCostWhenCartCreatedHandler {
    private ItemRepository itemRepository;
    private CartCostRepository cartCostRepository;

    public ComputeCostWhenCartCreatedHandler(ItemRepository itemRepository, CartCostRepository cartCostRepository) {
        this.itemRepository = itemRepository;
        this.cartCostRepository = cartCostRepository;
    }

    public void handle(CartCreatedEvent cartCreatedEvent) {
        var cartItems =
                cartCreatedEvent.itemsForPurchase.stream()
                        .map(x -> {
                            try {
                                var item = this.itemRepository.get(x.getItemId())
                                        .orElseThrow();
                                return new CartCostCalculator.ComputeCostCartItem(item, x.getQuantity());
                            } catch (SQLException e) {
                                throw new RuntimeException();
                            }
                        })
                        .collect(Collectors.toList());
        var cartCostCalculator = new CartCostCalculator();
        var cartCost = cartCostCalculator.compute(cartItems);
        this.cartCostRepository.save(cartCost);
    }
}
