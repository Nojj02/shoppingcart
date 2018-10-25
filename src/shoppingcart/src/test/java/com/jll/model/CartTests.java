package com.jll.model;

import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

public class CartTests {
    public static class GetVersionTests {
        @Test
        public void Returns0_CreatedObject() {
            var cart = new Cart(new CartIdentity(UUID.randomUUID()), new ArrayList<>());

            Assertions.assertEquals(0, cart.getVersion());
        }

        @Test
        public void Returns1_AnEventHappened() {
            Item banana = new Item(new ItemIdentity(UUID.randomUUID()), "Banana", 12.00);

            var cart = new Cart(new CartIdentity(UUID.randomUUID()), new ArrayList<>());
            cart.addItem(ItemForPurchase.createItemForPurchase(banana.getId(), 1));

            Assertions.assertEquals(1, cart.getVersion());
        }
    }

    public static class AddItemTests {
        @Test
        public void addItem() {
            Item banana = new Item(new ItemIdentity(UUID.randomUUID()), "Banana", 12.00);
            Item potato = new Item(new ItemIdentity(UUID.randomUUID()), "Potato", 18.00);

            var itemsForPurchase = List.of(
                    ItemForPurchase.createItemForPurchase(banana.getId(), 5)
            );

            var cart = new Cart(new CartIdentity(UUID.randomUUID()), itemsForPurchase);
            cart.addItem(ItemForPurchase.createItemForPurchase(potato.getId(), 2));

            /*var cost = cart.getCost();
            Assertions.assertEquals(0, cost.getDiscountAmount());
            Assertions.assertEquals(96, cost.getGrossAmount());*/
        }
    }
}