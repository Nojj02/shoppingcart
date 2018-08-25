package com.jll;

import com.google.common.collect.MoreCollectors;

import java.util.Collection;
import java.util.Optional;

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
            .mapToDouble(itemForPurchase ->
                getShoppingItem(itemForPurchase.getItemCode())
                    .map(item -> item.getPrice() * itemForPurchase.getQuantity())
                    .orElse(0.0))
            .sum();
    }

    public Optional<ShoppingItem> getShoppingItem(String itemCode) {
        return _shoppingItems.stream()
                .filter(shoppingItem -> shoppingItem.getItemCode() == itemCode)
                .collect(MoreCollectors.toOptional());
    }

    public class ShopException extends Exception {
    }

    public class NotEnoughItemsInShopException extends ShopException {
    }
}
