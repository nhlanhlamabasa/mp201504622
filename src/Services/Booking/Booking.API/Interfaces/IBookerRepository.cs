namespace Booking.API.Interfaces
{
#pragma warning disable
    using Booking.API.Entities;
    using HotelSystem.SharedKernel.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IBookerRepository" />
    /// </summary>
    public interface IBookerRepository
    {
        /// <summary>
        /// The GetBookingByBookerId
        /// </summary>
        /// <param name="bookerId">The bookerId<see cref="Guid"/></param>
        /// <param name="bookingId">The bookingId<see cref="Guid"/></param>
        /// <returns>The <see cref="Booking"/></returns>
        Booking GetBookingByBookerId(Guid bookerId, Guid bookingId);

        /// <summary>
        /// The GetBookingsByBookerId
        /// </summary>
        /// <param name="BoookerId">The BoookerId<see cref="Guid"/></param>
        /// <param name="pageIndex">The pageIndex<see cref="int"/></param>
        /// <param name="pageSize">The pageSize<see cref="int"/></param>
        /// <returns>The <see cref="Task{T}"/> where T is<see cref="PaginatedList{R}"/> where R is <see cref="Entities.Booking"></see></returns>
        PaginatedList<Entities.Booking> GetBookingsByBookerId(Guid BoookerId, int pageIndex, int pageSize);

        /// <summary>
        /// The GetBookingsByBookerId
        /// </summary>
        /// <param name="bookerId">The bookerId<see cref="Guid"/></param>
        /// <returns>The <see cref="ICollection{Booking}"/></returns>
        ICollection<Booking> GetBookingsByBookerId(Guid bookerId);
    }
}
