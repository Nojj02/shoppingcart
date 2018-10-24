package com.jll.model;

public class CartCost
    extends AggregateRoot<CartCostIdentity> {
    public CartCost(CartCostIdentity id) {
        super(id);
    }
}
