package com.jll.models;

import java.util.UUID;

public abstract class Coupon extends AggregateRoot {
    private String couponCode;
    private Discount discount;

    protected Coupon(UUID id, String couponCode, Discount discount) {
        super(id);
        this.couponCode = couponCode;
        this.discount = discount;
    }

    public String getCouponCode() {
        return couponCode;
    }

    public Discount getDiscount() {
        return discount;
    }

    public abstract boolean appliesTo(String itemTypeCode);
}



