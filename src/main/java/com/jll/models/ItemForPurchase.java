package com.jll.models;

public class ItemForPurchase {
    private String itemCode;
    private final int quantity;

    public ItemForPurchase(String itemCode, int quantity) {
        this.itemCode = itemCode;
        this.quantity = quantity;
    }

    public String getItemCode() {
        return itemCode;
    }

    public int getQuantity() {
        return quantity;
    }
}
