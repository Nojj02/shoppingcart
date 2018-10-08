package com.jll.repositories;

import com.fasterxml.jackson.annotation.JsonAutoDetect;
import com.fasterxml.jackson.annotation.PropertyAccessor;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.DeserializationFeature;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.jll.models.AggregateRoot;
import com.jll.models.Cart;
import com.jll.models.CartEvent;
import com.jll.utilities.ConnectionManager;
import org.postgresql.util.PGobject;

import javax.xml.stream.util.EventReaderDelegate;
import java.io.IOException;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Timestamp;
import java.util.*;

import static java.util.stream.Collectors.groupingBy;

public abstract class EventingRepository<T extends Cart> {
    private final Class<T> classOfT;
    protected ConnectionManager connectionManager;
    private final String tableName;
    private ObjectMapper objectMapper;

    protected EventingRepository(ConnectionManager connectionManager, Class<T> classOfT, String tableName) {
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
                preparedStatement.setObject(1, entity.getId());
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
/*
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
            preparedStatement.setObject(2, entity.getId());
            preparedStatement.execute();
        } catch (SQLException e) {
            throw e;
        } catch (JsonProcessingException e) {
            throw new RuntimeException("Failed to convert entity to JSON", e);
        }
    }
*/

    // Needs a read model
    public Collection<Cart> get(int count) throws SQLException {
        var sql = "SELECT * FROM shoppingcart." + getTableName();

        try (var connection = this.connectionManager.connect();
             PreparedStatement preparedStatement = connection.prepareCall(sql)) {

            var result = preparedStatement.executeQuery();
            var eventRecords = new ArrayList<EventRecord>();

            while (result.next()) {
                var eventRecord = new EventRecord(result);
                eventRecords.add(eventRecord);
            }

            var entities = new ArrayList<Cart>();
            Map<UUID, List<EventRecord>> entityByIdGroups =
                    eventRecords.stream()
                        .collect(groupingBy(eventRecord -> eventRecord.id));
            for (var entityByIdGroup : entityByIdGroups.entrySet()) {
                var events = new ArrayList<CartEvent>();
                for(var eventRecord : entityByIdGroup.getValue()) {
                    CartEvent anEvent = getEntityFromResultSet(eventRecord);
                    events.add(anEvent);
                }

                var entity = Cart.reconstitute(entityByIdGroup.getKey(), events);
                entities.add(entity);
            }


            return entities;
        } catch (SQLException e) {
            throw e;
        }
    }

    private class EventRecord {
        public UUID id;
        public String event;
        public String eventType;

        public EventRecord(ResultSet result) throws SQLException {
            this.id = result.getObject("id", UUID.class);
            this.event = result.getString("event");
            this.eventType = result.getString("event_type");
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
                var eventRecord = new EventRecord(result);
                CartEvent anEvent = getEntityFromResultSet(eventRecord);

                events.add(anEvent);
            }

            if (events.isEmpty()) {
                return Optional.empty();
            } else {
                return Optional.of(Cart.reconstitute(id, events));
            }

        } catch (SQLException e) {
            throw e;
        }
    }
    /*
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

            var events = new ArrayList<CartEvent>();
            var result = preparedStatement.executeQuery();
            var entities = new ArrayList<T>();
            while (result.next()) {
                CartEvent anEvent = getEntityFromResultSet(result);

                events.add(anEvent);
            }

            if (events.isEmpty()) {
                return Optional.empty();
            } else {
                return Optional.of(Cart.reconstitute(events));
            }

            return entities;

        } catch (SQLException e) {
            throw e;
        }
    }*/

    protected CartEvent getEntityFromResultSet(EventRecord eventRecord) {
        try {
            return (CartEvent)objectMapper.readValue(eventRecord.event, Class.forName(eventRecord.eventType));
        } catch (IOException e) {
            throw new RuntimeException("Failed to convert JSON back to " + eventRecord.eventType, e);
        } catch (ClassNotFoundException e) {
            throw new RuntimeException("Failed to convert JSON back to " + eventRecord.eventType, e);
        }
    }
}
