package com.jll.model;

public abstract class ValueObject<T> {
    public abstract Boolean isEqualTo(T other);
}
