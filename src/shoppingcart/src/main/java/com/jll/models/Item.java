package com.jll.models;

public class Item extends AggregateRoot<ItemIdentity>
{
    private final String itemCode;
    private ItemType itemType;
    private double price;
    private Weight weight;
    private Discount discount;

    private Item() {
        super(ItemIdentity.Unknown);
        itemCode = null;
    }

    public Item(ItemIdentity id, String itemCode, double price) {
        this(id, itemCode, ItemType.Unknown, price, Weight.ZERO);
    }

    public Item(ItemIdentity id, String itemCode, ItemType itemType, double price) {
        this(id, itemCode, itemType, price, Weight.ZERO);
    }

    public Item(ItemIdentity id, String itemCode, ItemType itemType, double price, Weight weight) {
        super(id);
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

    public Discount getDiscount() {
        return this.discount;
    }

    public void setDiscount(Discount discount) {
        this.discount = discount;
    }

    public ItemType getItemType() {
        return itemType;
    }

    public Weight getWeight() {
        return weight;
    }
}

