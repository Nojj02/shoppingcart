using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCartEvents;
using ShoppingCartReader.DataAccess;
using ShoppingCartReader.Model;

namespace ShoppingCartHandlers.Handlers
{
    public class CouponReadModelHandler : IEventHandler
    {
        private readonly ICouponReadRepository _couponReadRepository;

        public CouponReadModelHandler(ICouponReadRepository couponReadRepository)
        {
            _couponReadRepository = couponReadRepository;
        }

        public async Task Handle(IList<object> newEvents)
        {
            foreach (var newEvent in newEvents)
            {
                if (newEvent is CouponCreatedEvent couponCreatedEvent)
                {
                    var newCoupon = new CouponReadModel
                    {
                        Id = couponCreatedEvent.Id,
                        Code = couponCreatedEvent.Code,
                        PercentOff = couponCreatedEvent.PercentOff,
                        ForItemTypeId = couponCreatedEvent.ForItemTypeId
                    };
                    await _couponReadRepository.SaveAsync(newCoupon);
                }
            }
        }
    }
}