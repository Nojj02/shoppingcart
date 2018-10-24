package com.jll.repositories;

import com.jll.model.Item;
import com.jll.model.ItemIdentity;
import com.jll.utilities.ConnectionManager;

public class ItemPostgresRepository
        extends PostgresRepository<Item, ItemIdentity>
        implements ItemRepository {
    public ItemPostgresRepository(ConnectionManager connectionManager) {
        super(connectionManager, Item.class, "item");
    }
}

