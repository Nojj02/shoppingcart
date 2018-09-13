package com.jll.controllers;

import com.google.common.collect.MoreCollectors;
import com.jll.dtos.CartDto;
import com.jll.dtos.ItemDto;
import com.jll.dtos.PostCartDto;
import com.jll.dtos.PostItemDto;
import com.jll.models.*;
import com.jll.repositories.CartRepository;
import com.jll.repositories.ItemRepository;
import com.jll.utilities.LocalConnectionManagerFactory;
import org.apache.coyote.Response;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.security.cert.CollectionCertStoreParameters;
import java.sql.SQLException;
import java.util.Map;
import java.util.Optional;
import java.util.UUID;
import java.util.stream.Collectors;

@RestController
@RequestMapping("/carts")
public class CartController {

    @GetMapping()
    public ResponseEntity get() {
        var cartRepository = new CartRepository(LocalConnectionManagerFactory.Get());
        try {
            var cartDtos = cartRepository.get(20).stream()
                    .map(cart -> new CartDto(cart));
            return ResponseEntity.ok(cartDtos);
        } catch (SQLException e) {
            e.printStackTrace();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body("Could not retrieve Carts");
        }
    }

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
                    .body("There should be at least one Item in your cart");
        }

        var itemIds =
                postCartDto.CartItems.stream()
                        .map(cartItemDto -> cartItemDto.ItemId)
                        .collect(Collectors.toList());

        try {
            var itemRepository = new ItemRepository(LocalConnectionManagerFactory.Get());
            var queriedItems = itemRepository.get(itemIds);

            var matchingItemsMap =
                    postCartDto.CartItems.stream()
                            .collect(Collectors.toMap(
                                    cartItem -> cartItem,
                                    cartItem -> queriedItems.stream()
                                            .filter(item -> item.getId().equals(cartItem.ItemId))
                                            .collect(MoreCollectors.toOptional())));

            var missingCartItems =
                    matchingItemsMap.entrySet().stream()
                            .filter(pair -> !pair.getValue().isPresent())
                            .map(pair -> pair.getKey())
                            .collect(Collectors.toList());

            if (!missingCartItems.isEmpty()) {
                var missingItemIds = missingCartItems.stream()
                        .map(cartItemDto -> cartItemDto.ItemId.toString())
                        .collect(Collectors.joining(","));
                return ResponseEntity.badRequest()
                        .body("The following Items could not be found: " + missingItemIds);
            }

            var itemsForPurchase =
                    matchingItemsMap.entrySet().stream()
                            .filter(pair -> pair.getValue().isPresent())
                            .map(pair -> new ItemForPurchase(
                                    pair.getValue().get(),
                                    pair.getKey().Quantity
                            ))
                            .collect(Collectors.toList());

            var cart = new Cart(
                    UUID.randomUUID(),
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
}