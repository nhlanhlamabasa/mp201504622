namespace HotelSystem.SharedKernel.ViewModels
{
    public class InvoiceDownload
    {
        public BookingViewModel booking { get; set; }

        public HotelViewModel hotel { get; set; }

        public InvoiceViewModel invoice { get; set; }

        public PaymentViewModel payment { get; set; }

        public string Email { get; set; }

        public FullName Name { get; set; }
    }
}
