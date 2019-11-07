namespace HotelSystem.SharedKernel
{
    /// <summary>
    /// Defines the <see cref="Address" />
    /// </summary>
    public class Address : ValueObject<Address>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Address"/> class.
        /// </summary>
        /// <param name="Country">The Country<see cref="string"/></param>
        /// <param name="City">The City<see cref="string"/></param>
        /// <param name="Province">The Province<see cref="string"/></param>
        /// <param name="Street">The Street<see cref="string"/></param>
        public Address(string Country, string City, string Province, string Street)
        {
            this.City = City;
            this.Country = Country;
            this.Street = Street;
            this.Province = Province;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="Address"/> class from being created.
        /// </summary>
        private Address()
        {
        }

        /// <summary>
        /// Gets the City
        /// </summary>
        public string City { get; private set; }

        /// <summary>
        /// Gets the Country
        /// </summary>
        public string Country { get; private set; }

        /// <summary>
        /// Gets the Province
        /// </summary>
        public string Province { get; private set; }

        /// <summary>
        /// Gets the Street
        /// </summary>
        public string Street { get; private set; }
    }
}
