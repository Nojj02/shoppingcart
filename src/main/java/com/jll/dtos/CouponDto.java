package com.jll.dtos;

import com.jll.models.Coupon;
import com.jll.models.ItemType;
import com.jll.models.ItemTypeCoupon;
import com.jll.models.StoreWideCoupon;

public class CouponDto {
    public String CouponCode;
    public DiscountDto Discount;
    public CouponType CouponType;
    public String ItemTypeCode;

    public CouponDto(Coupon coupon) {
        CouponCode = coupon.getCouponCode();
        Discount = new DiscountDto(coupon.getDiscount());
        if (coupon instanceof StoreWideCoupon) {
            CouponType = CouponType.StoreWide;
        } else if (coupon instanceof ItemTypeCoupon) {
            var itemTypeCoupon = (ItemTypeCoupon)coupon;
            CouponType = CouponType.ItemType;
            ItemTypeCode = itemTypeCoupon.getItemTypeCode();
        }
    }
}
