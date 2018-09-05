package com.jll;

public class Weight {
    public static final Weight ZERO = Weight.grams(0);
    private double weightInGrams;

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

    public double getWeightInGrams() {
        return weightInGrams;
    }

    public boolean isLessThanOrEqual(Weight weight) {
        return this.weightInGrams <= weight.weightInGrams;
    }

    public Weight subtract(Weight other) {
        return Weight.grams(this.weightInGrams - other.weightInGrams);
    }

    public double getWeightInKilograms() {
        return this.weightInGrams / 1000;
    }
}
