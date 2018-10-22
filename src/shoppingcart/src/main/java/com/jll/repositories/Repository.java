package com.jll.repositories;

import com.fasterxml.jackson.annotation.JsonAutoDetect;
import com.fasterxml.jackson.annotation.PropertyAccessor;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.DeserializationFeature;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.jll.models.AggregateRoot;
import com.jll.models.Identity;
import com.jll.utilities.ConnectionManager;
import org.postgresql.util.PGobject;

import java.io.IOException;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Optional;
import java.util.UUID;

public abstract class Repository<T extends AggregateRoot<TId>, TId extends Identity> {
    private final Class<T> classOfT;
    protected ConnectionManager connectionManager;
    private final String tableName;
    private ObjectMapper objectMapper;

    protected Repository(ConnectionManager connectionManager, Class<T> classOfT, String tableName) {
        this.connectionManager = connectionManager;
        this.classOfT = classOfT;
        this.tableName = tableName;
        this.objectMapper = new ObjectMapper();
        objectMapper.setVisibility(PropertyAccessor.FIELD, JsonAutoDetect.Visibility.ANY);
        objectMapper.configure(DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES, false);
    }

    protected Class<T> getClassOfT() {
        return classOfT;
    }

    protected String getTableName() {
        return this.tableName;
    }

    public void save(T entity)
            throws SQLException {
        var sql =
                "INSERT INTO shoppingcart." + getTableName() + " (" +
                        "id" +
                        ",content" +
                        ",type" +
                        ",timestamp" +
                        ") VALUES (" +
                        "?" +
                        ",?" +
                        ",?" +
                        ",?" +
                        ")";

        try (var connection = this.connectionManager.connect();
             PreparedStatement preparedStatement = connection.prepareCall(sql)) {
            var date = new java.util.Date();
            var timestampNow = new Timestamp(date.getTime());
            PGobject jsonObject = new PGobject();
            jsonObject.setType("jsonb");
            jsonObject.setValue(objectMapper.writeValueAsString(entity));
            preparedStatement.setObject(1, entity.getId2());
            preparedStatement.setObject(2, jsonObject);
            preparedStatement.setObject(3, entity.getClass().getName());
            preparedStatement.setObject(4, timestampNow);
            preparedStatement.execute();
        } catch (SQLException e) {
            throw e;
        } catch (JsonProcessingException e) {
            throw new RuntimeException("Failed to convert entity to JSON", e);
        }
    }

    public void update(T entity)
            throws SQLException {
        var sql =
                "UPDATE shoppingcart." + getTableName() + " SET content = ? WHERE id = ?";

        try (var connection = this.connectionManager.connect();
             PreparedStatement preparedStatement = connection.prepareCall(sql)) {
            PGobject jsonObject = new PGobject();
            jsonObject.setType("jsonb");
            jsonObject.setValue(objectMapper.writeValueAsString(entity));
            preparedStatement.setObject(1, jsonObject);
            preparedStatement.setObject(2, entity.getId2());
            preparedStatement.execute();
        } catch (SQLException e) {
            throw e;
        } catch (JsonProcessingException e) {
            throw new RuntimeException("Failed to convert entity to JSON", e);
        }
    }

    public Collection<T> get(int count) throws SQLException {
        var sql = "SELECT * FROM shoppingcart." + getTableName() + " LIMIT " + count;

        try (var connection = this.connectionManager.connect();
             PreparedStatement preparedStatement = connection.prepareCall(sql)) {

            var result = preparedStatement.executeQuery();
            var entities = new ArrayList<T>();
            while (result.next()) {
                T entity = getEntityFromResultSet(result);

                entities.add(entity);
            }

            return entities;
        } catch (SQLException e) {
            throw e;
        }
    }

    public Optional<T> get(UUID id) throws SQLException {
        var sql = "SELECT * FROM shoppingcart." + getTableName() + " WHERE id = ?";

        try (var connection = this.connectionManager.connect();
             PreparedStatement preparedStatement = connection.prepareCall(sql)) {
            preparedStatement.setObject(1, id);

            var result = preparedStatement.executeQuery();
            if (result.next()) {
                T entity = getEntityFromResultSet(result);

                if (result.next()) {
                    throw new SQLException("Expected only 1 record. Returned more than 1.");
                }
                return Optional.of(entity);
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

        var sql = "SELECT * FROM shoppingcart." + getTableName() + " WHERE id IN (";
        for (var ignored : ids) {
            sql += "?,";
        }
        sql = sql.substring(0, sql.length() - 1);
        sql += ")";

        try (var connection = this.connectionManager.connect();
             PreparedStatement preparedStatement = connection.prepareCall(sql)) {
            for (int index = 1; index <= ids.size(); index++) {
                preparedStatement.setObject(index, idsList.get(index - 1));
            }

            var result = preparedStatement.executeQuery();
            var entities = new ArrayList<T>();
            while (result.next()) {
                T entity = getEntityFromResultSet(result);

                entities.add(entity);
            }

            return entities;

        } catch (SQLException e) {
            throw e;
        }
    }

    protected T getEntityFromResultSet(ResultSet result) throws SQLException {
        var content = result.getString("content");
        try {
            return objectMapper.readValue(content, getClassOfT());
        } catch (IOException e) {
            throw new RuntimeException("Failed to convert JSON back to " + getClassOfT().getName(), e);
        }
    }
}
