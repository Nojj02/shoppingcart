namespace ShoppingCartApi.Controllers.Item
{
    public class Item
    {
        public Item(
            string code,
            decimal price)
        {
            Code = code;
            Price = price;
        }
        
        public string Code { get; }
        
        public decimal Price { get; }
    }
}