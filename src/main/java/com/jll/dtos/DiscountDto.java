package com.jll.dtos;

import com.jll.models.Discount;

public class DiscountDto {
    public double FixedAmount;
    public double Percentage;

    public DiscountDto() {
    }

    public DiscountDto(Discount discount) {
        FixedAmount = discount.getFixedAmount();
        Percentage = discount.getPercentage();
    }
}
