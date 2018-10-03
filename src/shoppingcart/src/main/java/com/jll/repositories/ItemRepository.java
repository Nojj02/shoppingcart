package com.jll.repositories;

import com.jll.models.Item;
import com.jll.utilities.ConnectionManager;

public class ItemRepository extends Repository<Item> {
    public ItemRepository(ConnectionManager connectionManager) {
        super(connectionManager, Item.class, "item");
    }
}


