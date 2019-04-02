using System;

namespace ShoppingCartApi.Model
{
    public class ItemReadModel : IItem
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public Guid ItemTypeId { get; set; }
        public decimal Price { get; set; }
        public Percent PercentOff { get; set; }
        public decimal AmountOff { get; set; }

        public static ItemReadModel Map(Item x)
        {
            var itemReadModel = new ItemReadModel();
            itemReadModel.Id = x.Id;
            itemReadModel.ItemTypeId = x.ItemTypeId;
            itemReadModel.AmountOff = x.AmountOff;
            itemReadModel.Code = x.Code;
            itemReadModel.PercentOff = x.PercentOff;
            itemReadModel.Price = x.Price;
            return itemReadModel;
        }
    }
}