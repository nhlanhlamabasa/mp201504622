namespace Booking.API.Specifications.RoomSpecifications
{
#pragma warning disable
    using Booking.API.Entities;
    using System;
    using System.Linq.Expressions;

    public class RoomQuerySpecification : QuerySpecification<Room>
    {

        private readonly Guid HotelId;


        public RoomQuerySpecification(Guid HotelId)
        {
            this.HotelId = HotelId;
        }

        public override Expression<Func<Room, bool>> Criteria =>
            x => x.HotelId == HotelId;
    }
}
