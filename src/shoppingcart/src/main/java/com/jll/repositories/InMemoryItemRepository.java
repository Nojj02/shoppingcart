package com.jll.repositories;

import com.jll.model.Item;
import com.jll.model.ItemIdentity;
import com.jll.repositories.ItemRepository;

import java.sql.SQLException;
import java.util.List;
import java.util.Optional;

public class InMemoryItemRepository implements ItemRepository {

    private List<Item> items;

    public InMemoryItemRepository(List<Item> items) {

        this.items = items;
    }

    @Override
    public Optional<Item> get(ItemIdentity itemIdentity) {
        return items.stream()
                .filter(x -> x.getId().isEqualTo(itemIdentity))
                .findFirst();
    }

    @Override
    public void save(Item aggregateRoot) {
        this.items.add(aggregateRoot);
    }
}
