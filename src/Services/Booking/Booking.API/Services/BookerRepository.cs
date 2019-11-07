namespace Booking.API.Services
{
    using Booking.API.Data;
    using Booking.API.Entities;
    using Booking.API.Interfaces;
    using Booking.API.Models;
    using HotelSystem.SharedKernel.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="BookerRepository" />
    /// </summary>
    public class BookerRepository : IBookerRepository
    {
        /// <summary>
        /// Defines the _context
        /// </summary>
        private readonly BookingContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookerRepository"/> class.
        /// </summary>
        /// <param name="context">The context<see cref="BookingContext"/></param>
        public BookerRepository(BookingContext context)
        {
            _context = context;
        }

        /// <summary>
        /// The GetBookingByBookerId
        /// </summary>
        /// <param name="bookerId">The bookerId<see cref="Guid"/></param>
        /// <param name="bookingId">The bookingId<see cref="Guid"/></param>
        /// <returns>The <see cref="Booking"/></returns>
        public Booking GetBookingByBookerId(Guid bookerId, Guid bookingId)
        {
            return _context.Bookings.Where(x => x.Id == bookingId && x.BookerId == bookerId).FirstOrDefault();
        }

        /// <summary>
        /// The GetBookingsByBookerId
        /// </summary>
        /// <param name="BoookerId">The BoookerId<see cref="Guid"/></param>
        /// <param name="pageIndex">The pageIndex<see cref="int"/></param>
        /// <param name="pageSize">The pageSize<see cref="int"/></param>
        /// <returns>The <see cref="PaginatedList{T}"> whose generic type argument is <see cref="Entities.Booking"/></see></returns>
        public PaginatedList<Booking> GetBookingsByBookerId(Guid BoookerId, int pageIndex, int pageSize)
        {
            if (pageSize > Constants.MAX_PAGE_SIZE)
            {
                pageSize = Constants.MAX_PAGE_SIZE;
            }
            IQueryable<Booking> collectionBeforePaging = _context.Bookings.Where(o => o.BookerId == BoookerId)
                .OrderByDescending(o => o.Arrival)
                .AsQueryable();
            return PaginatedList<Booking>.Create(collectionBeforePaging, pageIndex, pageSize);
        }

        /// <summary>
        /// The GetBookingsByBookerId
        /// </summary>
        /// <param name="bookerId">The bookerId<see cref="Guid"/></param>
        /// <returns>The <see cref="ICollection{Booking}"/></returns>
        public ICollection<Booking> GetBookingsByBookerId(Guid bookerId)
        {
            return _context.Bookings.Where(x => x.BookerId == bookerId).OrderByDescending(o => o.Arrival).ToList();
        }
    }
}
