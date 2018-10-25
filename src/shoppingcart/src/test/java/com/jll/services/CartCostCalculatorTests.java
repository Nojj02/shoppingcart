package com.jll.services;

import com.jll.model.*;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

public class CartCostCalculatorTests {
    public static class ComputeTests {
        @Test
        void computesTotalCost() {
            var banana = new Item(ItemIdentity.generateNew(), "Banana", 12.00);
            var potato = new Item(ItemIdentity.generateNew(), "Potato", 18.00);

            var cartItems = List.of(
                    new CartCostCalculator.ComputeCostCartItem(banana, 5),
                    new CartCostCalculator.ComputeCostCartItem(potato, 2)
            );

            var calculator = new CartCostCalculator();
            var cartCost = calculator.compute(cartItems);
            
            var cost = cartCost.getCost();
            
            Assertions.assertEquals(0, cost.getDiscountAmount());
            Assertions.assertEquals(96, cost.getGrossAmount());
        }

        @Test
        void itemsAreDiscounted_itemsAreOnSpecialWithPercentage() {
            var banana = new Item(ItemIdentity.generateNew(), "Banana", 12.00);
            var potato = new Item(ItemIdentity.generateNew(), "Potato", 18.00);
            banana.setDiscount(new Discount(50.00, 0));

            var cartItems = List.of(
                    new CartCostCalculator.ComputeCostCartItem(banana, 5),
                    new CartCostCalculator.ComputeCostCartItem(potato, 2)
            );

            var calculator = new CartCostCalculator();
            var cartCost = calculator.compute(cartItems);
            var cost = cartCost.getCost();

            Assertions.assertEquals(30, cost.getDiscountAmount());
            Assertions.assertEquals(96, cost.getGrossAmount());
        }

        @Test
        void itemsAreDiscounted_itemsAreOnSpecialWithFixedRate() {
            var banana = new Item(ItemIdentity.generateNew(), "Banana", 12.00);
            var potato = new Item(ItemIdentity.generateNew(), "Potato", 18.00);
            banana.setDiscount(new Discount(0, 3));

            var cartItems = List.of(
                    new CartCostCalculator.ComputeCostCartItem(banana, 5),
                    new CartCostCalculator.ComputeCostCartItem(potato, 2)
            );

            var calculator = new CartCostCalculator();
            var cartCost = calculator.compute(cartItems);
            var cost = cartCost.getCost();

            Assertions.assertEquals(15, cost.getDiscountAmount());
            Assertions.assertEquals(96, cost.getGrossAmount());
        }

        @Test
        void itemsAreDiscountedWithHighestAmount_itemsAreOnSpecialWithFixedRateAndPercentage() {
            var banana = new Item(ItemIdentity.generateNew(), "Banana", 12.00);
            var potato = new Item(ItemIdentity.generateNew(), "Potato", 18.00);
            banana.setDiscount(new Discount(10.00, 5));

            var cartItems = List.of(
                    new CartCostCalculator.ComputeCostCartItem(banana, 5),
                    new CartCostCalculator.ComputeCostCartItem(potato, 2)
            );

            var calculator = new CartCostCalculator();
            var cartCost = calculator.compute(cartItems);
            var cost = cartCost.getCost();

            Assertions.assertEquals(25, cost.getDiscountAmount());
            Assertions.assertEquals(96, cost.getGrossAmount());
        }

        @Test
        void allItemsAreDiscounted_storeWideCoupon() {
            var banana = new Item(ItemIdentity.generateNew(), "Banana", 12.00);
            var potato = new Item(ItemIdentity.generateNew(), "Potato", 18.00);
            banana.setDiscount(new Discount(10.00, 5));

            var cartItems = List.of(
                    new CartCostCalculator.ComputeCostCartItem(banana, 5),
                    new CartCostCalculator.ComputeCostCartItem(potato, 2)
            );

            var coupon =
                    Coupon.storeWide(
                            CouponIdentity.generateNew(),
                            "ALL50",
                            new Discount(50, 0));

            var calculator = new CartCostCalculator();
            var cartCost = calculator.compute(cartItems, Optional.of(coupon));
            var cost = cartCost.getCost();

            Assertions.assertEquals(48, cost.getDiscountAmount());
            Assertions.assertEquals(96, cost.getGrossAmount());
        }

        @Test
        void typesOfItemsAreDiscounted_typeOfItemCoupon() {
            var vegetableItemType =  new ItemType("Vegetable");
            var fruitItemType =  new ItemType("Fruit");
            var banana = new Item(ItemIdentity.generateNew(),"Banana", fruitItemType, 12.00);
            var potato = new Item(ItemIdentity.generateNew(),"Potato", vegetableItemType, 18.00);
            var lettuce = new Item(ItemIdentity.generateNew(),"Lettuce", vegetableItemType, 20.00);

            var cartItems = List.of(
                    new CartCostCalculator.ComputeCostCartItem(banana, 5),
                    new CartCostCalculator.ComputeCostCartItem(potato, 2),
                    new CartCostCalculator.ComputeCostCartItem(lettuce, 2)
            );

            var coupon = Coupon.forItemType(
                    CouponIdentity.generateNew(),
                    "VEG25",
                    new Discount(25, 0),
                    "Vegetable");

            var calculator = new CartCostCalculator();
            var cartCost = calculator.compute(cartItems, Optional.of(coupon));

            var cost = cartCost.getCost();

            Assertions.assertEquals(19, cost.getDiscountAmount());
            Assertions.assertEquals(136, cost.getGrossAmount());
        }

        @Test
        void shippingCost_defaultCost_itemsHaveDefaultWeight() {
            var vegetableItemType =  new ItemType("Vegetable");
            var fruitItemType =  new ItemType("Fruit");
            var banana = new Item(ItemIdentity.generateNew(),"Banana", fruitItemType, 12.00);
            var potato = new Item(ItemIdentity.generateNew(),"Potato", vegetableItemType, 18.00);

            var cartItems = List.of(
                    new CartCostCalculator.ComputeCostCartItem(banana, 5),
                    new CartCostCalculator.ComputeCostCartItem(potato, 2)
            );

            var calculator = new CartCostCalculator();
            var cartCost = calculator.compute(cartItems);

            var cost = cartCost.getCost();

            Assertions.assertEquals(24.50, cost.getShippingCost());
            Assertions.assertEquals(120.50, cost.getTotalCost());
        }

        @Test
        void shippingCost_itemsHaveVariableWeight() {
            var vegetableItemType =  new ItemType("Vegetable");
            var fruitItemType =  new ItemType("Fruit");
            var banana = new Item(ItemIdentity.generateNew(),"Banana", fruitItemType, 12.00, Weight.grams(100));
            var potato = new Item(ItemIdentity.generateNew(),"Potato", vegetableItemType, 18.00);

            var cartItems = List.of(
                    new CartCostCalculator.ComputeCostCartItem(banana, 5),
                    new CartCostCalculator.ComputeCostCartItem(potato, 2)
            );

            var calculator = new CartCostCalculator();
            var cartCost = calculator.compute(cartItems);

            var cost = cartCost.getCost();

            Assertions.assertEquals(96.00, cost.getGrossAmount());
            Assertions.assertEquals(47.00, cost.getShippingCost());
            Assertions.assertEquals(143.00, cost.getTotalCost());
        }
    }
}
