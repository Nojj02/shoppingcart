package com.jll.models;

import java.util.UUID;

public class Coupon extends AggregateRoot<CouponIdentity> {
    private String couponCode;
    private Discount discount;
    private CouponType couponType;
    private String itemTypeCode;

    public static Coupon storeWide(
            CouponIdentity id,
            String couponCode,
            Discount discount) {
        var coupon = new Coupon(id, couponCode, discount);
        coupon.couponType = CouponType.StoreWide;
        return coupon;
    }

    public static Coupon forItemType(
            CouponIdentity id,
            String couponCode,
            Discount discount,
            String itemTypeCode) {
        var coupon = new Coupon(id, couponCode, discount);
        coupon.couponType = CouponType.ItemType;
        coupon.itemTypeCode = itemTypeCode;
        return coupon;
    }

    private Coupon() {
        super(CouponIdentity.Unknown);
    }

    protected Coupon(CouponIdentity id, String couponCode, Discount discount) {
        super(id);
        this.couponCode = couponCode;
        this.discount = discount;
    }

    public CouponType getCouponType() { return couponType; }

    public String getCouponCode() {
        return couponCode;
    }

    public Discount getDiscount() {
        return discount;
    }

    public String getItemTypeCode() {
        return itemTypeCode;
    }

    public boolean appliesTo(String itemTypeCode) {
        if (couponType == CouponType.ItemType) {
            return this.itemTypeCode == itemTypeCode;
        } else if (couponType == CouponType.StoreWide){
            return true;
        }
        return false;
    }
}


