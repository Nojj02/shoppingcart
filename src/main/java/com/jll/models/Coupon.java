package com.jll.models;

public abstract class Coupon {
    private Discount discount;

    protected Coupon(Discount discount) {
        this.discount = discount;
    }

    public Discount getDiscount() {
        return discount;
    }

    public abstract boolean appliesTo(String itemTypeCode);
}



