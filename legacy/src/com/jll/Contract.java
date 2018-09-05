package com.jll;

public final class Contract {
    public void NotNull(Object obj) {
        if (obj == null) {
            throw new ContractException("Object cannot be null");
        }
    }

    public class ContractException extends RuntimeException {
        public ContractException(String message) {
            super(message);
        }
    }
}
