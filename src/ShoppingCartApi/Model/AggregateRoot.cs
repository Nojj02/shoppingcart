using System;
using System.Collections.Generic;

namespace ShoppingCartApi.Model
{
    public abstract class AggregateRoot : Entity, IAggregateRoot
    {
        protected AggregateRoot(Guid id) 
            : base(id)
        {
        }
    }
}