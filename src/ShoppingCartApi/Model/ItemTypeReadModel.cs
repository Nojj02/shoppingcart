using System;

namespace ShoppingCartApi.Model
{
    public class ItemTypeReadModel : IItemType
    {
        public Guid Id { get; set; }
        
        public string Code { get; set; }

        public static ItemTypeReadModel Map(ItemType x)
        {
            return new ItemTypeReadModel {Id = x.Id, Code = x.Code};
        }
    }
}