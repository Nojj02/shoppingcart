namespace ShoppingCartApi.Model.Events
{
    public class ItemTypeCreatedEvent : IItemTypeEvent
    {
        public string Code { get; set; }
    }
}