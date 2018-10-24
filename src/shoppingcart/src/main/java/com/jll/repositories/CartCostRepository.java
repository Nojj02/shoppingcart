package com.jll.repositories;

import com.jll.model.CartCost;
import com.jll.model.CartCostIdentity;
import com.jll.repositories.Repository;

public interface CartCostRepository extends Repository<CartCost, CartCostIdentity> {
    void save(CartCost aggregateRoot);
}
