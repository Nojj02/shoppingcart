using System;
using System.Collections.Generic;
using ShoppingCartApi.Model.Events;

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