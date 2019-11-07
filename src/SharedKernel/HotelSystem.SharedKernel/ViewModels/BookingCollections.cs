using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HotelSystem.SharedKernel.ViewModels
{
    public class BookingCollections
    {
        public BookingCollections(ICollection<RoomViewModel> roomViewModels, ICollection<LineViewModel> lineViewModels)
        {
            RoomViewModels = new List<RoomViewModel>(roomViewModels);
            LineViewModels = new List<LineViewModel>(lineViewModels);
        }

        [Required]
        public ICollection<RoomViewModel> RoomViewModels { get; set; }

        [Required]
        public ICollection<LineViewModel> LineViewModels { get; set; }
    }
}
