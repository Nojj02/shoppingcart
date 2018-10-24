package com.jll.repositories;

import com.jll.model.Cart;
import com.jll.utilities.ConnectionManager;

public class CartRepository extends EventingRepository<Cart> {
    public CartRepository(ConnectionManager connectionManager) {
        super(connectionManager, "cart");
    }
}
