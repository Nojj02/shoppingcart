package com.jll;

import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;

import java.sql.SQLException;
import java.util.List;

public class DbTest {
    private final String url = "jdbc:postgresql://localhost:5432/postgres";
    private final String user = "postgres";
    private final String password = "thepassword";

    @Test
    public void Test() throws SQLException {
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

        var shop = new Shop(items);

        var shopRepository = new ShopRepository(getConnectionManager());
        shopRepository.save(shop);
    }

    public ConnectionManager getConnectionManager() {
        return new ConnectionManager(url, user, password);
    }
}

