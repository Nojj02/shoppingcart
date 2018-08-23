package com.jll;

public class ShoppingItem
{
    private String itemCode;
    private double price;

    public ShoppingItem(String itemCode, double price) {
        this.itemCode = itemCode;

        this.price = price;
    }

    public double getPrice() {
        return this.price;
    }

    public String getItemCode() {
        return itemCode;
    }
}

