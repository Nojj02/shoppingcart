namespace ShoppingCartEvents
{
    public class ItemAmountDiscountSetEvent : DomainEvent, IItemEvent
    {
        public decimal AmountOff { get; set; }
    }
}