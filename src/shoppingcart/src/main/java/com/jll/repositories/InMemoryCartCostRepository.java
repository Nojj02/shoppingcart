package com.jll.repositories;

import com.jll.model.CartCost;
import com.jll.model.CartCostIdentity;
import com.jll.repositories.CartCostRepository;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

public class InMemoryCartCostRepository
    implements CartCostRepository {

    public List<CartCost> cartCosts = new ArrayList<>();

    @Override
    public Optional<CartCost> get(CartCostIdentity cartCostIdentity) {
        return cartCosts.stream()
                .filter(x -> x.getId().isEqualTo(cartCostIdentity))
                .findFirst();
    }

    @Override
    public void save(CartCost aggregateRoot) {
        cartCosts.add(aggregateRoot);
    }
}
