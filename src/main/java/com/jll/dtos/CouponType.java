package com.jll.dtos;

import com.fasterxml.jackson.annotation.JsonValue;

import java.util.EnumSet;

public enum CouponType {
    StoreWide {
        @Override
        public String toString() {
            return "Store-wide";
        }
    },
    ItemType {
        @Override
        public String toString() {
            return "Item Type";
        }
    };

    @JsonValue
    public String toJson() {
        return this.toString();
    }
}
