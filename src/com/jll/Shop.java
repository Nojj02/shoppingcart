package com.jll;

import com.google.common.collect.MoreCollectors;

import java.util.Collection;
import java.util.Optional;

public class Shop
{
    private Collection<Item> items;
    private ShippingCostCalculator shippingCostCalculator;

    public Shop(Collection<Item> items) {
        this(items, new ShippingCostCalculator());
    }

    public Shop(Collection<Item> items, ShippingCostCalculator shippingCostCalculator){
        if (items.size() < 10) {
            throw new NotEnoughItemsInShopException();
        }
        this.items = items;
        this.shippingCostCalculator = shippingCostCalculator;
    }

    public Collection<Item> get_ShoppingItems()
    {
        return items;
    }

    public Optional<Item> getItem(String itemCode) {
        return items.stream()
                .filter(item -> item.getItemCode() == itemCode)
                .collect(MoreCollectors.toOptional());
    }

    public void setDiscount(String itemCode, Discount discount) {
        var itemOptional = getItem(itemCode);
        if (itemOptional.isPresent()) {
            itemOptional.get().setDiscount(discount);
        }
    }

    public double compute(Collection<ItemForPurchase> itemsForPurchase) {
        return compute(itemsForPurchase, Optional.empty());
    }

    public double compute(Collection<ItemForPurchase> itemsForPurchase, Coupon coupon) {
        return compute(itemsForPurchase, Optional.of(coupon));
    }

    public double compute(Collection<ItemForPurchase> itemsForPurchase, Optional<Coupon> couponOptional) {
        return itemsForPurchase.stream()
            .mapToDouble(itemForPurchase ->
                getItem(itemForPurchase.getItemCode())
                    .map(item -> {
                        var totalGrossAmount = item.getPrice() * itemForPurchase.getQuantity();
                        var perItemDiscountNetAmount = item.getDiscountedPrice() * itemForPurchase.getQuantity();

                        var couponDiscount =
                            couponOptional
                                .filter(coupon -> coupon.appliesTo(item.getItemType().getItemTypeCode()))
                                .map(coupon -> coupon.getDiscount())
                                .orElse(Discount.None);
                        var couponDiscountNetAmount = couponDiscount.applyPercentageDiscount(totalGrossAmount);

                        return Math.min(perItemDiscountNetAmount, couponDiscountNetAmount);
                    })
                    .orElse(0.0)
                )
            .sum();
    }

    public Cost computeNew(Collection<ItemForPurchase> itemsForPurchase) {
        return computeNew(itemsForPurchase, Optional.empty());
    }

    public Cost computeNew(Collection<ItemForPurchase> itemsForPurchase, Coupon coupon) {
        return computeNew(itemsForPurchase, Optional.of(coupon));
    }

    public Cost computeNew(Collection<ItemForPurchase> itemsForPurchase, Optional<Coupon> couponOptional) {
        var totalCost = itemsForPurchase.stream()
                .map(itemForPurchase ->
                        getItem(itemForPurchase.getItemCode())
                                .map(item -> {
                                    var totalGrossAmount = item.getPrice() * itemForPurchase.getQuantity();
                                    var perItemDiscountNetAmount = item.getDiscountedPrice() * itemForPurchase.getQuantity();

                                    var couponDiscount =
                                            couponOptional
                                                    .filter(coupon -> coupon.appliesTo(item.getItemType().getItemTypeCode()))
                                                    .map(coupon -> coupon.getDiscount())
                                                    .orElse(Discount.None);
                                    var couponDiscountNetAmount = couponDiscogunt.applyPercentageDiscount(totalGrossAmount);

                                    var netItemAmount = Math.min(perItemDiscountNetAmount, couponDiscountNetAmount);

                                    var shippingCost = shippingCostCalculator.compute(item.getWeight()) * itemForPurchase.getQuantity();

                                    return new Cost(netItemAmount, shippingCost);
                                })
                                .orElse(new Cost(0.0, 0.0))
                )
                .reduce(new Cost(0, 0), (a, b) -> a.add(b));

        return totalCost;
    }

    public class Cost {
        public double discountedCost;
        public double shippingCost;

        public Cost(double discountedCost, double shippingCost) {

            this.discountedCost = discountedCost;
            this.shippingCost = shippingCost;
        }

        public Cost add(Cost other) {
            return new Cost(this.discountedCost + other.discountedCost,
                    this.shippingCost + other.shippingCost);
        }

        public double getTotalCost() {
            return discountedCost + shippingCost;
        }
    }

    public class ShopException extends RuntimeException {
    }

    public class NotEnoughItemsInShopException extends ShopException {
    }
}

