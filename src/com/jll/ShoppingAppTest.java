package com.jll;

import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;

import java.util.List;

import static org.junit.jupiter.api.Assertions.*;

public class ShoppingAppTest {
    @Test
    void shopHasItems() throws Shop.NotEnoughItemsInShopException {
        var firstItem = new ShoppingItem();
        var secondItem = new ShoppingItem();
        var items = List.of(
                firstItem,
                secondItem,
                new ShoppingItem(),
                new ShoppingItem(),
                new ShoppingItem(),
                new ShoppingItem(),
                new ShoppingItem(),
                new ShoppingItem(),
                new ShoppingItem(),
                new ShoppingItem()
        );

        var shop = new Shop(items);

        var itemList = shop.get_ShoppingItems().toArray();
        Assertions.assertEquals(itemList[0], firstItem);
        Assertions.assertEquals(itemList[1], secondItem);
    }

    @Test
    void error_shopHasLessThan10Items(){
        var firstItem = new ShoppingItem();
        var secondItem = new ShoppingItem();
        var items = List.of(
            firstItem,
            secondItem
        );

        Assertions.assertThrows(Shop.NotEnoughItemsInShopException.class, () -> new Shop(items));
    }
}