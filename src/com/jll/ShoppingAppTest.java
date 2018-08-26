package com.jll;

import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;

import java.util.List;

public class ShoppingAppTest {
    public static class CreationTests {
        @Test
        void shopHasItems() throws Shop.NotEnoughItemsInShopException {
            var firstItem = new ShoppingItem("Banana", 15.00);
            var secondItem = new ShoppingItem("Tomato", 22.00);
            var items = List.of(
                    firstItem,
                    secondItem,
                    new ShoppingItem("Potato", 18.00),
                    new ShoppingItem("Apple", 20.00),
                    new ShoppingItem("Hammer", 175.00),
                    new ShoppingItem("Wrench", 250.00),
                    new ShoppingItem("Lettuce", 20.00),
                    new ShoppingItem("Orange", 22.00),
                    new ShoppingItem("Screwdriver", 220.00),
                    new ShoppingItem("Pear", 25.00)
            );

            var shop = new Shop(items);

            var itemList = shop.get_ShoppingItems().toArray();
            Assertions.assertEquals(itemList[0], firstItem);
            Assertions.assertEquals(itemList[1], secondItem);
        }

        @Test
        void error_shopHasLessThan10Items() {
            var firstItem = new ShoppingItem("Banana", 15.00);
            var secondItem = new ShoppingItem("Tomato", 22.00);
            var items = List.of(
                    firstItem,
                    secondItem
            );

            Assertions.assertThrows(Shop.NotEnoughItemsInShopException.class, () -> new Shop(items));
        }
    }

    public static class ComputeTests {
        @Test
        void computesTotalCost()
            throws Shop.NotEnoughItemsInShopException {
            var items = List.of(
                new ShoppingItem("Banana", 12.00),
                new ShoppingItem("Tomato", 3.00),
                new ShoppingItem("Potato", 18.00),
                new ShoppingItem("Apple", 20.00),
                new ShoppingItem("Hammer", 175.00),
                new ShoppingItem("Wrench", 250.00),
                new ShoppingItem("Lettuce", 20.00),
                new ShoppingItem("Orange", 22.00),
                new ShoppingItem("Screwdriver", 220.00),
                new ShoppingItem("Pear", 25.00)
            );

            var shop = new Shop(items);

            var itemsForPurchase = List.of(
                new ItemForPurchase("Banana", 5),
                new ItemForPurchase("Potato", 2)
            );

            var result = shop.Compute(itemsForPurchase);

            Assertions.assertEquals(96, result);
        }

        @Test
        void itemsAreDiscounted_itemsAreOnSpecialWithPercentage()
                throws Shop.NotEnoughItemsInShopException {
            var items = List.of(
                    new ShoppingItem("Banana", 12.00),
                    new ShoppingItem("Tomato", 3.00),
                    new ShoppingItem("Potato", 18.00),
                    new ShoppingItem("Apple", 20.00),
                    new ShoppingItem("Hammer", 175.00),
                    new ShoppingItem("Wrench", 250.00),
                    new ShoppingItem("Lettuce", 20.00),
                    new ShoppingItem("Orange", 22.00),
                    new ShoppingItem("Screwdriver", 220.00),
                    new ShoppingItem("Pear", 25.00)
            );

            var shop = new Shop(items);

            shop.setDiscount("Banana", new Discount(50.00, 0));

            var itemsForPurchase = List.of(
                    new ItemForPurchase("Banana", 5),
                    new ItemForPurchase("Potato", 2)
            );

            var result = shop.Compute(itemsForPurchase);

            Assertions.assertEquals(66, result);
        }

        @Test
        void itemsAreDiscounted_itemsAreOnSpecialWithFixedRate()
                throws Shop.NotEnoughItemsInShopException {
            var items = List.of(
                    new ShoppingItem("Banana", 12.00),
                    new ShoppingItem("Tomato", 3.00),
                    new ShoppingItem("Potato", 18.00),
                    new ShoppingItem("Apple", 20.00),
                    new ShoppingItem("Hammer", 175.00),
                    new ShoppingItem("Wrench", 250.00),
                    new ShoppingItem("Lettuce", 20.00),
                    new ShoppingItem("Orange", 22.00),
                    new ShoppingItem("Screwdriver", 220.00),
                    new ShoppingItem("Pear", 25.00)
            );

            var shop = new Shop(items);

            shop.setDiscount("Banana", new Discount(0, 3));

            var itemsForPurchase = List.of(
                    new ItemForPurchase("Banana", 5),
                    new ItemForPurchase("Potato", 2)
            );

            var result = shop.Compute(itemsForPurchase);

            Assertions.assertEquals(81, result);
        }

        @Test
        void itemsAreDiscountedWithHighestAmount_itemsAreOnSpecialWithFixedRateAndPercentage()
                throws Shop.NotEnoughItemsInShopException {
            var items = List.of(
                    new ShoppingItem("Banana", 12.00),
                    new ShoppingItem("Tomato", 3.00),
                    new ShoppingItem("Potato", 18.00),
                    new ShoppingItem("Apple", 20.00),
                    new ShoppingItem("Hammer", 175.00),
                    new ShoppingItem("Wrench", 250.00),
                    new ShoppingItem("Lettuce", 20.00),
                    new ShoppingItem("Orange", 22.00),
                    new ShoppingItem("Screwdriver", 220.00),
                    new ShoppingItem("Pear", 25.00)
            );

            var shop = new Shop(items);

            shop.setDiscount("Banana", new Discount(10.0, 5));

            var itemsForPurchase = List.of(
                    new ItemForPurchase("Banana", 5),
                    new ItemForPurchase("Potato", 2)
            );

            var result = shop.Compute(itemsForPurchase);

            Assertions.assertEquals(71, result);
        }

        @Test
        void itemsAreDiscounted_storeWideCoupon()
                throws Shop.NotEnoughItemsInShopException {
                var items = List.of(
                        new ShoppingItem("Banana", 12.00),
                        new ShoppingItem("Tomato", 3.00),
                        new ShoppingItem("Potato", 18.00),
                        new ShoppingItem("Apple", 20.00),
                        new ShoppingItem("Hammer", 175.00),
                        new ShoppingItem("Wrench", 250.00),
                        new ShoppingItem("Lettuce", 20.00),
                        new ShoppingItem("Orange", 22.00),
                        new ShoppingItem("Screwdriver", 220.00),
                        new ShoppingItem("Pear", 25.00)
                );

                var shop = new Shop(items);

                var itemsForPurchase = List.of(
                        new ItemForPurchase("Banana", 5),
                        new ItemForPurchase("Potato", 2)
                );

                var coupon = new Coupon("Banana", new Discount(50, 0));

                var result = shop.Compute(itemsForPurchase, coupon);

                Assertions.assertEquals(66, result);
            }
    }
}