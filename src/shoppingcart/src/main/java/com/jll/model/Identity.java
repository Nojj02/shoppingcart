package com.jll.model;

import java.util.UUID;

public abstract class Identity
        extends ValueObject<Identity> {
    private UUID value;

    protected Identity(
            UUID value
    ) {
        this.value = value;
    }

    public UUID getValue() {
        return value;
    }

    public boolean matches(UUID idAsUuid) {
        return idAsUuid.equals(this.value);
    }

    @Override
    public Boolean isEqualTo(Identity other) {
        return this.value == other.value;
    }
}

