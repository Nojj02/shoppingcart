package com.jll.models;

public class ItemType {
    public final static ItemType Unknown = new ItemType("Unknown");

    private String itemTypeCode;

    public ItemType(String itemTypeCode) {
        this.itemTypeCode = itemTypeCode;
    }

    public String getItemTypeCode() {
        return itemTypeCode;
    }
}
