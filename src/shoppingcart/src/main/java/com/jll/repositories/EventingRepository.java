package com.jll.repositories;

import com.fasterxml.jackson.annotation.JsonAutoDetect;
import com.fasterxml.jackson.annotation.PropertyAccessor;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.DeserializationFeature;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.jll.models.cartModel.Cart;
import com.jll.models.cartModel.CartIdentity;
import com.jll.models.cartModel.events.CartEvent;
import com.jll.utilities.ConnectionManager;
import org.postgresql.util.PGobject;

import java.io.IOException;
import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.sql.Timestamp;
import java.util.*;

public abstract class EventingRepository<T extends Cart> {
    protected ConnectionManager connectionManager;
    private final String tableName;
    private ObjectMapper objectMapper;

    protected EventingRepository(ConnectionManager connectionManager, String tableName) {
        this.connectionManager = connectionManager;
        this.tableName = tableName;
        this.objectMapper = new ObjectMapper();
        objectMapper.setVisibility(PropertyAccessor.FIELD, JsonAutoDetect.Visibility.ANY);
        objectMapper.configure(DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES, false);
    }

    protected String getTableName() {
        return this.tableName;
    }

    public void save(T entity)
            throws SQLException {
        var sql = "INSERT INTO shoppingcart." + getTableName() + " (" +
                        "id" +
                        ",version" +
                        ",event_type" +
                        ",event" +
                        ",timestamp" +
                        ") VALUES (" +
                        "?" +
                        ",?" +
                        ",?" +
                        ",?" +
                        ",?" +
                        "); ";
        try (var connection = this.connectionManager.connect();
             PreparedStatement preparedStatement = connection.prepareCall(sql)) {
            connection.setAutoCommit(false);

            for (var anEvent : entity.getEvents()) {
                var date = new java.util.Date();
                var timestampNow = new Timestamp(date.getTime());
                PGobject jsonObject = new PGobject();
                jsonObject.setType("jsonb");
                jsonObject.setValue(objectMapper.writeValueAsString(anEvent));
                preparedStatement.setObject(1, entity.getId2());
                preparedStatement.setObject(2, anEvent.version);
                preparedStatement.setObject(3, anEvent.getClass().getName());
                preparedStatement.setObject(4, jsonObject);
                preparedStatement.setObject(5, timestampNow);
                preparedStatement.execute();
            }

            connection.commit();
        } catch (SQLException e) {
            throw e;
        } catch (JsonProcessingException e) {
            throw new RuntimeException("Failed to convert entity to JSON", e);
        }
    }

    public void update(T entity)
            throws SQLException {
        var sql = "INSERT INTO shoppingcart." + getTableName() + " (" +
                "id" +
                ",version" +
                ",event_type" +
                ",event" +
                ",timestamp" +
                ") VALUES (" +
                "?" +
                ",?" +
                ",?" +
                ",?" +
                ",?" +
                "); ";
        try (var connection = this.connectionManager.connect();
             PreparedStatement preparedStatement = connection.prepareCall(sql)) {
            connection.setAutoCommit(false);

            for (var anEvent : entity.getNewCartEvents()) {
                var date = new java.util.Date();
                var timestampNow = new Timestamp(date.getTime());
                PGobject jsonObject = new PGobject();
                jsonObject.setType("jsonb");
                jsonObject.setValue(objectMapper.writeValueAsString(anEvent));
                preparedStatement.setObject(1, entity.getId2());
                preparedStatement.setObject(2, anEvent.version);
                preparedStatement.setObject(3, anEvent.getClass().getName());
                preparedStatement.setObject(4, jsonObject);
                preparedStatement.setObject(5, timestampNow);
                preparedStatement.execute();
            }

            connection.commit();
        } catch (SQLException e) {
            throw e;
        } catch (JsonProcessingException e) {
            throw new RuntimeException("Failed to convert entity to JSON", e);
        }
    }

    public Optional<Cart> get(UUID id) throws SQLException {
        var sql = "SELECT * FROM shoppingcart." + getTableName() + " WHERE id = ?";

        try (var connection = this.connectionManager.connect();
             PreparedStatement preparedStatement = connection.prepareCall(sql)) {
            preparedStatement.setObject(1, id);

            var events = new ArrayList<CartEvent>();
            var result = preparedStatement.executeQuery();
            while (result.next()) {
                var eventRecord = EventRecord.fromResultSet(result);
                CartEvent anEvent = getEntityFromResultSet(eventRecord);

                events.add(anEvent);
            }

            if (events.isEmpty()) {
                return Optional.empty();
            } else {
                return Optional.of(Cart.reconstitute(new CartIdentity(id), events));
            }

        } catch (SQLException e) {
            throw e;
        }
    }

    protected CartEvent getEntityFromResultSet(EventRecord eventRecord) {
        try {
            return (CartEvent)objectMapper.readValue(eventRecord.getEvent(), Class.forName(eventRecord.getEventType()));
        } catch (IOException e) {
            throw new RuntimeException("Failed to convert JSON back to " + eventRecord.getEventType(), e);
        } catch (ClassNotFoundException e) {
            throw new RuntimeException("Failed to convert JSON back to " + eventRecord.getEventType(), e);
        }
    }
}

