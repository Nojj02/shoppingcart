package com.jll.models;

import java.util.UUID;

public class AggregateRoot {
    private final UUID id;

    public AggregateRoot(UUID id) {
        this.id = id;
    }

    public UUID getId() {
        return this.id;
    }
}
