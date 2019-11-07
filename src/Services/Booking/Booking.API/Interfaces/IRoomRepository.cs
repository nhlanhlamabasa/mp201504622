namespace Booking.API.Interfaces
{
    using Booking.API.Entities;
    using Booking.API.Specifications.RoomSpecifications;
    using HotelSystem.SharedKernel.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IRoomRepository" />
    /// </summary>
    public interface IRoomRepository
    {
        /// <summary>
        /// Gets room
        /// </summary>
        /// <param name="id">room id</param>
        /// <returns></returns>
        Task<Room> GetRoomById(Guid id);

        /// <summary>
        /// Gets rooms
        /// </summary>
        /// <param name="pageIndex">The pageIndex<see cref="int"/></param>
        /// <param name="pageSize">The pageSize<see cref="int"/></param>
        /// <returns>The <see cref="Task{T}"/> where T is <see cref="PaginatedList{Room}"/></returns>
        PaginatedList<Room> GetRooms(int pageIndex, int pageSize);

        /// <summary>
        /// The GetRoomsWithSpecification
        /// </summary>
        /// <param name="roomQuery">The roomQuery<see cref="RoomQuerySpecification"/></param>
        /// <param name="noBookingsQuery">The noBookingsQuery<see cref="NoBookingsQuerySpecification"/></param>
        /// <param name="travelDatesQuery">The travelDatesQuery<see cref="TravelDatesQuerySpecification"/></param>
        /// <returns>The <see cref="List{Room}"/></returns>
        List<Room> GetRoomsWithSpecification(RoomQuerySpecification roomQuery,
            NoBookingsQuerySpecification noBookingsQuery, TravelDatesQuerySpecification travelDatesQuery);

        /// <summary>
        /// Get rooms
        /// </summary>
        /// <param name="roomQuery"></param>
        /// <param name="noBookingsQuery"></param>
        /// <param name="travelDatesQuery"></param>
        /// <param name="pageIndex">The pageIndex<see cref="int"/></param>
        /// <param name="pageSize">The pageSize<see cref="int"/></param>
        /// <returns>The <see cref="Task{T}"/> where T is <see cref="PaginatedList{Room}"/></returns>
        PaginatedList<Room> GetRoomsWithSpecification(RoomQuerySpecification roomQuery,
            NoBookingsQuerySpecification noBookingsQuery, TravelDatesQuerySpecification travelDatesQuery, int pageIndex, int pageSize);

        /// <summary>
        /// Creates new room
        /// </summary>
        /// <param name="room">Room to be inserted</param>
        /// <returns>Added hotel</returns>
        Task<Room> InsertRoom(Room room);

        /// <summary>
        /// Updates room
        /// </summary>
        /// <param name="room">Room to be updated</param>
        /// <returns>Updated room</returns>
        Task<Room> UpdateRoom(Room room);

        /// <summary>
        /// The UpdateRooms
        /// </summary>
        /// <param name="rooms">The rooms<see cref="ICollection{Room}"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task UpdateRooms(ICollection<Room> rooms);
    }
}
