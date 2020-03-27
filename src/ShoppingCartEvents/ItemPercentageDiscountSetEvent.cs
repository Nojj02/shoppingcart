using ShoppingCartSharedKernel;

namespace ShoppingCartEvents
{
    public class ItemPercentageDiscountSetEvent : DomainEvent, IItemEvent
    {
        public Percent PercentOff { get; set; }
    }
}