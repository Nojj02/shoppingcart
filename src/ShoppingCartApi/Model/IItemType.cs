namespace ShoppingCartApi.Model
{
    public interface IItemType : IAggregateRoot
    {
        string Code { get; }
    }
}