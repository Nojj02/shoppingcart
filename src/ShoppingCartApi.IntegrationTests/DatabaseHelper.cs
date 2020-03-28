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
DELETE FROM shoppingcart_views.item_type;
DELETE FROM shoppingcart_views.item;
DELETE FROM shoppingcart_views.coupon;
DELETE FROM shoppingcart.item_type;
DELETE FROM shoppingcart.item;
DELETE FROM shoppingcart.cart;
DELETE FROM shoppingcart.coupon;
DELETE FROM shoppingcart.event_tracking";

                connection.Execute(deletionSql);
            }
        }
    }
}