namespace Booking.API.Services
{
    #pragma warning disable
    using Booking.API.Data;
    using Booking.API.Entities;
    using Booking.API.Interfaces;
    using Booking.API.Models;
    using HotelSystem.SharedKernel.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// An implementation of the hotel service interface
    /// </summary>
    public class HotelRepository : IHotelRepository
    {
        /// <summary>
        /// Defines the _context
        /// </summary>
        private readonly BookingContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="HotelRepository"/> class.
        /// </summary>
        /// <param name="context">BookingContext instatiated by DI</param>
        public HotelRepository(BookingContext context)
        {
            _context = context;
        }

        /// <summary>
        /// The GetAllRooms
        /// </summary>
        /// <param name="Id">The Id<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{ICollection{Room}}"/></returns>
        public async Task<ICollection<Room>> GetAllRooms(Guid Id)
        {
            return await _context.Rooms
                .Where(x => x.HotelId == Id)
                .ToListAsync();
        }

        /// <summary>
        /// Gets hotel
        /// </summary>
        /// <param name="Id">Hotel Id</param>
        /// <returns>Hotel with given Id</returns>
        public async Task<Hotel> GetHotelById(Guid Id)
        {
            return await _context.Hotels
                .SingleOrDefaultAsync(x => x.Id == Id);
        }

        /// <summary>
        /// The GetHotels
        /// </summary>
        /// <returns>The <see cref="ICollection{Hotel}"/></returns>
        public ICollection<Hotel> GetHotels()
        {
            return _context.Hotels.OrderBy(o => o.Id).ToList();
        }

        /// <summary>
        /// Gets hotels
        /// </summary>
        /// <param name="pageIndex">The pageIndex<see cref="int"/></param>
        /// <param name="pageSize">The pageSize<see cref="int"/></param>
        /// <returns>The <see cref="PaginatedList{Hotel}"/></returns>
        public PaginatedList<Hotel> GetHotels(int pageIndex, int pageSize)
        {
            if (pageSize > Constants.MAX_PAGE_SIZE)
            {
                pageSize = Constants.MAX_PAGE_SIZE;
            }
            IQueryable<Hotel> collectionBeforePaging = _context.Hotels
                .OrderBy(o => o.Id).AsQueryable();
            return PaginatedList<Hotel>.Create(collectionBeforePaging, pageIndex, pageSize);
        }

        /// <summary>
        /// Creates new hotel
        /// </summary>
        /// <param name="hotel">A hotel</param>
        /// <returns>A created hotel</returns>
        public async Task<Hotel> InsertHotel(Hotel hotel)
        {
            try
            {
                var inserted = await _context.Hotels.AddAsync(hotel);
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
        /// Updates hotel
        /// </summary>
        /// <param name="hotel">Hotel to be updated</param>
        /// <returns>returns updated hotel</returns>
        public async Task<Hotel> UpdateHotel(Hotel hotel)
        {
            try
            {
                var updated = _context.Hotels.Update(hotel);
                await _context.SaveChangesAsync();
                return updated.Entity;
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
    }
}
