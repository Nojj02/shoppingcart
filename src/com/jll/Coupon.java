package com.jll;

public class Coupon {
    private String itemCode;
    private Discount discount;

    private Coupon() {

    }

    public static Coupon storeWide(Discount discount) {
        var coupon = new Coupon();
        coupon.discount = discount;
        return coupon;
    }

    public String getItemCode() {
        return itemCode;
    }

    public Discount getDiscount() {
        return discount;
    }
}
