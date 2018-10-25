package com.jll.model;

import java.util.UUID;

public class CartCostIdentity
    extends Identity {
    public static CartCostIdentity generateNew() {
        return new CartCostIdentity(UUID.randomUUID());
    }

    public static CartCostIdentity fromExisting(UUID id) {
        return new CartCostIdentity(id);
    }

    private CartCostIdentity(UUID value) {
        super(value);
    }
}
