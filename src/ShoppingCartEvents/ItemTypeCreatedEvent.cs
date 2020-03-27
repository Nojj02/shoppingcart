namespace ShoppingCartEvents
{
    public class ItemTypeCreatedEvent : DomainEvent, IItemTypeEvent
    {
        public string Code { get; set; }
    }
}