package com.jll;

public class Item
{
    private String itemCode;
    private ItemType itemType;
    private double price;
    private Discount discount;

    public Item(String itemCode, double price) {
        this(itemCode, ItemType.Unknown, price);
    }

    public Item(String itemCode, ItemType itemType, double price) {
        this.itemCode = itemCode;
        this.itemType = itemType;
        this.price = price;
        this.discount = Discount.None;
    }

    public double getPrice() {
        return this.price;
    }

    public String getItemCode() {
        return this.itemCode;
    }

    public void setDiscount(Discount discount) {
        this.discount = discount;
    }

    public double getDiscountedPrice() {
        var percentageDiscount = this.discount.applyPercentageDiscount(this.price);
        var fixedAmountDiscount = this.price - this.discount.getFixedAmount();
        return Math.min(percentageDiscount, fixedAmountDiscount);
    }

    public ItemType getItemType() {
        return itemType;
    }
}

