package com.jll.repositories;

import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Timestamp;
import java.util.UUID;

public class EventRecord {
    private UUID id;
    private String event;
    private String eventType;
    private long version;
    private Timestamp timestamp;

    public static EventRecord fromResultSet(ResultSet result) throws SQLException {
        var eventRecord = new EventRecord();
        eventRecord.id = result.getObject("id", UUID.class);
        eventRecord.version = result.getLong("version");
        eventRecord.event = result.getString("event");
        eventRecord.eventType = result.getString("event_type");
        eventRecord.timestamp = result.getTimestamp("timestamp");
        return eventRecord;
    }

    public UUID getId() {
        return id;
    }

    public String getEvent() {
        return event;
    }

    public String getEventType() {
        return eventType;
    }

    public long getVersion() {
        return version;
    }

    public Timestamp getTimestamp() {
        return timestamp;
    }
}