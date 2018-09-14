package com.jll.models;

public class Cost {
    public double grossAmount;
    public double discountAmount;
    public double shippingCost;

    public static Cost Zero = new Cost(0, 0, 0);

    public Cost(double grossAmount, double discountAmount, double shippingCost) {

        this.grossAmount = grossAmount;
        this.discountAmount = discountAmount;
        this.shippingCost = shippingCost;
    }

    public Cost add(Cost other) {
        return new Cost(this.grossAmount + other.grossAmount,
                this.discountAmount + other.discountAmount,
                this.shippingCost + other.shippingCost);
    }

    public double getTotalCost() {
        return grossAmount + shippingCost - discountAmount;
    }

    public double getGrossAmount() {
        return this.grossAmount;
    }

    public double getDiscountAmount() {
        return this.discountAmount;
    }

    public double getShippingCost() {
        return this.shippingCost;
    }
}
