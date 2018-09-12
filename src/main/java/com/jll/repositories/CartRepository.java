package com.jll.repositories;

import com.google.gson.Gson;
import com.jll.models.Cart;
import com.jll.models.Item;
import com.jll.utilities.ConnectionManager;
import org.postgresql.util.PGobject;

import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.sql.Timestamp;
import java.util.*;

public class CartRepository {
    private ConnectionManager connectionManager;

    public CartRepository(ConnectionManager connectionManager) {

        this.connectionManager = connectionManager;
    }

    public void save(Cart cart)
        throws SQLException {
        var sql =
                "INSERT INTO shoppingcart.cart (" +
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
            var date = new Date();
            var timestampNow = new Timestamp(date.getTime());
            PGobject jsonShop = new PGobject();
            jsonShop.setType("jsonb");
            jsonShop.setValue(gson.toJson(cart));
            preparedStatement.setObject(1, cart.getId());
            preparedStatement.setObject(2, jsonShop);
            preparedStatement.setObject(3, timestampNow);
            preparedStatement.execute();
        } catch (SQLException e) {
            throw e;
        }
    }

    public Collection<Cart> get(int count) throws SQLException {
        var sql = "SELECT * FROM shoppingcart.cart LIMIT " + count;

        var gson = new Gson();
        try (var connection = this.connectionManager.connect();
             PreparedStatement preparedStatement = connection.prepareCall(sql)) {

            var result = preparedStatement.executeQuery();
            var carts = new ArrayList<Cart>();
            while (result.next()) {
                var content = result.getString("content");

                carts.add(gson.fromJson(content, Cart.class));
            }

            return carts;
        } catch (SQLException e) {
            throw e;
        }
    }

    public Optional<Cart> get(UUID id) throws SQLException {
        var sql = "SELECT * FROM shoppingcart.cart WHERE id = ?";

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
                return Optional.of(gson.fromJson(content, Cart.class));
            } else {
                return Optional.empty();
            }

        } catch (SQLException e) {
            throw e;
        }
    }
}
