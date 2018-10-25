package com.jll.model;

public class CartCost
    extends AggregateRoot<CartCostIdentity> {
    private Cost cost;

    public CartCost(CartCostIdentity id, Cost cost) {
        super(id);
        this.cost = cost;
    }

    public Cost getCost() {
        return cost;
    }
}
