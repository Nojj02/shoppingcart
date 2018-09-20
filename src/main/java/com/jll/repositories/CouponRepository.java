package com.jll.repositories;

import com.jll.models.Cart;
import com.jll.models.Coupon;
import com.jll.utilities.ConnectionManager;

public class CouponRepository extends Repository<Coupon> {
    public CouponRepository(ConnectionManager connectionManager) {
        super(connectionManager, Coupon.class, "coupon");
    }
}
