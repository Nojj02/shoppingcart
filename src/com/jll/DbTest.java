package com.jll;

import com.google.gson.Gson;
import org.junit.jupiter.api.Test;
import org.postgresql.util.PGobject;

import java.sql.*;
import java.util.List;
import java.util.UUID;

public class DbTest {
    private final String url = "jdbc:postgresql://localhost:5432/postgres";
    private final String user = "postgres";
    private final String password = "thepassword";

    @Test
    public void Test() {
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

        var sql =
            "INSERT INTO shoppingcart.shop (" +
                    "id" +
                    ",content" +
                    ",timestamp" +
                    ") VALUES (" +
                    "?" +
                    ",?" +
                    ",?" +
                    ")";

        var gson = new Gson();
        try (var connection = connect();
             PreparedStatement preparedStatement = connection.prepareCall(sql)) {
            var date = new java.util.Date();
            var timestampNow = new Timestamp(date.getTime());
            PGobject jsonShop = new PGobject();
            jsonShop.setType("jsonb");
            jsonShop.setValue(gson.toJson(shop));
            preparedStatement.setObject(1, UUID.randomUUID());
            preparedStatement.setObject(2, jsonShop);
            preparedStatement.setObject(3, timestampNow);
            preparedStatement.execute();
        } catch (SQLException e) {
            System.out.println(e.getMessage());
        }
    }

    public Connection connect() {
        Connection conn = null;
        try {
            conn = DriverManager.getConnection(url, user, password);
            System.out.println("Connected to the PostgreSQL server successfully.");
        } catch (SQLException e) {
            System.out.println(e.getMessage());
        }

        return conn;
    }
}
