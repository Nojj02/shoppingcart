package com.jll.model;

import com.fasterxml.jackson.annotation.JsonProperty;

public enum CouponType {
    @JsonProperty("store_wide")
    StoreWide,
    @JsonProperty("item_type")
    ItemType
}
