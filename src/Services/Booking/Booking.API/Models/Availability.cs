namespace Booking.API.Models
{
#pragma warning disable
    using Booking.API.Entities;
    using Booking.API.Interfaces;
    using Booking.API.Specifications.RoomSpecifications;
    using HotelSystem.SharedKernel;
    using HotelSystem.SharedKernel.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Availability
    {
        public static void SuitableRoomCombinations(List<Room> AvailableRooms, int NumberOfGuests, ref List<Room> SuitableRooms)
        {
            Combinations(AvailableRooms, NumberOfGuests, new List<Room>(), ref SuitableRooms);
        }

        private static void Combinations(List<Room> AvailableRooms, int NumberOfGuests, List<Room> Partial, ref List<Room> SuitableRooms)
        {
            int sum = 0;

            foreach (Room room in Partial)
                sum += room.Capacity;
            
            if (sum == NumberOfGuests)
                SuitableRooms.AddRange(Partial);

            if (sum >= NumberOfGuests)
                return;

            for (int i = 0; i < AvailableRooms.Count; i++)
            {
                List<Room> Remaining = new List<Room>();

                Room room = AvailableRooms[i];

                for (int j = i + 1; j < AvailableRooms.Count; j++)
                    Remaining.Add(AvailableRooms[j]);
                
                List<Room> Partial_rec = new List<Room>(Partial);

                Partial_rec.Add(room); 

                Combinations(Remaining, NumberOfGuests, Partial_rec, ref SuitableRooms);
            }
        }
    }
}
