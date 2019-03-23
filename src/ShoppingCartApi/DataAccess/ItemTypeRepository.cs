﻿using System;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public class ItemTypeRepository : Repository<ItemType>, IItemTypeRepository
    {
        public ItemTypeRepository(string connectionString)
            : base(connectionString)
        {
        }

        protected override string TableName => "item_type";

        public async Task<ItemType> GetAsync(string code)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var content =
                    await connection.QuerySingleOrDefaultAsync<string>(
                        "SELECT content " +
                        $"FROM {SchemaAndTableName} " +
                        "WHERE content->>'Code' = @code",
                        new
                        {
                            code = code
                        });

                return content == null ? null : JsonConvert.DeserializeObject<ItemType>(content);
            }
        }
    }
}