package com.jll.repositories;

import com.jll.models.Item;
import com.jll.models.ItemIdentity;
import com.jll.utilities.ConnectionManager;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;

import java.sql.SQLException;
import java.util.List;
import java.util.UUID;

public class ItemRepositoryTests {
    private final String url = "jdbc:postgresql://localhost:5432/postgres";
    private final String user = "postgres";
    private final String password = "thepassword";

    @Test
    public void saveAndGet() throws SQLException {
        var items = List.of(
                new Item(new ItemIdentity(UUID.randomUUID()), "Banana", 12.00),
                new Item(new ItemIdentity(UUID.randomUUID()), "Tomato", 3.00)
        );

        var itemRepository = new ItemRepository(getConnectionManager());
        for(var item : items) {
            itemRepository.save(item);
        }

        for(var item : items) {
            var retrievedItem = itemRepository.get(item.getId2());
            Assertions.assertNotNull(retrievedItem);
        }
    }

    public ConnectionManager getConnectionManager() {
        return new ConnectionManager(url, user, password);
    }
}

