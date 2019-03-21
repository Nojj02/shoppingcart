namespace ShoppingCartApi.Controllers.Item
{
    public class SetDiscountRequestDto
    {
        public double PercentOff { get; set; }
        public decimal AmountOff { get; set; }
    }
}