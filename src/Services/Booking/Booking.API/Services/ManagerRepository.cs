#pragma warning disable

namespace Booking.API.Services
{
    using Booking.API.Data;
    using Booking.API.Entities;
    using Booking.API.Exceptions;
    using Booking.API.Interfaces;
    using HotelSystem.SharedKernel;
    using HotelSystem.SharedKernel.ViewModels;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ManagerRepository : IManagerRepository
    {
        private readonly BookingContext _context;

        public ManagerRepository(BookingContext context)
        {
            _context = context;
        }

        public async Task<Amenity> AddAmenity(Amenity model)
        {
            var result = _context.Amenities.Add(model);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Hotel> AddHotel(Hotel hotel)
        {
            var result = _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Room> AddRoom(Guid HotelId, Room room)
        {
            room.HotelId = HotelId;
            var result = _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Hotel> EditHotel(Guid HotelId, Hotel hotel)
        {
            var result = _context.Hotels.Update(hotel);
            _context.SaveChanges();
            return result.Entity;
        }

        public async Task<Room> EditRoom(Guid HotelId, Guid RoomId, Room room)
        {
            var result = _context.Update(room);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<ICollection<Amenity>> GetAmenities()
        {
            return await _context.Amenities.OrderBy(o => o.Id).ToListAsync();
        }

        public int NumberOfAmenities()
        {
            return _context.Amenities.Count();
        }

        public int NumberOfRooms()
        {
            return _context.Rooms.Count();
        }

        public int NumberOfHotels()
        {
            return _context.Hotels.Count();
        }

        public decimal TotalPaymentsReceived(DateTime start, DateTime end)
        {
            decimal Total = 0;

            ICollection<Payment> payments = _context.Payments
                    .Where(x => x.CreationTime >= start)
                    .Where(x => x.CreationTime <= end)
                    .ToList();
            foreach (var payment in payments)
            {
                Total += payment.AmountPaid;
            }
            return Total;
        }

        public decimal TotalPaymentsReceived()
        {


            decimal Total = 0;
            foreach (var payment in _context.Payments)
            {
                Total += payment.AmountPaid;

            }
            return Total;
        }

        private Hotel MostBookings()
        {
            Hotel most = _context.Hotels.FirstOrDefault();
            foreach (Hotel hotel in _context.Hotels)
            {
                if (hotel.NumberOfBookings > most.NumberOfBookings)
                {
                    most = hotel;
                }
            }
            return most;
        }

        private Hotel LeastBookings()
        {
            Hotel least = _context.Hotels.FirstOrDefault();
            foreach (Hotel hotel in _context.Hotels)
            {
                if (hotel.NumberOfBookings < least.NumberOfBookings)
                {
                    least = hotel;
                }
            }
            return least;
        }

        public async Task<Complaint> GetComplaint(Guid Id)
        {
            return await _context.Complaints.DefaultIfEmpty(null).SingleAsync(x => x.Id == Id);
        }

        public async Task<ICollection<Complaint>> GetComplaints()
        {
            return await _context.Complaints.ToListAsync();
        }

        public PeriodSummary GetPeriodSummary(DateTime Start, DateTime End)
        {
            decimal total = TotalPaymentsReceived(Start, End);
            Hotel hotel = MostBookings();
            Hotel least = LeastBookings();

            PeriodSummary summary = new PeriodSummary(Start, End, total, hotel.Id, hotel.Name, least.Name, least.Id);


            return summary;
        }

        public async Task<Hotel> GetHotel(Guid HotelId, bool WithDetails = false)
        {
            return await _context.Hotels
                .Include(x => x.Rooms)
                    .ThenInclude(x => x.RoomBookings)
                .SingleOrDefaultAsync(x => x.Id == HotelId);
        }

        public async Task<ICollection<Hotel>> GetHotels()
        {
            ICollection<Hotel> hotels = await _context.Hotels
                .Include(x => x.Rooms)
                .ThenInclude(x => x.RoomBookings)
                    .ThenInclude(x => x.Booking)
                .ToListAsync();

            foreach (Hotel hotel in hotels)
            {
                ICollection<Room> rooms = await GetRooms(hotel.Id);
                int total = 0;
                foreach (Room room in rooms)
                {
                    total += room.RoomBookings.Count;
                }

                hotel.NumberOfBookings = total;
                hotel.NumberOfRooms = rooms.Count;
                _context.Hotels.Update(hotel);
                _context.SaveChanges();

            }

            return _context.Hotels.ToList();
        }

        public async Task<Room> GetRoom(Guid HotelId, Guid RoomId, bool WithDetails = false)
        {
            return await _context.Rooms.Include(x => x.RoomBookings).ThenInclude(x => x.Booking).SingleOrDefaultAsync(x => x.Id == RoomId);
        }

        public async Task<ICollection<Room>> GetRooms(Guid HotelId)
        {
            return await _context.Rooms.Include(x => x.RoomBookings).Where(x => x.HotelId == HotelId).ToListAsync();
        }

        public Task RemoveHotel(Guid HotelId)
        {
            Hotel hotel = GetHotelById(HotelId);
            _context.Hotels.Remove(hotel);
            _context.SaveChanges();
            return Task.FromResult(0);
        }

        public Task RemoveHotel(Guid HotelId, Hotel hotel)
        {
            hotel = GetHotelById(HotelId);
            _context.Hotels.Remove(hotel);
            _context.SaveChanges();
            return Task.FromResult(0);
        }

        public Task RemoveRoom(Guid HotelId, Guid RoomId)
        {
            Room room = GetRoomById(RoomId);
            _context.Rooms.Remove(room);
            _context.SaveChanges();
            return Task.FromResult(0);
        }

        public Task RemoveRoom(Guid HotelId, Guid RoomId, Room room)
        {
            room = GetRoomById(RoomId);
            _context.Rooms.Remove(room);
            _context.SaveChanges();
            return Task.FromResult(0);
        }

        private ICollection<Hotel> GetHotelsByManager(Guid ManagerId)
        {
            ICollection<Hotel> hotels = _context.Hotels.Where(x => x.ManagerId == ManagerId).ToList();
            if (hotels == null)
            {
                throw new NotFoundException("Manager has no hotels.");
            }
            else
            {
                return hotels;
            }
        }

        private Hotel GetHotelById(Guid HotelId)
        {
            Hotel hotel = _context.Hotels.SingleOrDefault(x => x.Id == HotelId);
            if (hotel == null)
            {
                throw new NotFoundException("Hotel not found.");
            }
            else
            {
                return hotel;
            }
        }

        private ICollection<Room> GetRoomsByHotel(Guid HotelId)
        {
            ICollection<Room> rooms = _context.Rooms.Where(x => x.HotelId == HotelId).ToList();
            if (rooms == null)
            {
                throw new NotFoundException("No rooms found.");
            }
            else
            {
                return rooms;
            }
        }

        private Room GetRoomById(Guid RoomId)
        {
            Room room = _context.Rooms.Single(x => x.Id == RoomId);
            if (room == null)
            {
                throw new NotFoundException("Room not found.");
            }
            else
            {
                return room;
            }
        }
    }
}
