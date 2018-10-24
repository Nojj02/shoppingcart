package com.jll.model;

public class ItemForPurchase {
    private int quantity;
    private ItemIdentity itemId;

    private ItemForPurchase(){
        quantity = 0;
    }

    public static ItemForPurchase createItemForPurchase(ItemIdentity itemIdentity, int quantity) {
        var itemForPurchase = new ItemForPurchase();
        itemForPurchase.itemId = itemIdentity;
        itemForPurchase.quantity = quantity;
        return itemForPurchase;
    }

    public int getQuantity() {
        return quantity;
    }

    public ItemIdentity getItemId() {
        return itemId;
    }
}

