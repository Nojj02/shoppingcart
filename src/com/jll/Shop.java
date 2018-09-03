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

    public Cost compute(Collection<ItemForPurchase> itemsForPurchase) {
        return compute(itemsForPurchase, Optional.empty());
    }

    public Cost compute(Collection<ItemForPurchase> itemsForPurchase, Coupon coupon) {
        return compute(itemsForPurchase, Optional.of(coupon));
    }

    public Cost compute(Collection<ItemForPurchase> itemsForPurchase, Optional<Coupon> couponOptional) {
        var totalCost = itemsForPurchase.stream()
                .map(itemForPurchase ->
                        getItem(itemForPurchase.getItemCode())
                                .map(item -> {
                                    var totalGrossAmount = item.getPrice() * itemForPurchase.getQuantity();
                                    var perItemDiscountAmount = item.getDiscountAmount() * itemForPurchase.getQuantity();

                                    var couponDiscount =
                                            couponOptional
                                                    .filter(coupon -> coupon.appliesTo(item.getItemType().getItemTypeCode()))
                                                    .map(coupon -> coupon.getDiscount())
                                                    .orElse(Discount.None);
                                    var couponDiscountAmount = couponDiscount.computeDiscount(totalGrossAmount);

                                    var discountAmount = Math.max(perItemDiscountAmount, couponDiscountAmount);

                                    var shippingCost = shippingCostCalculator.compute(item.getWeight()) * itemForPurchase.getQuantity();

                                    return new Cost(totalGrossAmount, discountAmount, shippingCost);
                                })
                                .orElse(new Cost(0.0, 0.0, 0.0))
                )
                .reduce(new Cost(0, 0,0), (a, b) -> a.add(b));

        return totalCost;
    }

    public class Cost {
        public double grossAmount;
        public double discountAmount;
        public double shippingCost;

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
    }

    public class ShopException extends RuntimeException {
    }

    public class NotEnoughItemsInShopException extends ShopException {
    }
}