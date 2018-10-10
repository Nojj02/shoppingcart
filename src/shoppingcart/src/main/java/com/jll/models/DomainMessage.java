package com.jll.models;

import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.jll.repositories.EventRecord;
import jdk.jfr.Timespan;

import java.io.IOException;
import java.sql.Timestamp;
import java.util.Collection;
import java.util.UUID;

public class DomainMessage {
    public UUID id;
    public JsonNode event;
    public String eventType;
    public long version;
    public Timestamp timestamp;
}

