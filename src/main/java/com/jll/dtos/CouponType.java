package com.jll.dtos;

import com.fasterxml.jackson.annotation.JsonCreator;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.annotation.JsonValue;

import java.util.EnumSet;

public enum CouponType {
    @JsonProperty("store_wide")
    StoreWide,
    @JsonProperty("item_type")
    ItemType;
}
