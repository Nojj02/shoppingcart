package com.jll.models;

import com.jll.models.*;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

public class CartTests {
    public static class GetVersionTests {
        @Test
        public void Returns0_CreatedObject() {
            var cart = new Cart(UUID.randomUUID(), new ArrayList<>());

            Assertions.assertEquals(0, cart.getVersion());
        }

        @Test
        public void Returns1_AnEventHappened() {
            Item banana = new Item(UUID.randomUUID(), "Banana", 12.00);

            var cart = new Cart(UUID.randomUUID(), new ArrayList<>());
            cart.addItem(new ItemForPurchase(banana, 1, Discount.None));

            Assertions.assertEquals(1, cart.getVersion());
        }
    }

    public static class ComputeTests {
        @Test
        void computesTotalCost() {
            Item banana = new Item(UUID.randomUUID(), "Banana", 12.00);
            Item potato = new Item(UUID.randomUUID(), "Potato", 18.00);

            var itemsForPurchase = List.of(
                    new ItemForPurchase(banana, 5),
                    new ItemForPurchase(potato, 2)
            );

            var cart = new Cart(UUID.randomUUID(), itemsForPurchase);

            var cost = cart.getCost();
            Assertions.assertEquals(0, cost.getDiscountAmount());
            Assertions.assertEquals(96, cost.getGrossAmount());
        }

        @Test
        void itemsAreDiscounted_itemsAreOnSpecialWithPercentage() {
            Item banana = new Item(UUID.randomUUID(), "Banana", 12.00);
            Item potato = new Item(UUID.randomUUID(), "Potato", 18.00);

            var itemsForPurchase = List.of(
                    new ItemForPurchase(banana, 5, new Discount(50.00, 0)),
                    new ItemForPurchase(potato, 2)
            );

            var cart = new Cart(UUID.randomUUID(), itemsForPurchase);

            var cost = cart.getCost();

            Assertions.assertEquals(30, cost.getDiscountAmount());
            Assertions.assertEquals(96, cost.getGrossAmount());
        }

        @Test
        void itemsAreDiscounted_itemsAreOnSpecialWithFixedRate() {
            Item banana = new Item(UUID.randomUUID(), "Banana", 12.00);
            Item potato = new Item(UUID.randomUUID(), "Potato", 18.00);

            var itemsForPurchase = List.of(
                    new ItemForPurchase(banana, 5, new Discount(0, 3)),
                    new ItemForPurchase(potato, 2)
            );

            var cart = new Cart(UUID.randomUUID(), itemsForPurchase);

            var cost = cart.getCost();

            Assertions.assertEquals(15, cost.getDiscountAmount());
            Assertions.assertEquals(96, cost.getGrossAmount());
        }

        @Test
        void itemsAreDiscountedWithHighestAmount_itemsAreOnSpecialWithFixedRateAndPercentage() {
            Item banana = new Item(UUID.randomUUID(), "Banana", 12.00);
            Item potato = new Item(UUID.randomUUID(), "Potato", 18.00);

            var itemsForPurchase = List.of(
                    new ItemForPurchase(banana, 5, new Discount(10.00, 5)),
                    new ItemForPurchase(potato, 2)
            );

            var cart = new Cart(UUID.randomUUID(), itemsForPurchase);

            var cost = cart.getCost();

            Assertions.assertEquals(25, cost.getDiscountAmount());
            Assertions.assertEquals(96, cost.getGrossAmount());
        }

        @Test
        void allItemsAreDiscounted_storeWideCoupon() {
            Item banana = new Item(UUID.randomUUID(), "Banana", 12.00);
            Item potato = new Item(UUID.randomUUID(), "Potato", 18.00);

            var itemsForPurchase = List.of(
                    new ItemForPurchase(banana, 5, new Discount(10.00, 5)),
                    new ItemForPurchase(potato, 2)
            );

            var cart = new Cart(UUID.randomUUID(), itemsForPurchase);

            var coupon =
                    Coupon.StoreWide(
                            UUID.randomUUID(),
                            "ALL50",
                            new Discount(50, 0));

            cart.applyCoupon(coupon);

            var cost = cart.getCost();

            Assertions.assertEquals(48, cost.getDiscountAmount());
            Assertions.assertEquals(96, cost.getGrossAmount());
        }

        @Test
        void typesOfItemsAreDiscounted_typeOfItemCoupon() {
            var vegetableItemType =  new ItemType("Vegetable");
            var fruitItemType =  new ItemType("Fruit");
            Item banana = new Item(UUID.randomUUID(),"Banana", fruitItemType, 12.00);
            Item potato = new Item(UUID.randomUUID(),"Potato", vegetableItemType, 18.00);
            Item lettuce = new Item(UUID.randomUUID(),"Lettuce", vegetableItemType, 20.00);

            var itemsForPurchase = List.of(
                    new ItemForPurchase(banana, 5),
                    new ItemForPurchase(potato, 2),
                    new ItemForPurchase(lettuce, 2)
            );

            var cart = new Cart(UUID.randomUUID(), itemsForPurchase);

            var coupon = Coupon.ForItemType(
                    UUID.randomUUID(),
                    "VEG25",
                    new Discount(25, 0),
                    "Vegetable");

            cart.applyCoupon(coupon);
            var cost = cart.getCost();

            Assertions.assertEquals(19, cost.getDiscountAmount());
            Assertions.assertEquals(136, cost.getGrossAmount());
        }

        @Test
        void shippingCost_defaultCost_itemsHaveDefaultWeight() {
            var vegetableItemType =  new ItemType("Vegetable");
            var fruitItemType =  new ItemType("Fruit");
            Item banana = new Item(UUID.randomUUID(),"Banana", fruitItemType, 12.00);
            Item potato = new Item(UUID.randomUUID(),"Potato", vegetableItemType, 18.00);

            var itemsForPurchase = List.of(
                    new ItemForPurchase(banana, 5),
                    new ItemForPurchase(potato, 2)
            );

            var cart = new Cart(UUID.randomUUID(), itemsForPurchase);
            var cost = cart.getCost();

            Assertions.assertEquals(24.50, cost.getShippingCost());
            Assertions.assertEquals(120.50, cost.getTotalCost());
        }

        @Test
        void shippingCost_itemsHaveVariableWeight() {
            var vegetableItemType =  new ItemType("Vegetable");
            var fruitItemType =  new ItemType("Fruit");
            Item banana = new Item(UUID.randomUUID(),"Banana", fruitItemType, 12.00, Weight.grams(100));
            Item potato = new Item(UUID.randomUUID(),"Potato", vegetableItemType, 18.00);

            var itemsForPurchase = List.of(
                    new ItemForPurchase(banana, 5),
                    new ItemForPurchase(potato, 2)
            );

            var cart = new Cart(UUID.randomUUID(), itemsForPurchase);
            var cost = cart.getCost();

            Assertions.assertEquals(96.00, cost.getGrossAmount());
            Assertions.assertEquals(47.00, cost.getShippingCost());
            Assertions.assertEquals(143.00, cost.getTotalCost());
        }
    }

    public static class AddItemTests {
        @Test
        public void addItem() {
            Item banana = new Item(UUID.randomUUID(), "Banana", 12.00);
            Item potato = new Item(UUID.randomUUID(), "Potato", 18.00);

            var itemsForPurchase = List.of(
                    new ItemForPurchase(banana, 5)
            );

            var cart = new Cart(UUID.randomUUID(), itemsForPurchase);
            cart.addItem(new ItemForPurchase(potato, 2));

            var cost = cart.getCost();
            Assertions.assertEquals(0, cost.getDiscountAmount());
            Assertions.assertEquals(96, cost.getGrossAmount());
        }
    }
}