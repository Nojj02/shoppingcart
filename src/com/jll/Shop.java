package com.jll;

import com.google.common.collect.MoreCollectors;

import java.util.Collection;
import java.util.Optional;

public class Shop
{
    private Collection<ShoppingItem> _shoppingItems;

    public Shop(Collection<ShoppingItem> items)
        throws NotEnoughItemsInShopException {
        if (items.size() < 10) {
            throw new NotEnoughItemsInShopException();
        }
        _shoppingItems = items;
    }

    public Collection<ShoppingItem> get_ShoppingItems()
    {
        return _shoppingItems;
    }

    public Optional<ShoppingItem> getShoppingItem(String itemCode) {
        return _shoppingItems.stream()
                .filter(shoppingItem -> shoppingItem.getItemCode() == itemCode)
                .collect(MoreCollectors.toOptional());
    }

    public void setDiscount(String itemCode, Discount discount) {
        var itemOptional = getShoppingItem(itemCode);
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
                getShoppingItem(itemForPurchase.getItemCode())
                    .map(item -> {
                        var totalGrossAmount = item.getPrice() * itemForPurchase.getQuantity();
                        var perItemDiscountNetAmount = item.getDiscountedPrice() * itemForPurchase.getQuantity();

                        var couponDiscount =
                            couponOptional
                                .filter(coupon -> coupon.getItemCode() == item.getItemCode())
                                .map(coupon -> coupon.getDiscount())
                                .orElse(Discount.None);
                        var couponDiscountNetAmount = couponDiscount.applyPercentageDiscount(totalGrossAmount);

                        return Math.min(perItemDiscountNetAmount, couponDiscountNetAmount);
                    })
                    .orElse(0.0)
                )
            .sum();
    }

    public class ShopException extends Exception {
    }

    public class NotEnoughItemsInShopException extends ShopException {
    }
}
