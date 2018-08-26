package com.jll;

public class Coupon {
    private String itemCode;
    private Discount discount;

    public Coupon(String itemCode, Discount discount) {
        this.itemCode = itemCode;
        this.discount = discount;
    }

    public String getItemCode() {
        return itemCode;
    }

    public Discount getDiscount() {
        return discount;
    }
}
