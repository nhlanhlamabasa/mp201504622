namespace Booking.API.Interfaces
{
#pragma warning disable
    using Booking.API.Entities;
    using HotelSystem.SharedKernel.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Booking respository
    /// </summary>
    public interface IBookingRepository
    {
        Task<ICollection<Booking>> FrontDeskBookings(Guid Id);

        /// <summary>
        /// The AddComplaint
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/></param>
        /// <param name="complaint">The complaint<see cref="Complaint"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task<Booking> AddComplaint(Guid id, Complaint complaint);

        /// <summary>
        /// The CheckIn
        /// </summary>
        /// <param name="bookingId">The bookingId<see cref="Guid"/></param>
        /// <param name="CheckInPerson">The CheckInPerson<see cref="Guid"/></param>
        /// <param name="checkInCheckList">The checkInCheckList<see cref="CheckInCheckList"/></param>
        /// <returns>The <see cref="Task{Booking}"/></returns>
        Task<Booking> CheckIn(Guid bookingId, Guid CheckInPerson, CheckInCheckList checkInCheckList);

        /// <summary>
        /// The CheckOut
        /// </summary>
        /// <param name="bookingId">The bookingId<see cref="Guid"/></param>
        /// <param name="CheckOutPerson">The CheckOutPerson<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{Booking}"/></returns>
        Task<Booking> CheckOut(Guid bookingId, Guid CheckOutPerson);

        /// <summary>
        /// The DeleteBooking
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task DeleteBooking(Guid id);

        /// <summary>
        /// Gets booking
        /// </summary>
        /// <param name="id">Booking id</param>
        /// <param name="WithInvoice">The WithInvoice<see cref="bool"/></param>
        /// <returns></returns>
        Task<Booking> GetBookingById(Guid id, bool WithInvoice);

        /// <summary>
        /// The GetBookingComplaints
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{ICollection{Complaint}}"/></returns>
        Task<ICollection<Complaint>> GetBookingComplaints(Guid id);

        /// <summary>
        /// The GetBookings
        /// </summary>
        /// <returns>The <see cref="ICollection{Booking}"/></returns>
        ICollection<Booking> GetBookings();

        /// <summary>
        /// Gets list of bookings
        /// </summary>
        /// <param name="pageIndex">The pageIndex<see cref="int"/></param>
        /// <param name="pageSize">The pageSize<see cref="int"/></param>
        /// <returns>The <see cref="PaginatedList{T}"/> where T is <see cref="Booking"/></returns>
        PaginatedList<Booking> GetBookings(int pageIndex, int pageSize);

        /// <summary>
        /// The GetHotel
        /// </summary>
        /// <param name="hotelId">The hotelId<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{Hotel}"/></returns>
        Task<Hotel> GetHotel(Guid hotelId);

        /// <summary>
        /// The GetInvoice
        /// </summary>
        /// <param name="InvoiceId">The InvoiceId<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{Invoice}"/></returns>
        Task<Invoice> GetInvoice(Guid InvoiceId);

        /// <summary>
        /// The GetInvoiceByBookingId
        /// </summary>
        /// <param name="BookingId">The BookingId<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{Invoice}"/></returns>
        Task<Invoice> GetInvoiceByBookingId(Guid BookingId);

        /// <summary>
        /// The GetSelectedRooms
        /// </summary>
        /// <param name="BookingId">The BookingId<see cref="Guid"/></param>
        /// <returns>The <see cref="List{Room}"/></returns>
        List<Room> GetSelectedRooms(Guid BookingId);

        /// <summary>
        /// Creates bookings
        /// </summary>
        /// <param name="booking">Booking</param>
        /// <returns>The <see cref="PaginatedList{T}"/> where T is <see cref="Booking"/></returns>
        Task<Booking> InsertBooking(Booking booking);

        /// <summary>
        /// The InsertRoomBooking
        /// </summary>
        /// <param name="roomBooking">The roomBooking<see cref="RoomBooking"/></param>
        /// <returns>The <see cref="Task{RoomBooking}"/></returns>
        Task<RoomBooking> InsertRoomBooking(RoomBooking roomBooking);

        /// <summary>
        /// The InsertRoomBookings
        /// </summary>
        /// <param name="roomBookings">The roomBookings<see cref="ICollection{RoomBooking}"/></param>
        /// <returns>The <see cref="Task{RoomBooking}"/></returns>
        Task InsertRoomBookings(ICollection<RoomBooking> roomBookings);

        /// <summary>
        /// The PayInvoice
        /// </summary>
        /// <param name="payment">The payment<see cref="Payment"/></param>
        /// <returns>The <see cref="Task{Booking}"/></returns>
        Task<Booking> PayInvoice(Payment payment);

        /// <summary>
        /// The SearchBooking
        /// </summary>
        /// <param name="arrival">The arrival<see cref="DateTime"/></param>
        /// <param name="departure">The departure<see cref="DateTime"/></param>
        /// <param name="bookerId">The bookerId<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{Booking}"/></returns>
        Task<Booking> SearchBooking(DateTime arrival, DateTime departure, Guid bookerId);

        /// <summary>
        /// Updates booking
        /// </summary>
        /// <param name="booking"></param>
        /// <returns>The <see cref="PaginatedList{T}"/> where T is <see cref="Booking"/></returns>
        Task<Booking> UpdateBooking(Booking booking);

        /// <summary>
        /// The UpdateBookingInvoice
        /// </summary>
        /// <param name="booking">The booking<see cref="Booking"/></param>
        /// <returns>The <see cref="Task{Invoice}"/></returns>
        Task<Invoice> UpdateBookingInvoice(Booking booking);

        /// <summary>
        /// The UpdateComplaint
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/></param>
        /// <param name="complaint">The complaint<see cref="Complaint"/></param>
        /// <returns>The <see cref="Task{Booking}"/></returns>
        Task<Booking> UpdateComplaint(Guid id, Complaint complaint);
    }
}
