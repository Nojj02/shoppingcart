namespace ShoppingCartApi.Model
{
    public class ItemAmountDiscountSetEvent : IItemEvent
    {
        public decimal AmountOff { get; set; }
    }
}