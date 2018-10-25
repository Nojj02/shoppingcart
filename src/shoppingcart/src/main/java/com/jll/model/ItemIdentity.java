package com.jll.model;

import java.util.UUID;

public class ItemIdentity extends Identity {
    public static ItemIdentity Unknown = new ItemIdentity(new UUID(0, 0));

    public static ItemIdentity generateNew() {
        return new ItemIdentity(UUID.randomUUID());
    }

    public ItemIdentity(UUID itemId) {
        super(itemId);
    }
}

