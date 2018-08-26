package com.jll;

public final class Discount {
    public static final Discount None = new Discount();

    private int fixedAmount;
    private double percentage;

    public static Discount Fixed(int amountDiscounted) {
        var discount = new Discount();
        discount.fixedAmount = amountDiscounted;
        return discount;
    }

    public static Discount percentage(double percentageDiscounted) {
        var discount = new Discount();
        discount.percentage = percentageDiscounted;
        return discount;
    }

    public int getFixedAmount() {
        return fixedAmount;
    }

    public double getPercentage() {
        return percentage;
    }
}
