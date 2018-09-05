package com.jll;

import com.google.gson.Gson;
import org.postgresql.util.PGobject;

import java.sql.CallableStatement;
import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.sql.Timestamp;
import java.util.UUID;

public class ShopRepository {
    private ConnectionManager connectionManager;

    public ShopRepository(ConnectionManager connectionManager) {

        this.connectionManager = connectionManager;
    }

    public void save(Shop shop)
        throws SQLException {
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
        try (var connection = this.connectionManager.connect();
             PreparedStatement preparedStatement = connection.prepareCall(sql)) {
            var date = new java.util.Date();
            var timestampNow = new Timestamp(date.getTime());
            PGobject jsonShop = new PGobject();
            jsonShop.setType("jsonb");
            jsonShop.setValue(gson.toJson(shop));
            preparedStatement.setObject(1, shop.getId());
            preparedStatement.setObject(2, jsonShop);
            preparedStatement.setObject(3, timestampNow);
            preparedStatement.execute();
        } catch (SQLException e) {
            throw e;
        }
    }

    public Shop get(UUID id) throws SQLException {
        var sql = "SELECT * FROM shoppingcart.shop WHERE id = ?";

        var gson = new Gson();
        try (var connection = this.connectionManager.connect();
             PreparedStatement preparedStatement = connection.prepareCall(sql)) {
            preparedStatement.setObject(1, id);

            var result = preparedStatement.executeQuery();
            if (result.next()) {
                var content = result.getString("content");

                if (result.next()) {
                    throw new SQLException("Expected only 1 record. Returned more than 1.");
                }
                return gson.fromJson(content, Shop.class);
            } else {
                return null;
            }

        } catch (SQLException e) {
            throw e;
        }
    }
}
