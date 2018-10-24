package com.jll.repositories;

import com.jll.model.Coupon;
import com.jll.model.CouponIdentity;
import com.jll.utilities.ConnectionManager;

import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.util.Optional;

public class CouponRepository extends Repository<Coupon, CouponIdentity> {
    public CouponRepository(ConnectionManager connectionManager) {
        super(connectionManager, Coupon.class, "coupon");
    }

    public Optional<Coupon> getByCouponCode(String couponCode) throws SQLException {
        var sql = "SELECT * FROM shoppingcart." + getTableName() + " WHERE content::json#>>'{couponCode}' = ?";

        try (var connection = this.connectionManager.connect();
             PreparedStatement preparedStatement = connection.prepareCall(sql)) {
            preparedStatement.setString(1, couponCode);

            var result = preparedStatement.executeQuery();
            if (result.next()) {
                Coupon entity = getEntityFromResultSet(result);

                if (result.next()) {
                    throw new SQLException("Expected only 1 record. Returned more than 1.");
                }
                return Optional.of(entity);
            } else {
                return Optional.empty();
            }

        } catch (SQLException e) {
            throw e;
        }
    }
}
