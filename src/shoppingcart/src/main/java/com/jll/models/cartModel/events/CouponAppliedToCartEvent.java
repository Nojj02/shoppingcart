package com.jll.models.cartModel.events;

import com.jll.models.Coupon;

public class CouponAppliedToCartEvent extends CartEvent {
    public Coupon coupon;
}
