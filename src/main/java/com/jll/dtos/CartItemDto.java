package com.jll.dtos;

import com.jll.models.CartItem;
import com.jll.models.Cost;
import com.jll.models.ItemForPurchase;

import java.util.UUID;

public class CartItemDto {
    public UUID ItemId;
    public int Quantity;
    public String ItemCode;
    public double Price;
    public double WeightInGrams;
    public double GrossAmount;
    public double DiscountAmount;
    public double ShippingCost;
    public double TotalCost;

    public CartItemDto(CartItem cartItem) {
        ItemForPurchase itemForPurchase = cartItem.getItemForPurchase();
        ItemId = itemForPurchase.getItemId();
        Quantity = itemForPurchase.getQuantity();
        ItemCode = itemForPurchase.getItemCode();
        Price = itemForPurchase.getPrice();
        WeightInGrams = itemForPurchase.getWeight().inGrams();
        Cost cost = cartItem.getCost();
        GrossAmount = cost.getGrossAmount();
        DiscountAmount = cost.getDiscountAmount();
        ShippingCost = cost.getShippingCost();
        TotalCost = cost.getTotalCost();
    }
}
