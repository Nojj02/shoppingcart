using System;

namespace ShoppingCartApi.Model
{
    public interface IItem : IAggregateRoot
    {
        string Code { get; }
        Guid ItemTypeId { get; }
        decimal Price { get; }
        Percent PercentOff { get; }
        decimal AmountOff { get; }
    }
}