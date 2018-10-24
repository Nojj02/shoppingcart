package com.jll.model;

public class Weight {
    public static final Weight ZERO = Weight.grams(0);
    private double weightInGrams;

    private Weight() {
    }

    public static Weight grams(double weightInGrams) {
        var weight = new Weight();
        weight.weightInGrams = weightInGrams;
        return weight;
    }

    public static Weight kilograms(double weightInKilograms) {
        var weight = new Weight();
        weight.weightInGrams = weightInKilograms * 1000;
        return weight;
    }

    public double inGrams() {
        return weightInGrams;
    }

    public double inKilograms() {
        return this.weightInGrams / 1000;
    }

    public boolean isLessThanOrEqual(Weight weight) {
        return this.weightInGrams <= weight.weightInGrams;
    }

    public Weight subtract(Weight other) {
        return Weight.grams(this.weightInGrams - other.weightInGrams);
    }
}
