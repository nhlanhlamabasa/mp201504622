namespace HotelSystem.SharedKernel
{
    /// <summary>
    /// Defines the <see cref="ContactDetails" />
    /// </summary>
    public class ContactDetails : ValueObject<ContactDetails>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactDetails"/> class.
        /// </summary>
        /// <param name="PostalCode">The PostalCode<see cref="string"/></param>
        public ContactDetails(string PostalCode)
        {
            this.PostalCode = PostalCode;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="ContactDetails"/> class from being created.
        /// </summary>
        private ContactDetails()
        {
        }

        /// <summary>
        /// Gets the PostalCode
        /// </summary>
        public string PostalCode { get; private set; }
    }
}
