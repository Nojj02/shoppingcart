package com.jll.dtos;

import com.jll.models.Coupon;

public class CouponDto {
    public String CouponCode;
    public DiscountDto Discount;
    public CouponType CouponType;
    public String ItemTypeCode;

    public CouponDto(Coupon coupon) {
        CouponCode = coupon.getCouponCode();
        Discount = new DiscountDto(coupon.getDiscount());
    }
}
