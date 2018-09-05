package com.jll;

import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;

import java.util.List;
import java.util.UUID;

public class ShoppingAppTest {
    public static class CreationTests {
        @Test
        void shopHasItems() {
            var firstItem = new Item("Banana", 15.00);
            var secondItem = new Item("Tomato", 22.00);
            var items = List.of(
                    firstItem,
                    secondItem,
                    new Item("Potato", 18.00),
                    new Item("Apple", 20.00),
                    new Item("Hammer", 175.00),
                    new Item("Wrench", 250.00),
                    new Item("Lettuce", 20.00),
                    new Item("Orange", 22.00),
                    new Item("Screwdriver", 220.00),
                    new Item("Pear", 25.00)
            );

            var shop = new Shop(UUID.randomUUID(), items);

            var itemList = shop.getShoppingItems().toArray();
            Assertions.assertEquals(itemList[0], firstItem);
            Assertions.assertEquals(itemList[1], secondItem);
        }

        @Test
        void error_shopHasLessThan10Items() {
            var firstItem = new Item("Banana", 15.00);
            var secondItem = new Item("Tomato", 22.00);
            var items = List.of(
                    firstItem,
                    secondItem
            );

            Assertions.assertThrows(Shop.NotEnoughItemsInShopException.class, () -> new Shop(UUID.randomUUID(), items));
        }
    }

    public static class ComputeTests {
        @Test
        void computesTotalCost() {
            var items = List.of(
                    new Item("Banana", 12.00),
                    new Item("Tomato", 3.00),
                    new Item("Potato", 18.00),
                    new Item("Apple", 20.00),
                    new Item("Hammer", 175.00),
                    new Item("Wrench", 250.00),
                    new Item("Lettuce", 20.00),
                    new Item("Orange", 22.00),
                    new Item("Screwdriver", 220.00),
                    new Item("Pear", 25.00)
            );

            var shop = new Shop(UUID.randomUUID(), items);

            var itemsForPurchase = List.of(
                    new ItemForPurchase("Banana", 5),
                    new ItemForPurchase("Potato", 2)
            );

            var cost = shop.compute(itemsForPurchase);

            Assertions.assertEquals(0, cost.discountAmount);
            Assertions.assertEquals(96, cost.grossAmount);
        }

        @Test
        void itemsAreDiscounted_itemsAreOnSpecialWithPercentage() {
            var items = List.of(
                    new Item("Banana", 12.00),
                    new Item("Tomato", 3.00),
                    new Item("Potato", 18.00),
                    new Item("Apple", 20.00),
                    new Item("Hammer", 175.00),
                    new Item("Wrench", 250.00),
                    new Item("Lettuce", 20.00),
                    new Item("Orange", 22.00),
                    new Item("Screwdriver", 220.00),
                    new Item("Pear", 25.00)
            );

            var shop = new Shop(UUID.randomUUID(), items);

            shop.setDiscount("Banana", new Discount(50.00, 0));

            var itemsForPurchase = List.of(
                    new ItemForPurchase("Banana", 5),
                    new ItemForPurchase("Potato", 2)
            );

            var cost = shop.compute(itemsForPurchase);

            Assertions.assertEquals(30, cost.discountAmount);
            Assertions.assertEquals(96, cost.grossAmount);
        }

        @Test
        void itemsAreDiscounted_itemsAreOnSpecialWithFixedRate() {
            var items = List.of(
                    new Item("Banana", 12.00),
                    new Item("Tomato", 3.00),
                    new Item("Potato", 18.00),
                    new Item("Apple", 20.00),
                    new Item("Hammer", 175.00),
                    new Item("Wrench", 250.00),
                    new Item("Lettuce", 20.00),
                    new Item("Orange", 22.00),
                    new Item("Screwdriver", 220.00),
                    new Item("Pear", 25.00)
            );

            var shop = new Shop(UUID.randomUUID(), items);

            shop.setDiscount("Banana", new Discount(0, 3));

            var itemsForPurchase = List.of(
                    new ItemForPurchase("Banana", 5),
                    new ItemForPurchase("Potato", 2)
            );

            var cost = shop.compute(itemsForPurchase);

            Assertions.assertEquals(15, cost.discountAmount);
            Assertions.assertEquals(96, cost.grossAmount);
        }

        @Test
        void itemsAreDiscountedWithHighestAmount_itemsAreOnSpecialWithFixedRateAndPercentage() {
            var items = List.of(
                    new Item("Banana", 12.00),
                    new Item("Tomato", 3.00),
                    new Item("Potato", 18.00),
                    new Item("Apple", 20.00),
                    new Item("Hammer", 175.00),
                    new Item("Wrench", 250.00),
                    new Item("Lettuce", 20.00),
                    new Item("Orange", 22.00),
                    new Item("Screwdriver", 220.00),
                    new Item("Pear", 25.00)
            );

            var shop = new Shop(UUID.randomUUID(), items);

            shop.setDiscount("Banana", new Discount(10.0, 5));

            var itemsForPurchase = List.of(
                    new ItemForPurchase("Banana", 5),
                    new ItemForPurchase("Potato", 2)
            );

            var cost = shop.compute(itemsForPurchase);

            Assertions.assertEquals(25, cost.discountAmount);
            Assertions.assertEquals(96, cost.grossAmount);
        }

        @Test
        void allItemsAreDiscounted_storeWideCoupon() {
            var items = List.of(
                    new Item("Banana", 12.00),
                    new Item("Tomato", 3.00),
                    new Item("Potato", 18.00),
                    new Item("Apple", 20.00),
                    new Item("Hammer", 175.00),
                    new Item("Wrench", 250.00),
                    new Item("Lettuce", 20.00),
                    new Item("Orange", 22.00),
                    new Item("Screwdriver", 220.00),
                    new Item("Pear", 25.00)
            );

            var shop = new Shop(UUID.randomUUID(), items);

            var itemsForPurchase = List.of(
                    new ItemForPurchase("Banana", 5),
                    new ItemForPurchase("Potato", 2)
            );

            var coupon = new StoreWideCoupon(new Discount(50, 0));

            var cost = shop.compute(itemsForPurchase, coupon);

            Assertions.assertEquals(48, cost.discountAmount);
            Assertions.assertEquals(96, cost.grossAmount);
        }

        @Test
        void typesOfItemsAreDiscounted_typeOfItemCoupon() {
            var vegetableItemType =  new ItemType("Vegetable");
            var fruitItemType =  new ItemType("Fruit");
            var toolItemType =  new ItemType("Tool");
            var items = List.of(
                    new Item("Banana", fruitItemType, 12.00),
                    new Item("Tomato", fruitItemType, 3.00),
                    new Item("Potato", vegetableItemType, 18.00),
                    new Item("Apple", fruitItemType, 20.00),
                    new Item("Hammer", toolItemType, 175.00),
                    new Item("Wrench", toolItemType, 250.00),
                    new Item("Lettuce", vegetableItemType, 20.00),
                    new Item("Orange", fruitItemType, 22.00),
                    new Item("Screwdriver", toolItemType, 220.00),
                    new Item("Pear", fruitItemType, 25.00)
            );

            var shop = new Shop(UUID.randomUUID(), items);

            var itemsForPurchase = List.of(
                    new ItemForPurchase("Banana", 5),
                    new ItemForPurchase("Potato", 2),
                    new ItemForPurchase("Lettuce", 2)
            );

            var coupon = new ItemTypeCoupon("Vegetable", new Discount(25, 0));

            var cost = shop.compute(itemsForPurchase, coupon);

            Assertions.assertEquals(19, cost.discountAmount);
            Assertions.assertEquals(136, cost.grossAmount);
        }

        @Test
        void shippingCost_defaultCost_itemsHaveDefaultWeight() {
            var vegetableItemType =  new ItemType("Vegetable");
            var fruitItemType =  new ItemType("Fruit");
            var toolItemType =  new ItemType("Tool");
            var items = List.of(
                    new Item("Banana", fruitItemType, 12.00),
                    new Item("Tomato", fruitItemType, 3.00),
                    new Item("Potato", vegetableItemType, 18.00),
                    new Item("Apple", fruitItemType, 20.00),
                    new Item("Hammer", toolItemType, 175.00),
                    new Item("Wrench", toolItemType, 250.00),
                    new Item("Lettuce", vegetableItemType, 20.00),
                    new Item("Orange", fruitItemType, 22.00),
                    new Item("Screwdriver", toolItemType, 220.00),
                    new Item("Pear", fruitItemType, 25.00)
            );

            var shop = new Shop(UUID.randomUUID(), items);

            var itemsForPurchase = List.of(
                    new ItemForPurchase("Banana", 5),
                    new ItemForPurchase("Potato", 2)
            );

            var cost = shop.compute(itemsForPurchase);

            Assertions.assertEquals(24.50, cost.shippingCost);
            Assertions.assertEquals(120.50, cost.getTotalCost());
        }

        @Test
        void shippingCost_itemsHaveVariableWeight() {
            var vegetableItemType =  new ItemType("Vegetable");
            var fruitItemType =  new ItemType("Fruit");
            var toolItemType =  new ItemType("Tool");
            var items = List.of(
                    new Item("Banana", fruitItemType, 12.00, Weight.grams(100)),
                    new Item("Tomato", fruitItemType, 3.00),
                    new Item("Potato", vegetableItemType, 18.00),
                    new Item("Apple", fruitItemType, 20.00),
                    new Item("Hammer", toolItemType, 175.00),
                    new Item("Wrench", toolItemType, 250.00),
                    new Item("Lettuce", vegetableItemType, 20.00),
                    new Item("Orange", fruitItemType, 22.00),
                    new Item("Screwdriver", toolItemType, 220.00),
                    new Item("Pear", fruitItemType, 25.00)
            );

            var shop = new Shop(UUID.randomUUID(), items);

            var itemsForPurchase = List.of(
                    new ItemForPurchase("Banana", 5),
                    new ItemForPurchase("Potato", 2)
            );

            var cost = shop.compute(itemsForPurchase);

            Assertions.assertEquals(96.00, cost.grossAmount);
            Assertions.assertEquals(47.00, cost.shippingCost);
            Assertions.assertEquals(143.00, cost.getTotalCost());
        }
    }
}