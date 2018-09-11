package com.jll.models;

public class StoreWideCoupon extends Coupon {
    public StoreWideCoupon(Discount discount) {
        super(discount);
    }

    @Override
    public boolean appliesTo(String itemTypeCode) {
        return true;
    }
}
