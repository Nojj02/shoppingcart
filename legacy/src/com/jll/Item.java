package com.jll;

public class Item
{
    private String itemCode;
    private ItemType itemType;
    private double price;
    private Weight weight;
    private Discount discount;

    public Item(String itemCode, double price) {
        this(itemCode, ItemType.Unknown, price, Weight.ZERO);
    }

    public Item(String itemCode, ItemType itemType, double price) {
        this(itemCode, itemType, price, Weight.ZERO);
    }

    public Item(String itemCode, ItemType itemType, double price, Weight weight) {
        this.itemCode = itemCode;
        this.itemType = itemType;
        this.price = price;
        this.weight = weight;
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

    public double getDiscountAmount() {
        var percentageDiscount = this.discount.computeDiscount(this.price);
        var fixedAmountDiscount = this.discount.getFixedAmount();
        return Math.max(percentageDiscount, fixedAmountDiscount);
    }

    public double getDiscountedPrice() {
        var percentageDiscount = this.discount.applyPercentageDiscount(this.price);
        var fixedAmountDiscount = this.price - this.discount.getFixedAmount();
        return Math.min(percentageDiscount, fixedAmountDiscount);
    }

    public ItemType getItemType() {
        return itemType;
    }

    public Weight getWeight() {
        return weight;
    }
}

