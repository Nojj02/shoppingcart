package com.jll;

public class ShippingCostCalculator {
    public double compute(Weight weight) {
        if (weight.isLessThanOrEqual(Weight.grams(75))) {
            return 3.50;
        }
        return 8.00;
    }
}
