package com.jll.model;

import java.util.UUID;

public class AggregateRoot<T extends Identity> {
    private T id;

    public AggregateRoot(T id) {
        this.id = id;
    }

    public UUID getId2() {
        return null;
    }

    public T getId() {
        return this.id;
    }
}
