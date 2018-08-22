package com.jll;

import java.util.Collection;

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

    public class NotEnoughItemsInShopException extends Exception {

    }
}
