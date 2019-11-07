using HotelSystem.SharedKernel;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string PostalCode { get; private set; }

        public string Country { get; private set; }

        public string City { get; private set; }

        public string Province { get; private set; }

        public string Street { get; private set; }

        public ApplicationUser() { }

        public ApplicationUser(string email, string username, string phoneNumber, FullName fullName, ContactDetails contactDetails, Address address)
        {
            Email = email;
            UserName = username;
            PhoneNumber = phoneNumber;
            var FullName = new FullName(fullName.FirstName, fullName.LastName);
            FirstName = FullName.FirstName;
            LastName = FullName.LastName;
            var ContactDetails = new ContactDetails(contactDetails.PostalCode);
            PostalCode = ContactDetails.PostalCode;
            var Address = new Address(address.Country, address.City, address.Province, address.Street);
            Country = Address.Country;
            City = Address.City;
            Province = Address.Province;
            Street = Address.Street;
        }

        public ApplicationUser(string id, string email, string username, string phoneNumber, FullName fullName, ContactDetails contactDetails, Address address)
        {
            Id = id;
            Email = email;
            UserName = username;
            PhoneNumber = phoneNumber;
            var FullName = new FullName(fullName.FirstName, fullName.LastName);
            FirstName = FullName.FirstName;
            LastName = FullName.LastName;
            var ContactDetails = new ContactDetails(contactDetails.PostalCode);
            PostalCode = ContactDetails.PostalCode;
            var Address = new Address(address.Country, address.City, address.Province, address.Street);
            Country = Address.Country;
            City = Address.City;
            Province = Address.Province;
            Street = Address.Street;
        }


        public override string ToString()
        {
            return
                $"Id:\t{Id}\r\n" +
                $"Email:\t{Email}\r\n" +
                $"User Name:\t{UserName}\r\n" +
                $"First Name:\t{FirstName}\r\n" +
                $"Last Name:\t{LastName}\r\n" +
                $"Postal Code:\t{PostalCode}\r\n" +
                $"country:\t{Country}\r\n" +
                $"City:\t{City}\r\n" +
                $"Province:\t{Province}\r\n" +
                $"Street:\t{Street}\r\n";
        }
    }
}
