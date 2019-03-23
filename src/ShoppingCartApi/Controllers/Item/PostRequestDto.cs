using System;

namespace ShoppingCartApi.Controllers.Item
{
    public class PostRequestDto
    {
        public string Code { get; set; }
        public decimal Price { get; set; }
        public Guid ItemTypeId { get; set; }
    }
}