package com.jll.models;

import java.util.UUID;

public class StoreWideCoupon extends Coupon {
    public StoreWideCoupon(UUID id, String couponCode, Discount discount) {
        super(id, couponCode, discount);
    }

    @Override
    public boolean appliesTo(String itemTypeCode) {
        return true;
    }
}
