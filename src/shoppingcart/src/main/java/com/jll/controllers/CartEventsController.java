package com.jll.controllers;

import org.springframework.hateoas.Resource;
import org.springframework.hateoas.mvc.ControllerLinkBuilder;

import com.jll.dtos.DomainEventToDtoMapper;
import com.jll.dtos.TransportMessage;
import com.jll.repositories.EventRepository;
import com.jll.utilities.LocalConnectionManagerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.sql.SQLException;
import java.util.Arrays;

@RestController
@RequestMapping("/events/carts")
public class CartEventsController {
    @GetMapping("/{startEventNumber}-{endEventNumber}")
    public ResponseEntity get(@PathVariable long startEventNumber, @PathVariable long endEventNumber) {
        var eventRepository = new EventRepository(LocalConnectionManagerFactory.Get(), "cart");
        try {
            var eventRecords = eventRepository.get(startEventNumber, endEventNumber);
            var domainMessages = DomainEventToDtoMapper.map(eventRecords);

            var links =
                    Arrays.asList(
                            ControllerLinkBuilder
                                    .linkTo(
                                            ControllerLinkBuilder.methodOn(CartEventsController.class)
                                                    .get(startEventNumber, endEventNumber))
                                    .withSelfRel()
                    );

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