using System;

namespace ShoppingCartApi.Model
{
    public interface ICoupon : IAggregateRoot
    {
        string Code { get; }
        Percent PercentOff { get; }
        Guid? ForItemTypeId { get; }
    }
}