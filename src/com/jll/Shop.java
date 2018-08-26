package com.jll;

import com.google.common.collect.MoreCollectors;

import java.util.Collection;
import java.util.List;
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

    public double Compute(Collection<ItemForPurchase> itemsForPurchase) {
        return Compute(itemsForPurchase, null);
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

    public double Compute(Collection<ItemForPurchase> itemsForPurchase, Coupon coupon) {
        return itemsForPurchase.stream()
                .mapToDouble(itemForPurchase -> {
                    var shoppingItemOptional = getShoppingItem(itemForPurchase.getItemCode());
                    if (!shoppingItemOptional.isPresent()) {
                        return 0;
                    } else {
                        var item = shoppingItemOptional.get();
                        var totalGrossAmount = item.getPrice() * itemForPurchase.getQuantity();
                        var couponDiscount = coupon != null && coupon.getItemCode() == item.getItemCode() ? coupon.getDiscount() : Discount.None;
                        var netAmount = Math.min(
                                item.getDiscountedPrice() * itemForPurchase.getQuantity(),
                                totalGrossAmount - (totalGrossAmount * (couponDiscount.getPercentage() / 100))
                        );
                        return netAmount;
                    }
                })
                .sum();
    }

    public class ShopException extends Exception {
    }

    public class NotEnoughItemsInShopException extends ShopException {
    }
}
