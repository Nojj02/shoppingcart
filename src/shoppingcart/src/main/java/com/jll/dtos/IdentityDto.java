package com.jll.dtos;

import com.jll.models.Identity;

import java.util.UUID;

public class IdentityDto {
    public UUID id;

    public static IdentityDto fromIdentity(Identity identity) {
        var identityDto = new IdentityDto();
        identityDto.id = identity.getValue();
        return identityDto;
    }
}
