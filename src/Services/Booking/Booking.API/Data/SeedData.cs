namespace Booking.API.Data
{
#pragma warning disable

    using Booking.API.Entities;
    using System.Collections.Generic;
    using System;

    public static class SeedData
    {
        public static List<RoomTypeEnum> RoomTypes = new List<RoomTypeEnum>()
        {
            new RoomTypeEnum(1, 1, 300, "Single", "single.jpg", "1541762783"),
            new RoomTypeEnum(2, 2, 500, "Double", "double.jpg", "1541701706"),
            new RoomTypeEnum(3, 3, 700, "Triple", "triple.jpg", "1541701705")
        };

        public static List<Amenity> Amenities = new List<Amenity>()
        {
            new Amenity(Guid.NewGuid(),"Air Conditioning", "air_conditioning.jpg", "1541701638"),
            new Amenity(Guid.NewGuid(), "Adapter Plugs", "adapter_plugs.jpg", "1541701638"),
            new Amenity(Guid.NewGuid(), "Bathroom Amenities", "bathroom_amenities.jpg", "1541712229"),
            new Amenity(Guid.NewGuid(), "Bath Tub", "bathtub.jpg", "1541711936"),
            new Amenity(Guid.NewGuid(), "Shower Cap", "shower_cap.jpg", "1541712282"),
            new Amenity(Guid.NewGuid(), "Coffee Maker", "coffee_maker.jpg", "1541764309"),
            new Amenity(Guid.NewGuid(), "HD TV", "hd_tv.jpg", "1541712134"),
            new Amenity(Guid.NewGuid(), "Desk", "desk.jpg", "1541701639"),
            new Amenity(Guid.NewGuid(), "Hairdryer", "hairdryer.jpg", "1541764220"),
            new Amenity(Guid.NewGuid(), "Remote Control TV", "remote.jpg", "1541701639"),
            new Amenity(Guid.NewGuid(), "Shower", "shower.jpg", "1541712444"),
            new Amenity(Guid.NewGuid(), "Safe", "safe.jpg", "1541701639"),
            new Amenity(Guid.NewGuid(), "DSTV", "dstv.jpg", "1541712566"),
            new Amenity(Guid.NewGuid(), "Telephone", "telephone.jpg", "1541701638"),
            new Amenity(Guid.NewGuid(), "Wireless Connect", "wireless.jpg", "1541712818")
        };
    }
}
