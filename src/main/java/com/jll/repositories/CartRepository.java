package com.jll.repositories;

import com.google.gson.Gson;
import com.jll.models.Cart;
import com.jll.models.Item;
import com.jll.utilities.ConnectionManager;
import org.apache.catalina.startup.ClassLoaderFactory;
import org.postgresql.util.PGobject;

import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.sql.Timestamp;
import java.util.*;

public class CartRepository extends Repository<Cart> {
    public CartRepository(ConnectionManager connectionManager) {
        super(connectionManager, Cart.class, "cart");
    }
}
