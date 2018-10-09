package com.jll.controllers;

import com.google.common.collect.MoreCollectors;
import com.jll.dtos.AddItemDto;
import com.jll.dtos.ApplyCouponDto;
import com.jll.dtos.CartDto;
import com.jll.dtos.PostCartDto;
import com.jll.models.Cart;
import com.jll.models.Discount;
import com.jll.models.EventDto;
import com.jll.models.ItemForPurchase;
import com.jll.repositories.CartRepository;
import com.jll.repositories.CouponRepository;
import com.jll.repositories.EventRepository;
import com.jll.repositories.ItemRepository;
import com.jll.utilities.LocalConnectionManagerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.io.IOException;
import java.sql.SQLException;
import java.util.UUID;
import java.util.stream.Collectors;

@RestController
@RequestMapping("/events/carts")
public class CartEventsController {

    @GetMapping()
    public ResponseEntity get() {
        var eventRepository = new EventRepository(LocalConnectionManagerFactory.Get(), "cart");
        try {
            var eventDtos = eventRepository.get(20).stream()
                    .map(eventRecord -> {
                        try {
                            return new EventDto(eventRecord);
                        } catch (IOException e) {
                            e.printStackTrace();
                            return null;
                        }
                    }).filter(dto -> dto != null)
                    .collect(Collectors.toList());
            return ResponseEntity.ok(eventDtos);
        } catch (SQLException e) {
            e.printStackTrace();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body("Could not retrieve Carts");
        }
    }
}