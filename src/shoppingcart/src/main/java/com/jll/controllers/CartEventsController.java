package com.jll.controllers;

import com.jll.dtos.DomainMessage;
import com.jll.utilities.web.TransportMessageLinkBuilder;
import org.springframework.hateoas.Resource;

import com.jll.dtos.DomainEventToDtoMapper;
import com.jll.dtos.TransportMessage;
import com.jll.repositories.EventRepository;
import com.jll.utilities.LocalConnectionManagerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.sql.SQLException;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Comparator;

@RestController
@RequestMapping("/events/carts")
public class CartEventsController {
    @GetMapping("/{startEventNumber}-{endEventNumber}")
    public ResponseEntity get(@PathVariable long startEventNumber, @PathVariable long endEventNumber) {
        var eventRepository = new EventRepository(LocalConnectionManagerFactory.Get(), "cart");
        try {
            var eventRecords = eventRepository.get(startEventNumber, endEventNumber);
            Collection<DomainMessage> domainMessages = DomainEventToDtoMapper.map(new ArrayList(eventRecords), startEventNumber);

            var latestRetrievedEventNumberInRange =
                    domainMessages.stream()
                        .map(x -> x.eventNumber)
                        .max(Comparator.comparingLong(x -> x))
                        .orElse(startEventNumber);
            var links = TransportMessageLinkBuilder.Build(startEventNumber, endEventNumber, latestRetrievedEventNumberInRange);

            var transportMessage = new TransportMessage();
            transportMessage.messages = domainMessages;

            var resource = new Resource<>(transportMessage, links);

            return ResponseEntity.ok(resource);
        } catch (SQLException e) {
            e.printStackTrace();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body("Could not retrieve Carts");
        }
    }
}