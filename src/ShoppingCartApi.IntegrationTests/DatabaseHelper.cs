using Dapper;
using Npgsql;

namespace ShoppingCartApi.IntegrationTests
{
    public class DatabaseHelper
    {
        private const string ConnectionString = "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=thepassword;";

        public static void DeleteAllData()
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var deletionSql = @"
DELETE FROM shoppingcart.item;
DELETE FROM shoppingcart.cart;
DELETE FROM shoppingcart.coupon;";

                connection.Execute(deletionSql);
            }
        }
    }
}