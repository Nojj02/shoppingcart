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
        var percentageDiscount = this.discount.applyPercentageDiscount(this.price);
        var fixedAmountDiscount = this.price - this.discount.getFixedAmount();
        return Math.min(percentageDiscount, fixedAmountDiscount);
    }
}

