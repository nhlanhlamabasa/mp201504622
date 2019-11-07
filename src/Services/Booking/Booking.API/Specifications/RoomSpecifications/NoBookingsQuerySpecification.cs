namespace Booking.API.Specifications.RoomSpecifications
{
#pragma warning disable
    using Booking.API.Entities;
    using System;
    using System.Linq.Expressions;

    public class NoBookingsQuerySpecification : QuerySpecification<Room>
    {

        public NoBookingsQuerySpecification()
        {
        }

        public override Expression<Func<Room, bool>> Criteria => x => x.RoomBookings.Count == 0;
    }
}
