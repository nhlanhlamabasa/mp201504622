namespace Booking.API.Models
{
#pragma warning disable
    using AutoMapper;
    using Booking.API.Entities;
    using HotelSystem.SharedKernel;
    using HotelSystem.SharedKernel.ViewModels;
    using System;
    using System.Collections.Generic;


    public class HotelMapperProfile : Profile
    {
        public HotelMapperProfile()
        {
            CreateMap<Hotel, HotelViewModel>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.CreationTime, opt => opt.MapFrom(src => src.CreationTime))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ManagerId, opt => opt.MapFrom(src => src.ManagerId))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
                .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.Province))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
                .ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.Rooms))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street))
                .ForMember(dest => dest.NumberOfBookings, opt => opt.MapFrom(src => src.NumberOfBookings))
                .ForMember(dest => dest.NumberOfRooms, opt => opt.MapFrom(src => src.NumberOfRooms))
                .ForMember(dest => dest.ImageName, opt => opt.MapFrom(src => src.ImageName))
                .ForMember(dest => dest.ImageID, opt => opt.MapFrom(src => src.ImageID))
                .ReverseMap();

            CreateMap<Room, RoomViewModel>()
                .ForMember(dest => dest.Amenities, opt => opt.MapFrom(src => src.Amenities))
                .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.Capacity))
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Cost))
                .ForMember(dest => dest.CreationTime, opt => opt.MapFrom(src => src.CreationTime))
                .ForMember(dest => dest.HotelId, opt => opt.MapFrom(src => src.HotelId))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.RoomBookings, opt => opt.MapFrom(src => src.RoomBookings))
                .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.RoomNumber))
                .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.RoomType))
                .ForMember(dest => dest.Amenities, opt => opt.MapFrom(src => src.Amenities))
                .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.HotelName))
                .ForMember(dest => dest.ImageName, opt => opt.MapFrom(src => src.ImageName))
                .ForMember(dest => dest.ImageID, opt => opt.MapFrom(src => src.ImageID))
                .ReverseMap();

            CreateMap<RoomAmenity, RoomAmenityViewModel>()
                .ForMember(dest => dest.Room, opt => opt.MapFrom(src => src.Room))
                .ForMember(dest => dest.Amenity, opt => opt.MapFrom(src => src.Amenity))
                .ForMember(dest => dest.AmenityId, opt => opt.MapFrom(src => src.AmenityId))
                .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.RoomId))
                .ReverseMap();

            CreateMap<Booking, BookingViewModel>()
                .ForMember(dest => dest.Arrival, opt => opt.MapFrom(src => src.Arrival))
                .ForMember(dest => dest.BookerId, opt => opt.MapFrom(src => src.BookerId))
                .ForMember(dest => dest.CreationTime, opt => opt.MapFrom(src => src.CreationTime))
                .ForMember(dest => dest.Departure, opt => opt.MapFrom(src => src.Departure))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Invoice, opt => opt.MapFrom(src => src.Invoice))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.NumberOfGuests, opt => opt.MapFrom(src => src.NumberOfGuests))
                .ForMember(dest => dest.RoomBookings, opt => opt.MapFrom(src => src.RoomBookings))
                .ForMember(dest => dest.SelectedRooms, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.HotelName))
                .ForMember(dest => dest.NumSelectedRooms, opt => opt.Ignore())
                .ForMember(dest => dest.Payment, opt => opt.Ignore())
                .ForMember(dest => dest.HotelId, opt => opt.MapFrom(src => src.HotelId))
                .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.HotelName))
                .ForMember(dest => dest.CheckInPerson, opt => opt.MapFrom(src => src.CheckInPerson))
                .ForMember(dest => dest.CheckOutPerson, opt => opt.MapFrom(src => src.CheckOutPerson))
                .ForMember(dest => dest.CheckInCheckList, opt => opt.MapFrom(src => src.CheckInCheckList))
                .ForMember(dest => dest.Complaints, opt => opt.MapFrom(src => src.Complaints))
                .ForMember(dest => dest.NewComplaint, opt => opt.MapFrom(src => src.Complaints))
                .ReverseMap();

            CreateMap<Complaint, ComplaintViewModel>().ReverseMap();

            CreateMap<CheckInCheckList, CheckInCheckListViewModel>().ReverseMap();

            CreateMap<RoomBooking, RoomBookingViewModel>()
                .ForMember(dest => dest.Booking, opt => opt.MapFrom(src => src.Booking))
                .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.BookingId))
                .ForMember(dest => dest.CreationTime, opt => opt.MapFrom(src => src.CreationTime))
                .ForMember(dest => dest.End, opt => opt.MapFrom(src => src.Departure))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.Room, opt => opt.MapFrom(src => src.Room))
                .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.RoomId))
                .ForMember(dest => dest.Start, opt => opt.MapFrom(src => src.Arrival))
                .ReverseMap();

            CreateMap<Amenity, AmenityViewModel>()
                .ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.Rooms))
                .ForMember(dest => dest.CreationTime, opt => opt.MapFrom(src => src.CreationTime))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.ImageName, opt => opt.MapFrom(src => src.ImageName))
                .ForMember(dest => dest.ImageID, opt => opt.MapFrom(src => src.ImageID))
                .ReverseMap();

            CreateMap<Invoice, InvoiceViewModel>()
                .ForMember(dest => dest.AppUser_BookerId, opt => opt.MapFrom(src => src.AppUser_BookerId))
                .ForMember(dest => dest.CreationTime, opt => opt.MapFrom(src => src.CreationTime))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Inv_Date, opt => opt.MapFrom(src => src.Inv_Date))
                .ForMember(dest => dest.Lines, opt => opt.MapFrom(src => src.Lines))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.BookingId))
                .ReverseMap();

            CreateMap<Line, LineViewModel>()
                .ForMember(dest => dest.CreationTime, opt => opt.MapFrom(src => src.CreationTime))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Invoice, opt => opt.MapFrom(src => src.Invoice))
                .ForMember(dest => dest.InvoiceId, opt => opt.MapFrom(src => src.InvoiceId))
                .ForMember(dest => dest.Line_Price, opt => opt.MapFrom(src => src.Line_Price))
                .ForMember(dest => dest.Line_Units, opt => opt.MapFrom(src => src.Line_Units))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.Room_Type, opt => opt.MapFrom(src => src.Room_Type))
                .ReverseMap();

            CreateMap<Payment, PaymentViewModel>()
                .ForMember(dest => dest.AmountPaid, opt => opt.MapFrom(src => src.AmountPaid))
                .ForMember(dest => dest.CreationTime, opt => opt.MapFrom(src => src.CreationTime))
                .ForMember(dest => dest.Invoice, opt => opt.MapFrom(src => src.Invoice))
                .ForMember(dest => dest.InvoiceId, opt => opt.MapFrom(src => src.InvoiceId))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.Payment_Method, opt => opt.MapFrom(src => src.Payment_Method))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.Ignore())
                .ForMember(dest => dest.LastName, opt => opt.Ignore())
                .ForMember(dest => dest.CardNumber, opt => opt.Ignore())
                .ForMember(dest => dest.Expiration, opt => opt.Ignore())
                .ForMember(dest => dest.CVV, opt => opt.Ignore())
                .ForMember(dest => dest.PostalCode, opt => opt.Ignore())
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ReverseMap();

            CreateMissingTypeMaps = false;
        }
    }
}
