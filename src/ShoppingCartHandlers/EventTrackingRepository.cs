using System;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using ShoppingCartHandlers.DataAccess;
using ShoppingCartReader.DataAccess;

namespace ShoppingCartHandlers
{
    public class EventTrackingRepository : IEventTrackingRepository
    {
        public async Task<MessageNumber> GetLastMessageNumber(string resourceName)
        {
            await using var connection = new NpgsqlConnection(Database.ConnectionString);
            await connection.OpenAsync();
            var eventTracking = await GetEventTracking(resourceName);

            if (eventTracking == null)
            {
                eventTracking = EventTracking.New(resourceName);
                await SaveAsync(eventTracking);
            }

            return eventTracking.LastMessageNumber;
        }

        public async Task UpdateLastMessageNumberAsync(string resourceName, MessageNumber newLastMessageNumber)
        {
            var eventTracking = await GetEventTracking(resourceName);
            eventTracking.UpdateLastMessageNumber(newLastMessageNumber);
            await UpdateAsync(eventTracking);
        }

        private async Task<EventTracking> GetEventTracking(string resourceName)
        {
            await using var connection = new NpgsqlConnection(Database.ConnectionString);
            await connection.OpenAsync();
            return await connection.QuerySingleOrDefaultAsync<EventTracking>(
                $@"SELECT resource_name as ResourceName, 
                        last_message_number as LastMessageNumber
                    FROM shoppingcart.event_tracking EventTracking
                    WHERE resource_name = @resource_name",
                new
                {
                    resource_name = resourceName
                });
        }

        private async Task SaveAsync(EventTracking entity)
        {
            await using var connection = new NpgsqlConnection(Database.ConnectionString);
            await connection.OpenAsync();
            await using var transaction = connection.BeginTransaction();
            try
            {
                await connection.ExecuteAsync(
                    $@"INSERT INTO shoppingcart.event_tracking
                            (resource_name, last_message_number, timestamp) 
                        VALUES 
                            (@resource_name, @last_message_number, @timestamp)",
                    new
                    {
                        resource_name = entity.ResourceName,
                        last_message_number = entity.LastMessageNumber,
                        timestamp = DateTimeOffset.UtcNow
                    });

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task UpdateAsync(EventTracking entity)
        {
            await using var connection = new NpgsqlConnection(Database.ConnectionString);
            await connection.OpenAsync();
            await using var transaction = connection.BeginTransaction();
            try
            {
                await connection.ExecuteAsync(
                    $@"UPDATE shoppingcart.event_tracking
                            SET resource_name = @resource_name, last_message_number = @last_message_number, timestamp = @timestamp",
                    new
                    {
                        resource_name = entity.ResourceName,
                        last_message_number = entity.LastMessageNumber,
                        timestamp = DateTimeOffset.UtcNow
                    });

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}