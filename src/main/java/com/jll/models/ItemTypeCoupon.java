package com.jll.models;

import java.util.UUID;

public class ItemTypeCoupon extends Coupon {
    private String itemTypeCode;

    public ItemTypeCoupon(UUID id, String code, String itemTypeCode, Discount discount) {
        super(id, code, discount);
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
