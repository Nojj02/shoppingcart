package com.jll.services;

import com.jll.model.Weight;

import java.util.ArrayList;
import java.util.List;

public class ShippingCostCalculator {
    private List<CostRange> costRanges;
    private int costPerExcessKilo = 5;
    private double baseCostForMaxWeight = 50.00;

    public ShippingCostCalculator() {
        costRanges = new ArrayList<>();
        costRanges.add(new CostRange(Weight.grams(75), 3.50));
        costRanges.add(new CostRange(Weight.grams(200), 8.00));
        costRanges.add(new CostRange(Weight.grams(499), 15.00));
        costRanges.add(new CostRange(Weight.grams(999), 22.00));
        costRanges.add(new CostRange(Weight.kilograms(2.999), 35.00));
        costRanges.add(new CostRange(Weight.kilograms(8), 50.00));
    }

    public double compute(Weight weight) {
        for (var costRange : costRanges) {
            if (weight.isLessThanOrEqual(costRange.UpperBound)) {
                return costRange.Amount;
            }
        }

        var excessWeight = Math.ceil(weight.subtract(Weight.kilograms(8)).inKilograms());
        return baseCostForMaxWeight + (excessWeight * costPerExcessKilo);
    }

    private class CostRange {
        public CostRange(Weight upperBound, double amount) {
            UpperBound = upperBound;
            Amount = amount;
        }

        public Weight UpperBound;
        public double Amount;
    }
}
