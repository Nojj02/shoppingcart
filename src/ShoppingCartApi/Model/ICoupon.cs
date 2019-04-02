using System;

namespace ShoppingCartApi.Model
{
    public interface ICoupon : IEntity
    {
        string Code { get; }
        Percent PercentOff { get; }
        Guid? ForItemTypeId { get; }
    }
}