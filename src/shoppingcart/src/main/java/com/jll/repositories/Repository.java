package com.jll.repositories;

import com.jll.model.AggregateRoot;
import com.jll.model.Identity;

import java.sql.SQLException;
import java.util.Optional;

public interface Repository<T extends AggregateRoot<TId>, TId extends Identity> {
    Optional<T> get(TId id)
            throws SQLException;
    void save(T aggregateRoot)
            throws SQLException;
}
