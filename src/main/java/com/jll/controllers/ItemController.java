package com.jll.controllers;

import java.sql.SQLException;
import java.util.UUID;

import com.jll.dtos.ItemDto;
import com.jll.dtos.PostItemDto;

import com.jll.models.Item;
import com.jll.models.ItemType;
import com.jll.models.Weight;
import com.jll.repository.ItemRepository;
import com.jll.utilities.ConnectionManager;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/items")
public class ItemController {
    private final String url = "jdbc:postgresql://localhost:5432/postgres";
    private final String user = "postgres";
    private final String password = "thepassword";

    @PostMapping()
    public ResponseEntity save(@RequestBody PostItemDto postItemDto) {
        var item = new Item(
                UUID.randomUUID(),
                postItemDto.ItemCode,
                ItemType.Unknown,
                postItemDto.Price,
                Weight.grams(postItemDto.WeightInGrams)
        );

        var connectionManager = new ConnectionManager(url, user, password);
        var itemRepository = new ItemRepository(connectionManager);
        try {
            itemRepository.save(item);
        } catch (SQLException e) {
            e.printStackTrace();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body("Could not save the Item");
        }

        var itemDto = new ItemDto(item);
        return ResponseEntity.ok(itemDto);
    }
}