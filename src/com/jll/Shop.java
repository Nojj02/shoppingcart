package com.jll;

import java.util.Collection;
import java.util.List;

public class Shop
{
    private Collection<ShoppingItem> _shoppingItems;

    public Shop(Collection<ShoppingItem> items)
        throws NotEnoughItemsInShopException {
        if (items.size() < 10) {
            throw new NotEnoughItemsInShopException();
        }
        _shoppingItems = items;
    }

    public Collection<ShoppingItem> get_ShoppingItems()
    {
        return _shoppingItems;
    }

    public double Compute(Collection<ItemForPurchase> itemsForPurchase) {
        return itemsForPurchase.stream()
            .mapToDouble(itemForPurchase -> {
                try {
                    var matchingShoppingItem = _shoppingItems.stream()
                        .filter(shoppingItem -> shoppingItem.getItemCode() == itemForPurchase.getItemCode())
                        .findFirst()
                        .orElseThrow(ItemNotRegisteredException::new);

                    return matchingShoppingItem.getPrice() * itemForPurchase.getQuantity();
                } catch (ItemNotRegisteredException e) {
                    e.printStackTrace();
                    throw new RuntimeException();
                }
            })
            .sum();
    }

    public class ShopException extends Exception {
    }

    public class NotEnoughItemsInShopException extends ShopException {
    }

    public class ItemNotRegisteredException extends ShopException {
    }
}
