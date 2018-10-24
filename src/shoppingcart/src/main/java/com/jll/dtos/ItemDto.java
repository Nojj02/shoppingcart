package com.jll.dtos;

import com.jll.model.Item;

import java.util.UUID;

public class ItemDto {
    public UUID Id;
    public String ItemCode;
    public double Price;
    public double WeightInGrams;

    public ItemDto(Item item) {
        Id = item.getId().getValue();
        ItemCode = item.getItemCode();
        Price = item.getPrice();
        WeightInGrams = item.getWeight().inGrams();
    }
}