package com.jll.repositories;

public class UnknownEntityTypeException extends RuntimeException {
    public UnknownEntityTypeException(ClassNotFoundException e) {
        super("An unknown type was saved", e);
    }
}
