package com.jll;

import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.*;

public class ShippingCostCalculatorTest {

    public static class ComputeTest extends ShippingCostCalculatorTest {
        @Test
        void defaultPrice_zero() {
            var shippingCostCalculator = new ShippingCostCalculator();

            var cost = shippingCostCalculator.compute(Weight.ZERO);

            Assertions.assertEquals(3.50, cost);
        }

        @Test
        void greaterThan75() {
            var shippingCostCalculator = new ShippingCostCalculator();

            var cost = shippingCostCalculator.compute(Weight.grams(76));

            Assertions.assertEquals(8, cost);
        }

        @Test
        void greaterThan200() {
            var shippingCostCalculator = new ShippingCostCalculator();

            var cost = shippingCostCalculator.compute(Weight.grams(201));

            Assertions.assertEquals(15, cost);
        }

        @Test
        void greaterThan499() {
            var shippingCostCalculator = new ShippingCostCalculator();

            var cost = shippingCostCalculator.compute(Weight.grams(500));

            Assertions.assertEquals(22, cost);
        }

        @Test
        void greaterThan2_999kilograms() {
            var shippingCostCalculator = new ShippingCostCalculator();

            var cost = shippingCostCalculator.compute(Weight.kilograms(3));

            Assertions.assertEquals(50, cost);
        }

        @Test
        void additional5forEveryAdditionalKilo_greaterThan8kilograms() {
            var shippingCostCalculator = new ShippingCostCalculator();

            var cost = shippingCostCalculator.compute(Weight.kilograms(10));

            Assertions.assertEquals(60, cost);
        }
    }
}