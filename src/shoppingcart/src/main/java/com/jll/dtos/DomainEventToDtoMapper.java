package com.jll.dtos;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.jll.repositories.EventRecord;

import java.io.IOException;
import java.util.Collection;
import java.util.List;
import java.util.stream.Collectors;
import java.util.stream.IntStream;

public class DomainEventToDtoMapper {
    public static DomainMessage map(EventRecord eventRecord, long eventNumber) throws IOException {
        var eventDto = new DomainMessage();
        var mapper = new ObjectMapper();
        eventDto.event = mapper.readTree(eventRecord.getEvent());
        eventDto.id = eventRecord.getId();
        eventDto.eventType = eventRecord.getEventType();
        eventDto.version = eventRecord.getVersion();
        eventDto.timestamp = eventRecord.getTimestamp();
        eventDto.eventNumber = eventNumber;
        return eventDto;
    }

    public static Collection<DomainMessage> map(List<EventRecord> eventRecords, long startEventNumber) {
        return IntStream.range(0, eventRecords.size())
                .boxed()
                .map(index -> {
                    var eventRecord = eventRecords.get(index);
                    try {
                        return DomainEventToDtoMapper.map(eventRecord, startEventNumber + index);
                    } catch (IOException e) {
                        e.printStackTrace();
                        return null;
                    }
                }).filter(dto -> dto != null)
                .collect(Collectors.toList());
    }
}
