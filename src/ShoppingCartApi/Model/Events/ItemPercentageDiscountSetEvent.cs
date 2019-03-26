namespace ShoppingCartApi.Model.Events
{
    public class ItemPercentageDiscountSetEvent : IItemEvent
    {
        public Percent PercentOff { get; set; }
    }
}