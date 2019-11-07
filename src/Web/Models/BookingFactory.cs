namespace Web.Models
{
    using HotelSystem.SharedKernel.ViewModels;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Web.Extensions;

    public static class BookingFactory
    {
        public static BookingViewModel GetBookingViewModel(HttpContext context)
        {
            BookingViewModel bookingViewModel = new BookingViewModel()
            {
                Arrival = context.Session.Get<DateTime>(BookingConstants.ARRIVAL_DATE),
                Departure = context.Session.Get<DateTime>(BookingConstants.DEPARTURE_DATE),
                NumberOfGuests = context.Session.Get<int>(BookingConstants.NUM_GUESTS),
                HotelId = context.Session.Get<Guid>(BookingConstants.HOTEL_ID),
                HotelName = context.Session.Get<string>(BookingConstants.HOTEL_NAME),
                SelectedRooms = context.Session.Get<List<RoomViewModel>>(BookingConstants.SELECTED_ROOMS),
                NumSelectedRooms = context.Session.Get<int>(BookingConstants.NUM_SELECTED_ROOMS),
                Status = context.Session.Get<string>(BookingConstants.BOOKING_STATUS),
                Id = context.Session.Get<Guid>(BookingConstants.BOOKING_ID)
            };

            return bookingViewModel;
        }

        public static void ClearSelection(HttpContext context)
        {
            ICollection<RoomViewModel> SelectedRooms = new List<RoomViewModel>();
            context.Session.Set(BookingConstants.NUM_SELECTED_ROOMS, 0);
            context.Session.Set(BookingConstants.SELECTED_ROOMS, SelectedRooms);
        }



        public static int NumberOfBeds(HttpContext context)
        {
            try
            {
                BookingViewModel bookingViewModel = GetBookingViewModel(context);
                return bookingViewModel.SelectedRooms.Select(x => x.Capacity).Sum();
            }
            catch(ArgumentNullException)
            {
                throw new Exception("Please select rooms");
            }
        }

        public static void RemoveRoom(HttpContext context, Guid Id)
        {
            ICollection<RoomViewModel> SelectedRooms = context.Session.Get<ICollection<RoomViewModel>>(BookingConstants.SELECTED_ROOMS);
            try
            {
                RoomViewModel room = SelectedRooms.Single(x => x.Id == Id);
                SelectedRooms.Remove(room);
                context.Session.Set(BookingConstants.SELECTED_ROOMS, SelectedRooms);
                context.Session.Set(BookingConstants.NUM_SELECTED_ROOMS, SelectedRooms.Count);
            }
            catch(Exception)
            {
                throw new Exception("No rooms selected.");
            }
        }

        public static void StoreBookingViewModel(BookingViewModel model, HttpContext context)
        {
            if(model.SelectedRooms != null)
            {
                context.Session.Set(BookingConstants.ARRIVAL_DATE, model.Arrival);
                context.Session.Set(BookingConstants.DEPARTURE_DATE, model.Departure);
                context.Session.Set(BookingConstants.HOTEL_ID, model.HotelId);
                context.Session.Set(BookingConstants.HOTEL_NAME, model.HotelName);
                context.Session.Set(BookingConstants.NUM_GUESTS, model.NumberOfGuests);
                context.Session.Set(BookingConstants.NUM_SELECTED_ROOMS, model.SelectedRooms.Count);
                context.Session.Set(BookingConstants.BOOKING_STATUS, model.Status);
                context.Session.Set(BookingConstants.BOOKING_ID, model.Id);
                context.Session.Set(BookingConstants.SELECTED_ROOMS, model.SelectedRooms);
            }
            else
            {
                context.Session.Set(BookingConstants.ARRIVAL_DATE, model.Arrival);
                context.Session.Set(BookingConstants.DEPARTURE_DATE, model.Departure);
                context.Session.Set(BookingConstants.HOTEL_ID, model.HotelId);
                context.Session.Set(BookingConstants.HOTEL_NAME, model.HotelName);
                context.Session.Set(BookingConstants.NUM_GUESTS, model.NumberOfGuests);
                context.Session.Set(BookingConstants.NUM_SELECTED_ROOMS, model.NumSelectedRooms);
                context.Session.Set(BookingConstants.BOOKING_STATUS, model.Status);
                context.Session.Set(BookingConstants.BOOKING_ID, model.Id);
            }
            
        }

        public static void ReserveRoom(RoomViewModel roomViewModel, HttpContext context)
        {
            ICollection<RoomViewModel> SelectedRooms = context.Session.Get<ICollection<RoomViewModel>>(BookingConstants.SELECTED_ROOMS);
            bool isDuplicate = false;
            if(SelectedRooms != null)
            {
                foreach(RoomViewModel room in SelectedRooms)
                {
                    if(room.Id == roomViewModel.Id)
                    {
                        isDuplicate = true;
                    }
                }
                if(!isDuplicate)
                {
                    SelectedRooms.Add(roomViewModel);
                    context.Session.Set(BookingConstants.SELECTED_ROOMS, SelectedRooms);
                    context.Session.Set(BookingConstants.NUM_SELECTED_ROOMS, SelectedRooms.Count);
                }
                else
                {
                    throw new Exception("You have already selected the room.");
                }
                
            }
            else
            {
                SelectedRooms = new List<RoomViewModel>();
                SelectedRooms.Add(roomViewModel);
                context.Session.Set(BookingConstants.SELECTED_ROOMS, SelectedRooms);
                context.Session.Set(BookingConstants.NUM_SELECTED_ROOMS, SelectedRooms.Count);
            }
        
        }

        public static BookingViewModel GenerateInvoice(BookingViewModel bookingViewModel)
        {
            InvoiceViewModel invoice = new InvoiceViewModel()
            {   
                Id = Guid.NewGuid(),
                AppUser_BookerId = bookingViewModel.BookerId,
                BookingId = bookingViewModel.Id,
                Inv_Date = DateTime.UtcNow
            };

            Dictionary<string, int> roomtype_frequency = bookingViewModel.SelectedRooms.GroupBy(x => x.RoomType).ToDictionary(x => x.Key, x => x.Count());

            Dictionary<string, decimal> roomtype_prices = new Dictionary<string, decimal>();

            foreach(KeyValuePair<string, int> entry in roomtype_frequency)
            {
                decimal price = bookingViewModel.SelectedRooms.Where(x => x.RoomType == entry.Key).FirstOrDefault().Cost;
                roomtype_prices.Add(entry.Key, price);
            }

            List<LineViewModel> lines = new List<LineViewModel>();

            foreach(KeyValuePair<string, int> entry in roomtype_frequency)
            {
                lines.Add(new LineViewModel()
                {
                    Id = Guid.NewGuid(),
                    InvoiceId = invoice.Id,
                    Line_Price = roomtype_prices[entry.Key],
                    Room_Type = entry.Key,
                    Line_Units = entry.Value
                });
            }

            invoice.Lines = new List<LineViewModel>(lines);
            bookingViewModel.Invoice = invoice;
            return bookingViewModel;
        }

        public static void ClearAll(HttpContext context)
        {
            context.Session.Remove(BookingConstants.ARRIVAL_DATE);
            context.Session.Remove(BookingConstants.DEPARTURE_DATE);
            context.Session.Remove(BookingConstants.HOTEL_ID);
            context.Session.Remove(BookingConstants.HOTEL_NAME);
            context.Session.Remove(BookingConstants.NUM_GUESTS);
            context.Session.Remove(BookingConstants.SELECTED_ROOMS);
            context.Session.Remove(BookingConstants.NUM_SELECTED_ROOMS);
            context.Session.Remove(BookingConstants.BOOKING_STATUS);
            context.Session.Remove(BookingConstants.BOOKING_ID);
            context.Session.Remove(BookingConstants.AVAILABLE_ROOMS);
        }

    }


    internal static class BookingConstants
    {
        public const string ARRIVAL_DATE = "ArrivalDate";
        public const string AVAILABLE_ROOMS = "AvailableRooms";
        public const string DEPARTURE_DATE = "DepartureDate";
        public const string HOTEL_ID = "HotelId";
        public const string HOTEL_NAME = "HotelName";
        public const string NUM_GUESTS = "NumberOfGuests";
        public const string SELECTED_ROOMS = "SelectedRooms";
        public const string NUM_SELECTED_ROOMS = "NumberOfSelectedRooms";
        public const string BOOKING_STATUS = "BookingStatus";
        public const string BOOKING_ID = "BookingId";
    }
    
}