package com.jll.utilities;

public class LocalConnectionManagerFactory {
    private static final String url = "jdbc:postgresql://localhost:5432/postgres";
    private static final String user = "postgres";
    private static final String password = "thepassword";
    private static ConnectionManager connectionManager = new ConnectionManager(url, user, password);

    public static ConnectionManager Get() {
        return connectionManager;
    }
}
