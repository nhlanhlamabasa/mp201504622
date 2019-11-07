namespace Booking.API.Extensions
{
    using Booking.API.Data;
    using Booking.API.Entities;
    using HotelSystem.SharedKernel;
#pragma warning disable
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Defines the <see cref="ModelBuilderExtensions" />
    /// </summary>
    public static class ModelBuilderExtensions
    {

        /// <summary>
        /// The Seed
        /// </summary>
        /// <param name="modelBuilder">The modelBuilder<see cref="ModelBuilder"/></param>
        public static void Seed(this ModelBuilder modelBuilder)
        {
            List<Hotel> Hotels = AddHotels();
            List<Room> Rooms = AddRooms(Hotels);
            List<Amenity> Amenities = SeedData.Amenities;
            modelBuilder.Entity<Hotel>().HasData(Hotels.ToArray());
            modelBuilder.Entity<Room>().HasData(Rooms.ToArray());
            modelBuilder.Entity<Amenity>().HasData(Amenities.ToArray());
            modelBuilder.Entity<RoomTypeEnum>().HasData(SeedData.RoomTypes.ToArray());
        }

        private static List<Hotel> AddHotels()
        {
            List<Hotel> hotels = new List<Hotel>();

            try
            {
                string fileName = @"..\..\..\..\data\Hotels.txt";

                FileStream fileReader = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(fileReader);
                string line;
                string[] attributes = reader.ReadLine().Split("\t");
                string[] requiredAtributes = { "Name", "NumberOfFloors", "ImageName", "ImageID", "Rating", "PostalCode", "Country", "City", "Province", "Street", "Id", "ManagerId" };

                if (attributes.Length == requiredAtributes.Length)
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] hotel = line.Split("\t");
                        string Name = hotel[0];
                        int.TryParse(hotel[1], out int NumberOfFloors);
                        string ImageName = hotel[2];
                        string ImageID = hotel[3];
                        Ratings Rating = (Ratings)Enum.Parse(typeof(Ratings), hotel[4]);
                        string PostalCode = hotel[5];
                        string Country = hotel[6];
                        string City = hotel[7];
                        string Province = hotel[8];
                        string Street = hotel[9];
                        Guid Id = Guid.Parse(hotel[10]);
                        Guid ManagerId = Guid.Parse(hotel[11]);

                        Hotel ehotel = new Hotel(Id, Name, Rating, ImageName, ImageID, new ContactDetails(PostalCode), new Address(Country, City, Province, Street))
                        {
                            ManagerId = ManagerId
                        };
                        hotels.Add(ehotel);
                    }

                    reader.Close();

                    return hotels;
                }

                throw new Exception($"{requiredAtributes.Length} attributes are required but {attributes.Length} " +
                    $"attributes were specified in the file: {fileName}");

            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private static List<Room> AddRooms(List<Hotel> hotels)
        {
            List<Room> rooms = new List<Room>();
            foreach (Hotel hotel in hotels)
            {
                rooms.AddRange(GenerateRooms(hotel));
            }
            return rooms;
        }


        private static List<Room> GenerateRooms(Hotel hotel)
        {
            List<Room> Rooms = new List<Room>();
            RoomTypeEnum[] RoomTypes = SeedData.RoomTypes.ToArray();
            Random random = new Random();
            for (int roomNumber = 0; roomNumber < 100; roomNumber++)
            {
                var type = RoomTypes[random.Next(0, RoomTypes.Length)];
                Room room = new Room(Guid.NewGuid(), roomNumber, type, hotel);
                Rooms.Add(room);
            }
            return Rooms;
        }
    }
}
