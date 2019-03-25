namespace ShoppingCartApi.Model
{
    public class ItemPercentageDiscountSetEvent : IItemEvent
    {
        public Percent PercentOff { get; set; }
    }
}