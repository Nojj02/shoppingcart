using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShoppingCartApi.Model.Events;

namespace ShoppingCartApi.Model
{
    public class Coupon : AggregateRoot<ICouponEvent>, ICoupon
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <param name="percentOff"></param>
        /// <param name="forItemTypeId">If null, means the coupon applies store-wide</param>
        public Coupon(
            Guid id,
            string code, 
            Percent percentOff,
            Guid? forItemTypeId = null)
            : base(id)
        {
            Apply(
                new CouponCreatedEvent
                {
                    Id = id,
                    Version = CurrentVersion + 1,
                    Code = code,
                    PercentOff = percentOff,
                    ForItemTypeId = forItemTypeId
                },
                isNew: true);
        }

        public Coupon(
            Guid id,
            IReadOnlyList<ICouponEvent> events)
            : base(id, events)
        {
        }

        public string Code { get; private set; }

        public Percent PercentOff { get; private set; }

        public Guid? ForItemTypeId { get; private set; }

        public void Apply(CouponCreatedEvent anEvent, bool isNew = false)
        {
            Code = anEvent.Code;
            PercentOff = anEvent.PercentOff;
            ForItemTypeId = anEvent.ForItemTypeId;
            
            base.AddEvent(anEvent, isNew);
        }

        protected override void ApplyEventsOnConstruction(ICouponEvent anEvent)
        {
            switch (anEvent)
            {
                case CouponCreatedEvent couponCreatedEvent:
                    Apply(couponCreatedEvent);
                    break;
                default:
                    throw new UnsupportedEventException(anEvent.GetType());
            }
        }
    }
}
