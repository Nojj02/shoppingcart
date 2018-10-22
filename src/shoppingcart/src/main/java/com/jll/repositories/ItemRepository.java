package com.jll.repositories;

import com.jll.models.Item;
import com.jll.models.ItemIdentity;
import com.jll.utilities.ConnectionManager;

public class ItemRepository extends Repository<Item, ItemIdentity> {
    public ItemRepository(ConnectionManager connectionManager) {
        super(connectionManager, Item.class, "item");
    }
}