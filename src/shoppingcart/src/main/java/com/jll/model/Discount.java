package com.jll.model;

public final class Discount {
    public static final Discount None = new Discount(0, 0);

    private double fixedAmount;
    private double percentage;

    private Discount() {
    }

    public Discount(double percentage, double fixedAmount) {
        this.percentage = percentage;
        this.fixedAmount = fixedAmount;
    }

    public double getFixedAmount() {
        return fixedAmount;
    }

    public double getPercentage() {
        return percentage;
    }

    public double computeDiscount(double value) {
        return Math.max(value * (percentage / 100), fixedAmount);
    }

    public double applyPercentageDiscount(double value) {
        return value * (1 - (percentage / 100));
    }
}
