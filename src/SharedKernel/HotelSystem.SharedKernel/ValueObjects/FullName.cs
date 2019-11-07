namespace HotelSystem.SharedKernel
{
    /// <summary>
    /// Defines the <see cref="FullName" />
    /// </summary>
    public class FullName : ValueObject<FullName>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FullName"/> class.
        /// </summary>
        /// <param name="fullName">The fullName<see cref="FullName"/></param>
        public FullName(FullName fullName) : this(fullName.FirstName, fullName.LastName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FullName"/> class.
        /// </summary>
        /// <param name="firstName">The firstName<see cref="string"/></param>
        /// <param name="lastName">The lastName<see cref="string"/></param>
        public FullName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="FullName"/> class from being created.
        /// </summary>
        private FullName()
        {
        }

        /// <summary>
        /// Gets the FirstName
        /// </summary>
        public string FirstName { get; private set; }

        /// <summary>
        /// Gets the LastName
        /// </summary>
        public string LastName { get; private set; }

        /// <summary>
        /// The WithChangedFirstName
        /// </summary>
        /// <param name="firstName">The firstName<see cref="string"/></param>
        /// <returns>The <see cref="FullName"/></returns>
        public FullName WithChangedFirstName(string firstName)
        {
            return new FullName(firstName, this.LastName);
        }

        /// <summary>
        /// The WithChangedLastName
        /// </summary>
        /// <param name="lastName">The lastName<see cref="string"/></param>
        /// <returns>The <see cref="FullName"/></returns>
        public FullName WithChangedLastName(string lastName)
        {
            return new FullName(this.FirstName, lastName);
        }

        override
        public string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
}
