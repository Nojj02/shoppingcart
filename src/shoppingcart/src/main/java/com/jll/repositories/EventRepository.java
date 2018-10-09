package com.jll.repositories;

import com.fasterxml.jackson.annotation.JsonAutoDetect;
import com.fasterxml.jackson.annotation.PropertyAccessor;
import com.fasterxml.jackson.databind.DeserializationFeature;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.jll.models.Cart;
import com.jll.models.CartEvent;
import com.jll.utilities.ConnectionManager;

import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.util.*;

import static java.util.stream.Collectors.groupingBy;

public class EventRepository {
    protected ConnectionManager connectionManager;
    private final String tableName;
    private ObjectMapper objectMapper;

    public EventRepository(ConnectionManager connectionManager, String tableName) {
        this.connectionManager = connectionManager;
        this.tableName = tableName;
        this.objectMapper = new ObjectMapper();
        objectMapper.setVisibility(PropertyAccessor.FIELD, JsonAutoDetect.Visibility.ANY);
        objectMapper.configure(DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES, false);
    }

    protected String getTableName() {
        return this.tableName;
    }

    // Needs a read model
    public Collection<EventRecord> get(int count) throws SQLException {
        var sql = "SELECT * FROM shoppingcart." + getTableName() + " LIMIT " + count;

        try (var connection = this.connectionManager.connect();
             PreparedStatement preparedStatement = connection.prepareCall(sql)) {

            var result = preparedStatement.executeQuery();
            var eventRecords = new ArrayList<EventRecord>();

            while (result.next()) {
                var eventRecord = EventRecord.fromResultSet(result);
                eventRecords.add(eventRecord);
            }

            return eventRecords;
        } catch (SQLException e) {
            throw e;
        }
    }
}
