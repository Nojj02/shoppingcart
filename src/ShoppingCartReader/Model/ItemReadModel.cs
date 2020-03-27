using System;
using ShoppingCartSharedKernel;

namespace ShoppingCartReader.Model
{
    public class ItemReadModel : IEntity
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public Guid ItemTypeId { get; set; }
        public decimal Price { get; set; }
        public Percent PercentOff { get; set; }
        public decimal AmountOff { get; set; }
    }
}