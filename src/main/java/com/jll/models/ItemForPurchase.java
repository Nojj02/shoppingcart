package com.jll.models;

import java.util.UUID;

public class ItemForPurchase {
    private String itemTypeCode;
    private String itemCode;
    private final int quantity;
    private Discount discount;
    private UUID itemId;
    private double price;
    private Weight weight;

    public ItemForPurchase(Item item, int quantity, Discount discount) {
        this.itemId = item.getId();
        this.itemCode = item.getItemCode();
        this.itemTypeCode = item.getItemType().getItemTypeCode();
        this.price = item.getPrice();
        this.weight = item.getWeight();
        this.discount = item.getDiscount();
        this.quantity = quantity;
        this.discount = discount;
    }

    public ItemForPurchase(Item item, int quantity) {
        this(item, quantity, Discount.None);
    }

    public String getItemCode() {
        return itemCode;
    }

    public int getQuantity() {
        return quantity;
    }

    public Discount getDiscount() {
        return discount;
    }

    public UUID getItemId() {
        return itemId;
    }

    public double getPrice() {
        return price;
    }

    public Weight getWeight() {
        return weight;
    }

    public String getItemTypeCode() {
        return this.itemTypeCode;
    }
}
