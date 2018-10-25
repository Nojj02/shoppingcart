package com.jll.model;

import java.util.UUID;

public class CouponIdentity extends Identity {
    public static CouponIdentity Unknown = new CouponIdentity(UUID.randomUUID());

    public static CouponIdentity generateNew() {
        return new CouponIdentity(UUID.randomUUID());
    }

    public static CouponIdentity fromExisting(UUID id) {
        return new CouponIdentity(id);
    }

    private CouponIdentity(UUID value) {
        super(value);
    }

}
