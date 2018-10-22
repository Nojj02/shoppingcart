package com.jll.models;

import java.util.UUID;

public abstract class Identity {
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
}
