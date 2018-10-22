package com.jll.models;

import java.util.UUID;

public class ItemIdentity extends Identity {
    public static ItemIdentity Unknown = new ItemIdentity(new UUID(0, 0));

    public ItemIdentity(
            UUID itemId
    ) {
        super(itemId);
    }
}

