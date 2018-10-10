package com.jll.dtos;

import org.springframework.hateoas.Link;

import java.util.Collection;

public class TransportMessage {
    public Link _links;
    public Collection<DomainMessage> messages;
}
