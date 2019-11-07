namespace HotelSystem.SharedKernel
{
    /// <summary>
    /// Defines the Ratings
    /// </summary>
    public enum Ratings
    {
        /// <summary>
        /// Defines the One_Star
        /// </summary>
        One_Star,
        /// <summary>
        /// Defines the Two_Star
        /// </summary>
        Two_Star,
        /// <summary>
        /// Defines the Three_Star
        /// </summary>
        Three_Star,
        /// <summary>
        /// Defines the Four_Star
        /// </summary>
        Four_Star,
        /// <summary>
        /// Defines the Five_Star
        /// </summary>
        Five_Star
    }

    public enum BookingStatus
    {
        Paid = 1,
        Cancelled = 2,
        CheckedOut = 3,
        CheckedIn = 4,
        AwaitingPayment = 5
    }

    public enum PaymentMethod
    {
        CreditCard = 1
    }

    public enum ComplaintType
    {
        Service =1,
        Cleanliness =2,
        Noise = 3,
        Amenities = 4
    }

    public enum SatisfactionLevel
    {
        Poor = 1,
        Decent = 2,
        Good = 3,
        Superb = 4,
        Excellent = 5
    }
}
