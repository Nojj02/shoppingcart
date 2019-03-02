namespace ShoppingCartConsole
{
    public class ShoppingItem
    {
        public ShoppingItem(Item item, double quantity)
        {
            ItemCode = item.Code;
            Quantity = quantity;
            Cost = item.Price;
        }

        public string ItemCode { get; set; }

        public decimal Cost { get; }
        
        public double Quantity { get; }
    }
}