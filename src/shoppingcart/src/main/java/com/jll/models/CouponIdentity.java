package com.jll.models;

import java.util.UUID;

public class CouponIdentity extends Identity {
    public static CouponIdentity Unknown = new CouponIdentity(UUID.randomUUID());

    public CouponIdentity(UUID value) {
        super(value);
    }
}
