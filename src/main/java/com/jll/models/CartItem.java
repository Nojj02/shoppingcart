package com.jll.models;

public class CartItem {
    private ItemForPurchase itemForPurchase;
    private Cost cost;

    public CartItem(
            ItemForPurchase itemForPurchase,
            Cost cost
    ) {
        this.itemForPurchase = itemForPurchase;
        this.cost = cost;
    }

    public Cost getCost() {
        return cost;
    }

    public ItemForPurchase getItemForPurchase() {
        return itemForPurchase;
    }

    public void updateCost(Cost cost) {
        this.cost = cost;
    }
}
