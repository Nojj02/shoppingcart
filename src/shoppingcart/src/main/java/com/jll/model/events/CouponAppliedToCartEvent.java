package com.jll.model.events;

import com.jll.model.Coupon;

public class CouponAppliedToCartEvent extends CartEvent {
    public Coupon coupon;
}
