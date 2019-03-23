using System;

namespace ShoppingCartApi.Model
{
    public abstract class AggregateRoot : Entity
    {
        protected AggregateRoot(Guid id) 
            : base(id)
        {
        }
    }
}