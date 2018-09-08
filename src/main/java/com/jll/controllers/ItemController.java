package com.jll.controllers;

import java.util.UUID;

import com.jll.dtos.ItemDto;
import com.jll.dtos.PostItemDto;

import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/items")
public class ItemController {
    @PostMapping()
    public ItemDto save(@RequestBody PostItemDto postItemDto) {
        var itemDto = new ItemDto();
        itemDto.Id = UUID.randomUUID();
        return itemDto;
    }
}