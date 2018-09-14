package com.jll.repositories;

import com.google.gson.Gson;
import com.jll.models.AggregateRoot;
import com.jll.utilities.ConnectionManager;
import org.postgresql.util.PGobject;

import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Optional;
import java.util.UUID;

public abstract class Repository<T extends AggregateRoot> {
    private ConnectionManager connectionManager;
    private final Class<T> classOfT;
    private final String tableName;

    public Repository(ConnectionManager connectionManager, Class<T> classOfT, String tableName) {
        this.connectionManager = connectionManager;
        this.classOfT = classOfT;
        this.tableName = tableName;
    }

    public void save(T item)
            throws SQLException {
        var sql =
                "INSERT INTO shoppingcart." + this.tableName + " (" +
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
            jsonShop.setValue(gson.toJson(item));
            preparedStatement.setObject(1, item.getId());
            preparedStatement.setObject(2, jsonShop);
            preparedStatement.setObject(3, timestampNow);
            preparedStatement.execute();
        } catch (SQLException e) {
            throw e;
        }
    }

    public Collection<T> get(int count) throws SQLException {
        var sql = "SELECT * FROM shoppingcart." + this.tableName + " LIMIT " + count;

        var gson = new Gson();
        try (var connection = this.connectionManager.connect();
             PreparedStatement preparedStatement = connection.prepareCall(sql)) {

            var result = preparedStatement.executeQuery();
            var entities = new ArrayList<T>();
            while (result.next()) {
                var content = result.getString("content");

                entities.add(gson.fromJson(content, this.classOfT));
            }

            return entities;
        } catch (SQLException e) {
            throw e;
        }
    }

    public Optional<T> get(UUID id) throws SQLException {
        var sql = "SELECT * FROM shoppingcart." + this.tableName + " WHERE id = ?";

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
                return Optional.of(gson.fromJson(content, this.classOfT));
            } else {
                return Optional.empty();
            }

        } catch (SQLException e) {
            throw e;
        }
    }

    public Collection<T> get(Collection<UUID> ids) throws SQLException {
        if (ids.isEmpty()) return new ArrayList<>();
        var idsList = new ArrayList<>(ids);

        var sql = "SELECT * FROM shoppingcart." + this.tableName + " WHERE id IN (";
        for(var ignored : ids) {
            sql += "?,";
        }
        sql = sql.substring(0, sql.length() - 1);
        sql += ")";

        var gson = new Gson();
        try (var connection = this.connectionManager.connect();
             PreparedStatement preparedStatement = connection.prepareCall(sql)) {
            for (int index = 1; index <= ids.size() ; index++) {
                preparedStatement.setObject(index, idsList.get(index - 1));
            }

            var result = preparedStatement.executeQuery();
            var entities = new ArrayList<T>();
            while (result.next()) {
                var content = result.getString("content");

                entities.add(gson.fromJson(content, this.classOfT));
            }

            return entities;

        } catch (SQLException e) {
            throw e;
        }
    }
}
