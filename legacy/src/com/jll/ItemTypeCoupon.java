package com.jll;

public class ItemTypeCoupon extends Coupon {
    private String itemTypeCode;

    public ItemTypeCoupon(String itemTypeCode, Discount discount) {
        super(discount);
        this.itemTypeCode = itemTypeCode;
    }

    public String getItemTypeCode() {
        return itemTypeCode;
    }

    @Override
    public boolean appliesTo(String itemTypeCode) {
        return this.itemTypeCode == itemTypeCode;
    }
}
