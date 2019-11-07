namespace Booking.API.Services
{
    using Booking.API.Data;
    using Booking.API.Entities;
    using Booking.API.Interfaces;
    using Booking.API.Models;
    using Booking.API.Specifications.RoomSpecifications;
    using HotelSystem.SharedKernel.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="RoomRepository" />
    /// </summary>
    public class RoomRepository : IRoomRepository
    {
        /// <summary>
        /// Defines the _context
        /// </summary>
        private readonly BookingContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomRepository"/> class.
        /// </summary>
        /// <param name="context">The context<see cref="BookingContext"/></param>
        public RoomRepository(BookingContext context)
        {
            _context = context;
        }

        /// <summary>
        /// The GetRoomById
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{Room}"/></returns>
        public async Task<Room> GetRoomById(Guid id)
        {
            Room result = await _context.Rooms.Where(x => x.Id == id).SingleOrDefaultAsync();
            return result;
        }

        /// <summary>
        /// Gets rooms
        /// </summary>
        /// <param name="pageIndex">The pageIndex<see cref="int"/></param>
        /// <param name="pageSize">The pageSize<see cref="int"/></param>
        /// <returns>A list of rooms</returns>
        public PaginatedList<Room> GetRooms(int pageIndex, int pageSize)
        {
            if (pageSize > Constants.MAX_PAGE_SIZE)
            {
                pageSize = Constants.MAX_PAGE_SIZE;
            }
            IQueryable<Room> collectionBeforePaging = _context.Rooms
                .OrderBy(o => o.RoomNumber)
                .AsQueryable();

            return PaginatedList<Room>.Create(collectionBeforePaging, pageIndex, pageSize);
        }

        /// <summary>
        /// The GetRoomsWithSpecification
        /// </summary>
        /// <param name="roomQuery">The roomQuery<see cref="RoomQuerySpecification"/></param>
        /// <param name="noBookingsQuery">The noBookingsQuery<see cref="NoBookingsQuerySpecification"/></param>
        /// <param name="travelDatesQuery">The travelDatesQuery<see cref="TravelDatesQuerySpecification"/></param>
        /// <returns>The <see cref="List{Room}"/></returns>
        public List<Room> GetRoomsWithSpecification(RoomQuerySpecification roomQuery,
            NoBookingsQuerySpecification noBookingsQuery, TravelDatesQuerySpecification travelDatesQuery)
        {
            IQueryable<Room> allRooms = _context.Rooms
                .Where(roomQuery.Criteria)
                .OrderBy(o => o.RoomNumber)
                .Include(o => o.Hotel)
                .AsQueryable();

            IQueryable<Room> roomsWithoutBookings = allRooms
                .Where(noBookingsQuery.Criteria)
                .OrderBy(o => o.RoomNumber)
                .AsQueryable();

            IQueryable<Room> roomsWithoutDates = _context.RoomBookings.Include(o => o.Room)
                .Where(travelDatesQuery.Criteria)
                .OrderBy(o => o.Room.RoomNumber)
                .Select(s => s.Room)
                .AsQueryable();

            List<Room> AvailableRooms = new List<Room>(roomsWithoutBookings);
            bool isUnique = true;

            foreach(Room noDates in roomsWithoutDates)
            {
                foreach(Room noBookings in roomsWithoutBookings)
                {
                    if (noDates.Id.Equals(noBookings.Id))
                    {
                        isUnique = false;
                        break;
                    }
                }

                if(isUnique)
                {
                    AvailableRooms.Add(noDates);
                }
                
            }

            foreach(Room room in AvailableRooms)
            {
                room.Amenities = _context.RoomAmenities
                    .Where(x => x.RoomId == room.Id)
                    .Include(o => o.Amenity)
                    .OrderBy(o => o.RoomId)
                    .ToList();
            }

            return AvailableRooms;
        }

        /// <summary>
        /// Get rooms
        /// </summary>
        /// <param name="roomQuery"></param>
        /// <param name="noBookingsQuery"></param>
        /// <param name="travelDatesQuery"></param>
        /// <param name="pageIndex">The pageIndex<see cref="int"/></param>
        /// <param name="pageSize">The pageSize<see cref="int"/></param>
        /// <returns>The <see cref="PaginatedList{Room}"/></returns>
        public PaginatedList<Room> GetRoomsWithSpecification(RoomQuerySpecification roomQuery,
            NoBookingsQuerySpecification noBookingsQuery, TravelDatesQuerySpecification travelDatesQuery, int pageIndex, int pageSize)
        {
            if (pageSize > Constants.MAX_PAGE_SIZE)
            {
                pageSize = Constants.MAX_PAGE_SIZE;
            }

            IQueryable<Room> allRooms = _context.Rooms
                .Where(roomQuery.Criteria)
                .OrderBy(o => o.RoomNumber)
                .AsQueryable();

            IQueryable<Room> roomsWithoutBookings = allRooms.Where(noBookingsQuery.Criteria)
                .OrderBy(o => o.RoomNumber)
                .AsQueryable();

            IQueryable<Room> roomsWithoutDates = _context.RoomBookings.Include(o => o.Room)
                .Where(travelDatesQuery.Criteria)
                .OrderBy(o => o.Room.RoomNumber)
                .Select(s => s.Room)
                .AsQueryable();

            IQueryable<Room> availableRooms = roomsWithoutBookings;

            return PaginatedList<Room>.Create(availableRooms, pageIndex, pageSize);
        }

        /// <summary>
        /// Creates new room
        /// </summary>
        /// <param name="room">Room to be inserted</param>
        /// <returns>Added hotel</returns>
        public async Task<Room> InsertRoom(Room room)
        {
            try
            {
                var inserted = await _context.Rooms.AddAsync(room);
                await _context.SaveChangesAsync();
                return inserted.Entity;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        /// <summary>
        /// Updates room
        /// </summary>
        /// <param name="room">Room to be updated</param>
        /// <returns>Updated room</returns>
        public async Task<Room> UpdateRoom(Room room)
        {
            try
            {
                var update = _context.Rooms.Update(room);
                await _context.SaveChangesAsync();
                return update.Entity;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        /// <summary>
        /// The UpdateRooms
        /// </summary>
        /// <param name="rooms">The rooms<see cref="ICollection{Room}"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task UpdateRooms(ICollection<Room> rooms)
        {
            try
            {
                _context.Rooms.UpdateRange(rooms);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
