namespace ShoppingCartApi.Controllers.Coupon
{
    public class PostRequestDto
    {
        public string Code { get; set; }
        public double PercentOff { get; set; }
        public decimal AmountOff { get; set; }
    }
}