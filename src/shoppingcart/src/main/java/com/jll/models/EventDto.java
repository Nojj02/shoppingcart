package com.jll.models;

import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.jll.repositories.EventRecord;

import java.io.IOException;
import java.sql.Timestamp;
import java.util.UUID;

public class EventDto {
    public UUID id;
    public JsonNode event;
    public String eventType;
    public long version;
    public Timestamp timestamp;


    public EventDto(EventRecord eventRecord) throws IOException {
        var mapper = new ObjectMapper();
        this.event = mapper.readTree(eventRecord.getEvent());
        this.id = eventRecord.getId();
        this.eventType = eventRecord.getEventType();
        this.version = eventRecord.getVersion();
        this.timestamp = eventRecord.getTimestamp();
    }
}
