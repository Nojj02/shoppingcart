package com.jll.controllers;

import com.jll.dtos.CouponDto;
import com.jll.model.CouponIdentity;
import com.jll.model.CouponType;
import com.jll.dtos.PostCouponDto;
import com.jll.model.Coupon;
import com.jll.model.Discount;
import com.jll.repositories.CouponPostgresRepository;
import com.jll.utilities.LocalConnectionManagerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.sql.SQLException;
import java.util.UUID;

@RestController
@RequestMapping("/coupons")
public class CouponController {

    @GetMapping()
    public ResponseEntity get() {
        var couponRepository = new CouponPostgresRepository(LocalConnectionManagerFactory.Get());
        try {
            var couponDtos = couponRepository.get(20).stream()
                    .map(coupon -> new CouponDto(coupon));
            return ResponseEntity.ok(couponDtos);
        } catch (SQLException e) {
            e.printStackTrace();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body("Could not retrieve Coupons");
        }
    }

    @GetMapping("/{id}")
    public ResponseEntity get(@PathVariable UUID id) {
        var couponRepository = new CouponPostgresRepository(LocalConnectionManagerFactory.Get());
        try {
            return couponRepository.get(id)
                    .map(coupon -> (ResponseEntity)ResponseEntity.ok(new CouponDto(coupon)))
                    .orElse(ResponseEntity.status(HttpStatus.NOT_FOUND).body("Coupon " + id + " does not exist"));
        } catch (SQLException e) {
            e.printStackTrace();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body("Could not retrieve an Coupon");
        }
    }

    @PostMapping()
    public ResponseEntity save(@RequestBody PostCouponDto postCouponDto) {
        Coupon coupon;
        if (postCouponDto.CouponType == CouponType.StoreWide) {
            coupon = Coupon.storeWide(
                    new CouponIdentity(UUID.randomUUID()),
                    postCouponDto.CouponCode,
                    new Discount(postCouponDto.Discount.Percentage, postCouponDto.Discount.FixedAmount)
            );
        } else if(postCouponDto.CouponType == CouponType.ItemType) {
            coupon = Coupon.forItemType(
                    new CouponIdentity(UUID.randomUUID()),
                    postCouponDto.CouponCode,
                    new Discount(postCouponDto.Discount.Percentage, postCouponDto.Discount.FixedAmount),
                    postCouponDto.ItemTypeCode
            );
        } else {
            return ResponseEntity.badRequest().body("Unknown Coupon Type");
        }

        var couponRepository = new CouponPostgresRepository(LocalConnectionManagerFactory.Get());
        try {
            couponRepository.save(coupon);

            var couponDto = new CouponDto(coupon);
            return ResponseEntity.ok(couponDto);
        } catch (SQLException e) {
            e.printStackTrace();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                    .body("Could not save the Coupon");
        }
    }
}