package com.jll.model;

import com.jll.model.events.CartEvent;

public interface CartEventOperator {
    void apply(Cart c, CartEvent e);
}
