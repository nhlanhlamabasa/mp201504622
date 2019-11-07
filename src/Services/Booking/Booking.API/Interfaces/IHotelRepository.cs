namespace Booking.API.Interfaces
{
#pragma warning disable
    using Booking.API.Entities;
    using HotelSystem.SharedKernel.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Hotel service interface
    /// </summary>
    public interface IHotelRepository
    {
        /// <summary>
        /// Gets hotel
        /// </summary>
        /// <param name="Id">Hotel Id</param>
        /// <returns>Hotel with given Id</returns>
        Task<Hotel> GetHotelById(Guid Id);

        /// <summary>
        /// The GetAllRooms
        /// </summary>
        /// <param name="Id">The Id<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{ICollection{Room}}"/></returns>
        Task<ICollection<Room>> GetAllRooms(Guid Id);

        /// <summary>
        /// The GetHotels
        /// </summary>
        /// <returns>The <see cref="ICollection{Hotel}"/></returns>
        ICollection<Hotel> GetHotels();

        /// <summary>
        /// Gets hotels
        /// </summary>
        /// <param name="pageIndex">The pageIndex<see cref="int"/></param>
        /// <param name="pageSize">The pageSize<see cref="int"/></param>
        /// <returns>The <see cref="PaginatedList{Hotel}"/></returns>
        PaginatedList<Hotel> GetHotels(int pageIndex, int pageSize);

        /// <summary>
        /// Creates new hotel
        /// </summary>
        /// <param name="hotel">A hotel</param>
        /// <returns>The <see cref="Task{Hotel}"/></returns>
        Task<Hotel> InsertHotel(Hotel hotel);

        /// <summary>
        /// Updates hotel
        /// </summary>
        /// <param name="hotel">Hotel to be updated</param>
        /// <returns>The <see cref="Task{Hotel}"/></returns>
        Task<Hotel> UpdateHotel(Hotel hotel);
    }
}
