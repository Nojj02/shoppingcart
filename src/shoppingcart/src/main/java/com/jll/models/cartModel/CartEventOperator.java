package com.jll.models.cartModel;

import com.jll.models.cartModel.events.CartEvent;

public interface CartEventOperator {
    void apply(Cart c, CartEvent e);
}
