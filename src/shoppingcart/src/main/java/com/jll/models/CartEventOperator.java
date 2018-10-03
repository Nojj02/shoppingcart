package com.jll.models;

public interface CartEventOperator {
    void apply(Cart c, CartEvent e);
}
