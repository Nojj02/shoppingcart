using System;

namespace ShoppingCartReader.Model
{
    public class ItemTypeReadModel : IEntity
    {
        public Guid Id { get; set; }
        
        public string Code { get; set; }
    }
}