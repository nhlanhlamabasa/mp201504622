#pragma warning disable
namespace Booking.API.Interfaces
{
    using Booking.API.Entities;
    using HotelSystem.SharedKernel;
    using HotelSystem.SharedKernel.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IManagerRepository" />
    /// </summary>
    public interface IManagerRepository
    {
        int NumberOfAmenities();

        int NumberOfRooms();

        int NumberOfHotels();

        decimal TotalPaymentsReceived(DateTime start, DateTime end);

        decimal TotalPaymentsReceived();

        PeriodSummary GetPeriodSummary(DateTime Start, DateTime End);

        Task<ICollection<Complaint>> GetComplaints();

        Task<Complaint> GetComplaint(Guid Id);

        /// <summary>
        /// The AddHotel
        /// </summary>
        /// <param name="hotel">The hotel<see cref="Hotel"/></param>
        /// <returns>The <see cref="Task{Hotel}"/></returns>
        Task<Hotel> AddHotel(Hotel hotel);

        /// <summary>
        /// The AddRoom
        /// </summary>
        /// <param name="HotelId">The HotelId<see cref="Guid"/></param>
        /// <param name="room">The room<see cref="Room"/></param>
        /// <returns>The <see cref="Task{Room}"/></returns>
        Task<Room> AddRoom(Guid HotelId, Room room);

        /// <summary>
        /// The EditHotel
        /// </summary>
        /// <param name="HotelId">The HotelId<see cref="Guid"/></param>
        /// <param name="hotel">The hotel<see cref="Hotel"/></param>
        /// <returns>The <see cref="Task{Hotel}"/></returns>
        Task<Hotel> EditHotel(Guid HotelId, Hotel hotel);

        /// <summary>
        /// The EditRoom
        /// </summary>
        /// <param name="HotelId">The HotelId<see cref="Guid"/></param>
        /// <param name="RoomId">The RoomId<see cref="Guid"/></param>
        /// <param name="room">The room<see cref="Room"/></param>
        /// <returns>The <see cref="Task{Room}"/></returns>
        Task<Room> EditRoom(Guid HotelId, Guid RoomId, Room room);

        /// <summary>
        /// The GetHotel
        /// </summary>
        /// <param name="HotelId">The HotelId<see cref="Guid"/></param>
        /// <param name="WithDetails">The WithDetails<see cref="bool"/></param>
        /// <returns>The <see cref="Task{Hotel}"/></returns>
        Task<Hotel> GetHotel(Guid HotelId, bool WithDetails = false);

        /// <summary>
        /// The GetHotels
        /// </summary>
        /// <returns>The <see cref="Task{ICollection{Hotel}}"/></returns>
        Task<ICollection<Hotel>> GetHotels();

        /// <summary>
        /// The GetRoom
        /// </summary>
        /// <param name="HotelId">The HotelId<see cref="Guid"/></param>
        /// <param name="RoomId">The RoomId<see cref="Guid"/></param>
        /// <param name="WithDetails">The WithDetails<see cref="bool"/></param>
        /// <returns>The <see cref="Task{Room}"/></returns>
        Task<Room> GetRoom(Guid HotelId, Guid RoomId, bool WithDetails = false);

        /// <summary>
        /// The GetRooms
        /// </summary>
        /// <param name="HotelId">The HotelId<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{ICollection{Room}}"/></returns>
        Task<ICollection<Room>> GetRooms(Guid HotelId);

        /// <summary>
        /// The RemoveRoom
        /// </summary>
        /// <param name="HotelId">The HotelId<see cref="Guid"/></param>
        /// <param name="RoomId">The RoomId<see cref="Guid"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task RemoveRoom(Guid HotelId, Guid RoomId);

        /// <summary>
        /// The RemoveRoom
        /// </summary>
        /// <param name="HotelId">The HotelId<see cref="Guid"/></param>
        /// <param name="RoomId">The RoomId<see cref="Guid"/></param>
        /// <param name="room">The room<see cref="Room"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task RemoveRoom(Guid HotelId, Guid RoomId, Room room);

        /// <summary>
        /// The RemoveHotel
        /// </summary>
        /// <param name="HotelId">The HotelId<see cref="Guid"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task RemoveHotel(Guid HotelId);

        /// <summary>
        /// The RemoveHotel
        /// </summary>
        /// <param name="HotelId">The HotelId<see cref="Guid"/></param>
        /// <param name="hotel">The hotel<see cref="Hotel"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task RemoveHotel(Guid HotelId, Hotel hotel);

        /// <summary>
        /// The AddAmenity
        /// </summary>
        /// <param name="model">The model<see cref="Amenity"/></param>
        /// <returns>The <see cref="Task{Amenity}"/></returns>
        Task<Amenity> AddAmenity(Amenity model);

        /// <summary>
        /// The GetAmenities
        /// </summary>
        /// <returns>The <see cref="Task{ICollection{Amenity}}"/></returns>
        Task<ICollection<Amenity>> GetAmenities();
    }
}
