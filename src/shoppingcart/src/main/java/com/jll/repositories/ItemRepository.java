package com.jll.repositories;

import com.jll.model.Item;
import com.jll.model.ItemIdentity;
import com.jll.utilities.ConnectionManager;

public class ItemRepository extends Repository<Item, ItemIdentity> {
    public ItemRepository(ConnectionManager connectionManager) {
        super(connectionManager, Item.class, "item");
    }
}