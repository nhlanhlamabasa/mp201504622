namespace Booking.API.Services
{
#pragma warning disable
    using Booking.API.Data;
    using Booking.API.Entities;
    using Booking.API.Exceptions;
    using Booking.API.Interfaces;
    using Booking.API.Models;
    using Booking.API.Specifications.RoomSpecifications;
    using HotelSystem.SharedKernel;
    using HotelSystem.SharedKernel.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Booking repositry implementation
    /// </summary>
    public class BookingRepository : IBookingRepository
    {
        public async Task<ICollection<Booking>> FrontDeskBookings(Guid Id)
        {
            return await _context.Bookings
                .Include(x => x.RoomBookings)
                    .ThenInclude(x => x.Room)
                        .ThenInclude(x => x.Amenities)
                .Where(x => x.CheckInPerson == Id || x.CheckOutPerson == Id).ToListAsync();
        }

        /// <summary>
        /// Defines the _context
        /// </summary>
        private readonly BookingContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingRepository"/> class.
        /// </summary>
        /// <param name="context"></param>
        public BookingRepository(BookingContext context)
        {
            _context = context;
        }

        /// <summary>
        /// The AddComplaint
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/></param>
        /// <param name="complaint">The complaint<see cref="Complaint"/></param>
        /// <returns>The <see cref="Task{Booking}"/></returns>
        public async Task<Booking> AddComplaint(Guid id, Complaint complaint)
        {
            Booking booking = _context.Bookings.AsTracking().Single(x => x.Id == id);
            complaint.Booking = booking;
            _context.Complaints.Add(complaint);
            await _context.SaveChangesAsync();
            return booking;
        }

        /// <summary>
        /// The CheckIn
        /// </summary>
        /// <param name="bookingId">The bookingId<see cref="Guid"/></param>
        /// <param name="CheckInPerson">The CheckInPerson<see cref="Guid"/></param>
        /// <param name="checkInCheckList"></param>
        /// <returns>The <see cref="Task{Booking}"/></returns>
        public async Task<Booking> CheckIn(Guid bookingId, Guid CheckInPerson, CheckInCheckList checkInCheckList)
        {
            Booking booking = _context.Bookings.Find(bookingId);
            booking.Status = BookingStatus.CheckedIn.ToString();
            booking.CheckInPerson = CheckInPerson;

            booking.CheckInCheckList = new CheckInCheckList(checkInCheckList.KeysReceived,
                checkInCheckList.AllAmenities,
                checkInCheckList.PrintedRecepiet,
                checkInCheckList.BookingId);

            var result = _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        /// <summary>
        /// The CheckOut
        /// </summary>
        /// <param name="bookingId">The bookingId<see cref="Guid"/></param>
        /// <param name="CheckOutPerson">The CheckOutPerson<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{Booking}"/></returns>
        public async Task<Booking> CheckOut(Guid bookingId, Guid CheckOutPerson)
        {
            Booking booking = _context.Bookings.Find(bookingId);
            booking.Status = BookingStatus.CheckedOut.ToString();
            booking.CheckOutPerson = CheckOutPerson;
            var result = _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        /// <summary>
        /// The DeleteBooking
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public Task DeleteBooking(Guid id)
        {
            Booking booking = _context.Bookings.Include(x => x.RoomBookings)
                .ThenInclude(x => x.Room)
                .SingleOrDefault(x => x.Id == id);

            if (booking == null)
            {
                throw new NotFoundException("Booking not found");
            }

            booking.Status = BookingStatus.Cancelled.ToString();

            foreach (var room in booking.RoomBookings)
            {
                room.Room = null;
                room.Booking = null;
            }

            booking.RoomBookings = null;

            _context.Bookings.Update(booking);
            _context.SaveChanges();
            return Task.FromResult(0);
        }

        /// <summary>
        /// Gets booking
        /// </summary>
        /// <param name="id">booking id</param>
        /// <param name="WithInvoice">The WithInvoice<see cref="bool"/></param>
        /// <returns></returns>
        public async Task<Booking> GetBookingById(Guid id, bool WithInvoice)
        {
            if (WithInvoice)
            {
                return await _context.Bookings
                    .Include(x => x.Invoice)
                    .Include(x => x.RoomBookings)
                        .ThenInclude(x => x.Booking)
                    .Include(x => x.RoomBookings)
                        .ThenInclude(x => x.Room)
                    .SingleOrDefaultAsync(x => x.Id == id);
            }
            else
            {
                return await _context.Bookings
                    .SingleOrDefaultAsync(x => x.Id == id);
            }
        }

        /// <summary>
        /// The GetBookingComplaints
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{ICollection{Complaint}}"/></returns>
        public async Task<ICollection<Complaint>> GetBookingComplaints(Guid id)
        {
            return await _context.Complaints.Where(x => x.BookingId == id).ToListAsync();
        }

        /// <summary>
        /// The GetBookings
        /// </summary>
        /// <returns>The <see cref="ICollection{Booking}"/></returns>
        public ICollection<Booking> GetBookings()
        {
            return _context.Bookings.OrderBy(o => o.Id).ToList();
        }

        /// <summary>
        /// Gets bookings
        /// </summary>
        /// <param name="pageIndex">The pageIndex<see cref="int"/></param>
        /// <param name="pageSize">The pageSize<see cref="int"/></param>
        /// <returns></returns>
        public PaginatedList<Booking> GetBookings(int pageIndex, int pageSize)
        {
            if (pageSize > Constants.MAX_PAGE_SIZE)
            {
                pageSize = Constants.MAX_PAGE_SIZE;
            }
            IQueryable<Booking> collectionBeforePaging = _context.Bookings.OrderBy(o => o.Id).AsQueryable();
            return PaginatedList<Booking>.Create(collectionBeforePaging, pageIndex, pageSize);
        }

        /// <summary>
        /// The GetHotel
        /// </summary>
        /// <param name="hotelId">The hotelId<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{Hotel}"/></returns>
        public async Task<Hotel> GetHotel(Guid hotelId)
        {
            return await _context.Hotels.FindAsync(hotelId);
        }

        /// <summary>
        /// The GetInvoice
        /// </summary>
        /// <param name="InvoiceId">The InvoiceId<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{Invoice}"/></returns>
        public async Task<Invoice> GetInvoice(Guid InvoiceId)
        {
            return await _context.Invoices
                .Include(x => x.Lines)
                .Include(x => x.Payment)
                .SingleAsync(x => x.Id == InvoiceId);
        }

        /// <summary>
        /// The GetInvoiceByBookingId
        /// </summary>
        /// <param name="BookingId">The BookingId<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{Invoice}"/></returns>
        public async Task<Invoice> GetInvoiceByBookingId(Guid BookingId)
        {
            return await _context.Invoices.Include(x => x.Lines).Include(x => x.Payment).SingleOrDefaultAsync(x => x.BookingId == BookingId);
        }

        /// <summary>
        /// The GetSelectedRooms
        /// </summary>
        /// <param name="BookingId">The BookingId<see cref="Guid"/></param>
        /// <returns>The <see cref="List{Room}"/></returns>
        public List<Room> GetSelectedRooms(Guid BookingId)
        {
            List<Room> selected = new List<Room>();
            var roomBookings = _context.RoomBookings
                .Include(x => x.Room)
                .Include(x => x.Booking)
                .Where(x => x.BookingId == BookingId).ToList();
            foreach (var roomBooking in roomBookings)
            {
                Room r = roomBooking.Room;
                r.Amenities = GetRoomAmenities(r.Id);
                selected.Add(r);
            }

            return selected;
        }

        /// <summary>
        /// Creates Booking
        /// </summary>
        /// <param name="booking"></param>
        /// <returns>Added booking</returns>
        public async Task<Booking> InsertBooking(Booking booking)
        {
            ValidateBooking(booking);

            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        ICollection<Room> SelectedRooms = booking.SelectedRooms;
                        booking.Status = BookingStatus.Paid.ToString();
                        EntityEntry<Booking> inserted = _context.Bookings.Add(booking);
                        await _context.SaveChangesAsync();
                        booking = inserted.Entity;

                        foreach (Room room in SelectedRooms)
                        {
                            //Update room
                            _context.Rooms.Attach(room).State = EntityState.Modified;

                            //create room booking
                            RoomBooking roomBooking = new RoomBooking();

                            //link data
                            roomBooking.Booking = booking;
                            roomBooking.Room = room;
                            roomBooking.Arrival = booking.Arrival;
                            roomBooking.Departure = booking.Departure;

                            //add room booking
                            booking.RoomBookings.Add(roomBooking);

                            //Save
                            var e = _context.Bookings.Update(booking);
                            _context.Rooms.Update(room);
                            _context.SaveChanges();
                            booking = e.Entity;
                        }

                        booking = _context.Bookings
                        .Include(x => x.RoomBookings)
                            .ThenInclude(x => x.Room)
                        .Include(x => x.RoomBookings)
                            .ThenInclude(x => x.Booking)
                        .SingleOrDefault(x => x.Id == booking.Id);

                        transaction.Commit();
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

            });

            return booking;
        }

        /// <summary>
        /// The InsertRoomBooking
        /// </summary>
        /// <param name="roomBooking">The roomBooking<see cref="RoomBooking"/></param>
        /// <returns>The <see cref="Task{RoomBooking}"/></returns>
        public async Task<RoomBooking> InsertRoomBooking(RoomBooking roomBooking)
        {
            try
            {
                EntityEntry<RoomBooking> inserted = _context.RoomBookings.Add(roomBooking);
                await _context.SaveChangesAsync();
                return inserted.Entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The InsertRoomBookings
        /// </summary>
        /// <param name="roomBookings">The roomBookings<see cref="ICollection{RoomBooking}"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task InsertRoomBookings(ICollection<RoomBooking> roomBookings)
        {
            try
            {
                _context.RoomBookings.AddRange(roomBookings);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The PayInvoice
        /// </summary>
        /// <param name="model">The payment<see cref="Payment"/></param>
        /// <returns>The <see cref="Task{Booking}"/></returns>
        public async Task<Booking> PayInvoice(Payment model)
        {
            bool isSuccess = Enum.TryParse(model.Payment_Method, out PaymentMethod payment);
            if (isSuccess)
            {
                switch (payment)
                {
                    case PaymentMethod.CreditCard:
                        {
                            _context.Payments.Add(model);
                            await _context.SaveChangesAsync();
                            Booking booking = await GetBookingById(model.Invoice.BookingId, true);
                            booking.Status = BookingStatus.Paid.ToString();
                            booking = await UpdateBooking(booking);
                            return booking;
                        }
                }
            }
            throw new PaymentException("Payment Failed");
        }

        /// <summary>
        /// The SearchBooking
        /// </summary>
        /// <param name="arrival">The arrival<see cref="DateTime"/></param>
        /// <param name="departure">The departure<see cref="DateTime"/></param>
        /// <param name="bookerId">The bookerId<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{Booking}"/></returns>
        public async Task<Booking> SearchBooking(DateTime arrival, DateTime departure, Guid bookerId)
        {
            ICollection<Booking> bookings = await _context.Bookings.Include(x => x.Invoice)
                .Where(x => x.BookerId == bookerId).ToListAsync();

            DateTimeRange dateTimeRange = new DateTimeRange(arrival, departure);
            if (bookings == null)
            {
                throw new ArgumentNullException("No bookings were found");
            }
            else
            {
                if (bookings.Count == 1)
                {
                    Booking booking = bookings.FirstOrDefault();
                    booking.SelectedRooms = GetSelectedRooms(booking.Id);
                    return booking;
                }
                else
                {
                    foreach (var b in bookings)
                    {
                        DateTimeRange db = new DateTimeRange(b.Arrival, b.Departure);
                        if (db.Equals(dateTimeRange))
                        {
                            b.SelectedRooms = GetSelectedRooms(b.Id);
                            return b;
                        }
                    }
                    throw new ArgumentNullException("No bookings were found");
                }
            }
        }

        /// <summary>
        /// Updates booking
        /// </summary>
        /// <param name="booking"></param>
        /// <returns>Updated booking</returns>
        public async Task<Booking> UpdateBooking(Booking booking)
        {
            try
            {
                EntityEntry<Booking> updated = _context.Bookings.Update(booking);
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

        /// <summary>
        /// The UpdateBookingInvoice
        /// </summary>
        /// <param name="booking">The booking<see cref="Booking"/></param>
        /// <returns>The <see cref="Task{Invoice}"/></returns>
        public Task<Invoice> UpdateBookingInvoice(Booking booking)
        {
            throw new NotImplementedException();
        }

        public async Task<Booking> UpdateComplaint(Guid id, Complaint complaint)
        {
            Booking booking = _context.Bookings.AsTracking().Single(x => x.Id == id);
            complaint.Booking = booking;
            _context.Complaints.Update(complaint);
            await _context.SaveChangesAsync();
            return booking;
        }

        /// <summary>
        /// The GenerateBookingInvoice
        /// </summary>
        /// <param name="booking">The booking<see cref="Booking"/></param>
        /// <returns>The <see cref="Invoice"/></returns>
        private Invoice GenerateBookingInvoice(Booking booking)
        {
            //New Invoice
            EntityEntry<Invoice> entity = _context.Invoices.Add(booking.Invoice);
            _context.SaveChanges();
            return entity.Entity;
        }

        /// <summary>
        /// The GetRoomAmenities
        /// </summary>
        /// <param name="roomId">The roomId<see cref="Guid"/></param>
        /// <returns>The <see cref="List{RoomAmenity}"/></returns>
        private List<RoomAmenity> GetRoomAmenities(Guid roomId)
        {
            return _context.RoomAmenities.Include(x => x.Amenity).Include(x => x.Room).Where(x => x.RoomId == roomId).ToList();
        }

        /// <summary>
        /// The ValidateBooking
        /// </summary>
        /// <param name="booking">The booking<see cref="Booking"/></param>
        private void ValidateBooking(Booking booking)
        {
            TravelDatesQuerySpecification travelDatesQuery =
                new TravelDatesQuerySpecification(new DateTimeRange(booking.Arrival, booking.Departure));

            try
            {
                List<RoomBooking> bookings = new List<RoomBooking>();

                bookings = _context.RoomBookings
                 .Where(x => x.Booking.BookerId == booking.BookerId)
                 .Where(travelDatesQuery.Criteria)
                 .ToList();

                if (bookings.Count > 0)
                {
                    throw new TravelDatesOverlapException("Travel dates overlap with an existing booking.");
                }
            }
            catch (TravelDatesOverlapException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
