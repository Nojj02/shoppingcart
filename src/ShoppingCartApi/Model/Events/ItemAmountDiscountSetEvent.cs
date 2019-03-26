namespace ShoppingCartApi.Model.Events
{
    public class ItemAmountDiscountSetEvent : IItemEvent
    {
        public decimal AmountOff { get; set; }
    }
}