package com.jll.controllers;

import com.google.common.collect.MoreCollectors;
import com.jll.dtos.*;
import com.jll.models.cartModel.Cart;
import com.jll.models.cartModel.CartIdentity;
import com.jll.models.cartModel.ItemForPurchase;
import com.jll.repositories.CartRepository;
import com.jll.repositories.CouponRepository;
import com.jll.repositories.ItemRepository;
import com.jll.utilities.LocalConnectionManagerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.sql.SQLException;
import java.util.UUID;
import java.util.stream.Collectors;

@RestController
@RequestMapping("/carts")
public class CartController {

    @GetMapping("/{id}")
    public ResponseEntity get(@PathVariable UUID id) {
        var cartRepository = new CartRepository(LocalConnectionManagerFactory.Get());
        try {
            return cartRepository.get(id)
                    .map(cart -> (ResponseEntity) ResponseEntity.ok(new CartDto(cart)))
                    .orElse(ResponseEntity.status(HttpStatus.NOT_FOUND).body("Cart " + id + " does not exist"));
        } catch (SQLException e) {
            e.printStackTrace();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body("Could not retrieve an Cart");
        }
    }

    @PostMapping()
    public ResponseEntity save(@RequestBody PostCartDto postCartDto) {
        if (postCartDto.CartItems == null || postCartDto.CartItems.isEmpty()) {
            return ResponseEntity.badRequest()
                    .body("There should be at least one ItemIdentity in your cart");
        }

        var itemIds =
                postCartDto.CartItems.stream()
                        .map(cartItemDto -> cartItemDto.itemId)
                        .collect(Collectors.toList());

        try {
            var itemRepository = new ItemRepository(LocalConnectionManagerFactory.Get());
            var queriedItems = itemRepository.get(itemIds);

            var matchingItemsMap =
                    postCartDto.CartItems.stream()
                            .collect(Collectors.toMap(
                                    cartItem -> cartItem,
                                    cartItem -> queriedItems.stream()
                                            .filter(item -> item.getId().matches(cartItem.itemId))
                                            .collect(MoreCollectors.toOptional())));

            var missingCartItems =
                    matchingItemsMap.entrySet().stream()
                            .filter(pair -> !pair.getValue().isPresent())
                            .map(pair -> pair.getKey())
                            .collect(Collectors.toList());

            if (!missingCartItems.isEmpty()) {
                var missingItemIds = missingCartItems.stream()
                        .map(cartItemDto -> cartItemDto.itemId.toString())
                        .collect(Collectors.joining(","));
                return ResponseEntity.badRequest()
                        .body("The following Items could not be found: " + missingItemIds);
            }

            var itemsForPurchase =
                    matchingItemsMap.entrySet().stream()
                            .filter(pair -> pair.getValue().isPresent())
                            .map(pair -> ItemForPurchase.createItemForPurchase(
                                    pair.getValue().get().getId(),
                                    pair.getKey().quantity
                            ))
                            .collect(Collectors.toList());

            var cart = new Cart(
                    new CartIdentity(UUID.randomUUID()),
                    itemsForPurchase
            );

            var cartRepository = new CartRepository(LocalConnectionManagerFactory.Get());
            cartRepository.save(cart);

            var cartDto = new CartDto(cart);
            return ResponseEntity.ok(cartDto);
        } catch (SQLException e) {
            e.printStackTrace();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body("Could not save the Cart");
        }
    }

    @GetMapping("/{id}/addItem")
    public ResponseEntity addItem(@PathVariable UUID id, @RequestBody AddItemDto addItemDto) {
        var cartRepository = new CartRepository(LocalConnectionManagerFactory.Get());
        var itemRepository = new ItemRepository(LocalConnectionManagerFactory.Get());
        try {
            var optionalCart = cartRepository.get(id);
            if(!optionalCart.isPresent()) {
                return ResponseEntity.status(HttpStatus.NOT_FOUND).body("Cart " + id + " does not exist");
            }

            var optionalItem = itemRepository.get(addItemDto.ItemId);

            var itemForPurchase =
                    ItemForPurchase.createItemForPurchase(
                            optionalItem.get().getId(),
                            addItemDto.Quantity
                    );

            var cart = optionalCart.get();
            cart.addItem(itemForPurchase);

            cartRepository.save(cart);
            var cartDto = new CartDto(cart);
            return ResponseEntity.ok(cartDto);
        } catch (SQLException e) {
            e.printStackTrace();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body("Could not retrieve an Cart");
        }
    }

    @PostMapping("/{id}/applyCoupon")
    public ResponseEntity applyCoupon(@PathVariable UUID id, @RequestBody ApplyCouponDto applyCouponDto) {
        var cartRepository = new CartRepository(LocalConnectionManagerFactory.Get());
        var couponRepository = new CouponRepository(LocalConnectionManagerFactory.Get());
        try {
            var optionalCart = cartRepository.get(id);
            if(!optionalCart.isPresent()) {
                return ResponseEntity.status(HttpStatus.NOT_FOUND).body("Cart " + id + " does not exist.");
            }

            var optionalCoupon = couponRepository.getByCouponCode(applyCouponDto.CouponCode);
            if(!optionalCoupon.isPresent()) {
                return ResponseEntity.status(HttpStatus.NOT_FOUND).body("Coupon " + applyCouponDto.CouponCode + " does not exist.");
            }

            var cart = optionalCart.get();
            var coupon = optionalCoupon.get();

            cart.applyCoupon(coupon);

            cartRepository.update(cart);
            var cartDto = new CartDto(cart);
            return ResponseEntity.ok(cartDto);
        } catch (SQLException e) {
            e.printStackTrace();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body("An unknown error occurred. Could not complete request.");
        }
    }
}