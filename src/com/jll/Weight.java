package com.jll;

public class Weight {
    public static final Weight ZERO = Weight.grams(0);
    private double weightInGrams;

    public static Weight grams(double weightInGrams) {
        var weight = new Weight();
        weight.weightInGrams = weightInGrams;
        return weight;
    }

    public double getWeightInGrams() {
        return weightInGrams;
    }

    public boolean isLessThanOrEqual(Weight weight) {
        return this.weightInGrams <= weight.weightInGrams;
    }
}
