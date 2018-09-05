package com.jll;

import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;

import java.sql.SQLException;
import java.util.List;
import java.util.UUID;

public class ShopRepositoryTests {
    private final String url = "jdbc:postgresql://localhost:5432/postgres";
    private final String user = "postgres";
    private final String password = "thepassword";

    @Test
    public void saveAndGet() throws SQLException {
        var items = List.of(
                new Item("Banana", 12.00),
                new Item("Tomato", 3.00),
                new Item("Potato", 18.00),
                new Item("Apple", 20.00),
                new Item("Hammer", 175.00),
                new Item("Wrench", 250.00),
                new Item("Lettuce", 20.00),
                new Item("Orange", 22.00),
                new Item("Screwdriver", 220.00),
                new Item("Pear", 25.00)
        );

        var id = UUID.randomUUID();

        var shop = new Shop(id, items);

        var shopRepository = new ShopRepository(getConnectionManager());
        shopRepository.save(shop);

        var retrievedShop = shopRepository.get(id);
        Assertions.assertNotNull(retrievedShop);
    }

    public ConnectionManager getConnectionManager() {
        return new ConnectionManager(url, user, password);
    }
}

