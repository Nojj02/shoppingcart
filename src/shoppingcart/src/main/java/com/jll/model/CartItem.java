package com.jll.model;

public class CartItem {
    private ItemForPurchase itemForPurchase;

    private CartItem() {
    }

    public CartItem(
            ItemForPurchase itemForPurchase
    ) {
        this.itemForPurchase = itemForPurchase;
    }

    public ItemForPurchase getItemForPurchase() {
        return itemForPurchase;
    }
}
