namespace Booking.API.Specifications.RoomSpecifications
{
#pragma warning disable
    using Booking.API.Entities;
    using HotelSystem.SharedKernel;
    using System;
    using System.Linq.Expressions;


    public class TravelDatesQuerySpecification : QuerySpecification<RoomBooking>
    {

        private readonly DateTimeRange TravelDates;


        public TravelDatesQuerySpecification(DateTimeRange TravelDates)
        {
            this.TravelDates = TravelDates;
        }


        public override Expression<Func<RoomBooking, bool>> Criteria =>
            x => (x.Arrival < TravelDates.End && x.Departure > TravelDates.Start);
    }
}
