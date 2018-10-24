package com.jll.dtos;

import com.jll.model.*;

public class CouponDto {
    public String CouponCode;
    public DiscountDto Discount;
    public com.jll.model.CouponType CouponType;
    public String ItemTypeCode;

    public CouponDto(Coupon coupon) {
        CouponCode = coupon.getCouponCode();
        Discount = new DiscountDto(coupon.getDiscount());
        CouponType = coupon.getCouponType();
        ItemTypeCode = coupon.getItemTypeCode();
    }
}
