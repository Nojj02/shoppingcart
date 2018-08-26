package com.jll;

public class ShoppingItem
{
    private String itemCode;
    private double price;
    private Discount discount;

    public ShoppingItem(String itemCode, double price) {
        this.itemCode = itemCode;
        this.price = price;
        this.discount = Discount.None;
    }

    public double getPrice() {
        return this.price;
    }

    public String getItemCode() {
        return itemCode;
    }

    public void setDiscount(Discount discount) {
        this.discount = discount;
    }

    public double getDiscountedPrice() {
        return this.price - (this.price * (this.discount.getPercentage() / 100)) - this.discount.getFixedAmount();
    }
}

