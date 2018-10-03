package com.jll.controllers;

import java.sql.SQLException;
import java.util.UUID;

import com.jll.dtos.ItemDto;
import com.jll.dtos.PostItemDto;

import com.jll.models.Item;
import com.jll.models.ItemType;
import com.jll.models.Weight;
import com.jll.repositories.ItemRepository;
import com.jll.utilities.LocalConnectionManagerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/items")
public class ItemController {

    @GetMapping()
    public ResponseEntity get() {
        var itemRepository = new ItemRepository(LocalConnectionManagerFactory.Get());
        try {
            var itemDtos = itemRepository.get(20).stream()
                    .map(item -> new ItemDto(item));
            return ResponseEntity.ok(itemDtos);
        } catch (SQLException e) {
            e.printStackTrace();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body("Could not retrieve Items");
        }
    }

    @GetMapping("/{id}")
    public ResponseEntity get(@PathVariable UUID id) {
        var itemRepository = new ItemRepository(LocalConnectionManagerFactory.Get());
        try {
            return itemRepository.get(id)
                    .map(item -> (ResponseEntity)ResponseEntity.ok(new ItemDto(item)))
                    .orElse(ResponseEntity.status(HttpStatus.NOT_FOUND).body("Item " + id + " does not exist"));
        } catch (SQLException e) {
            e.printStackTrace();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body("Could not retrieve an Item");
        }
    }

    @PostMapping()
    public ResponseEntity save(@RequestBody PostItemDto postItemDto) {
        var item = new Item(
                UUID.randomUUID(),
                postItemDto.ItemCode,
                ItemType.Unknown,
                postItemDto.Price,
                Weight.grams(postItemDto.WeightInGrams)
        );

        var itemRepository = new ItemRepository(LocalConnectionManagerFactory.Get());
        try {
            itemRepository.save(item);

            var itemDto = new ItemDto(item);
            return ResponseEntity.ok(itemDto);
        } catch (SQLException e) {
            e.printStackTrace();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body("Could not save the Item");
        }
    }
}