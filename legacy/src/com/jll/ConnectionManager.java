package com.jll;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;

public class ConnectionManager {
    private ConnectionInfo connectionInfo;

    public ConnectionManager(ConnectionInfo connectionInfo) {
        this.connectionInfo = connectionInfo;
    }

    public ConnectionManager(String url, String user, String password) {
        this.connectionInfo = new ConnectionInfo();
        this.connectionInfo.Url = url;
        this.connectionInfo.User = user;
        this.connectionInfo.Password = password;
    }

    public Connection connect() throws SQLException {
        Connection conn;
        try {
            conn = DriverManager.getConnection(this.connectionInfo.Url, this.connectionInfo.User, this.connectionInfo.Password);
            System.out.println("Connected to the PostgreSQL server successfully.");
        } catch (SQLException e) {
            throw e;
        }

        return conn;
    }
}
