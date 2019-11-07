using HotelSystem.SharedKernel;
using IdentityModel;
using IdentityServer.API.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.API.Data
{
    public class DataContextSeedData
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataContextSeedData(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //Seed database
        public async Task EnsureSeedData()
        {
            try
            {
                await CreateUserRoles();

                if (!_context.ApplicationUsers.Any())
                {
                    await AddGuests();
                    await AddAdmin();
                    await AddManagers();
                    await AddFrontDesk();
                }

               
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        private async Task CreateUserRoles()
        {
            //Adding Admin Role
            var roleCheck = await _roleManager.RoleExistsAsync(UserRoles.ADMIN);
            if (!roleCheck)
            {
                //create the roles and seed them to the database
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.ADMIN));
            }

            //Adding Guest Role
            var guestCheck = await _roleManager.RoleExistsAsync(UserRoles.GUEST);
            if (!guestCheck)
            {
                //create guest role
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.GUEST));
            }

            //Adding manager Role
            var managerCheck = await _roleManager.RoleExistsAsync(UserRoles.MANAGER);
            if (!managerCheck)
            {
                //create manager role
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.MANAGER));
            }

            //Adding Front desk role
            var frontDesk = await _roleManager.RoleExistsAsync(UserRoles.FRONTDESK);
            if (!frontDesk)
            {
                //create font desk role
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.FRONTDESK));
            }
        }

        private async Task AddAdmin()
        {
            //UserName - Password

            //demo - demouser
            await AddUser(Guid.NewGuid().ToString(), "demo@mail.com", "demo", "0604102008",
                new FullName("demo", "demo"),
                new ContactDetails("20207"),
                new Address("South Africa", "Johannesburg", "Guateng", "16 Henschel Street"), "demouser", UserRoles.ADMIN);
        }

        private async Task AddManagers()
        {
            await AddUser("4caa4efb-235e-4fc1-b3a5-a59fb7934a8e", "winston@mail.com", "winston", "0604102009",
                new FullName("Winston", "Wallace"),
                new ContactDetails("20203"),
                new Address("South Africa", "Johannesburg", "Guateng", "53 Clydesdale Street"), "winstonwallace", UserRoles.MANAGER);
        }

        private async Task AddFrontDesk()
        {
            try
            {
                string fileName = @"..\..\..\..\data\FrontDesk.txt";
                FileStream fileReader = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(fileReader);
                string line;
                string[] attributes = reader.ReadLine().Split("\t");
                string[] requiredAtributes = {"Id", "FirstName", "LastName", "PhoneNumber", "Email", "PostalCode", "Country",
                "City", "Town", "Street", "Password"};

                if (attributes.Length == requiredAtributes.Length)
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] user = line.Split("\t");
                        string Id = user[0];
                        string FirstName = user[1];
                        string LastName = user[2];
                        string PhoneNumber = user[3];
                        string Email = user[4];
                        string PostalCode = user[5];
                        string Country = user[6];
                        string City = user[7];
                        string Province = user[8];
                        string Street = user[9];
                        string Password = user[10];

                        await AddUser(Id, Email, FirstName, PhoneNumber,
                            new FullName(FirstName, LastName),
                            new ContactDetails(PostalCode),
                            new Address(Country, City, Province, Street), Password, UserRoles.FRONTDESK);
                    }
                }
                else
                {
                    fileReader.Close();
                    fileReader.Dispose();
                    reader.Close();
                    reader.Dispose();
                    throw new Exception($"{requiredAtributes.Length} attributes are required but {attributes.Length}" +
                        $"attributes were specified in the file: {fileName}");
                }

            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task AddGuests()
        {
            try
            {
                string fileName = @"..\..\..\..\data\Guests.txt";
                FileStream fileReader = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(fileReader);
                string line;
                string[] attributes = reader.ReadLine().Split("\t");
                string[] requiredAtributes = {"Id", "FirstName", "LastName", "UserName", "Email", "PhoneNumber", "PostalCode", "Country",
                "City", "Province", "Street", "Password"};

                if (attributes.Length == requiredAtributes.Length)
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] user = line.Split("\t");
                        string Id = user[0];
                        string FirstName = user[1];
                        string LastName = user[2];
                        string UserName = user[3];
                        string Email = user[4];
                        string PhoneNumber = user[5];
                        string PostalCode = user[6];
                        string Country = user[7];
                        string City = user[8];
                        string Province = user[9];
                        string Street = user[10];
                        string Password = user[11];

                        await AddUser(Id, Email, UserName, PhoneNumber,
                            new FullName(FirstName, LastName),
                            new ContactDetails(PostalCode),
                            new Address(Country, City, Province, Street), Password, UserRoles.GUEST);
                    }
                }
                else
                {
                    fileReader.Close();
                    fileReader.Dispose();
                    reader.Close();
                    reader.Dispose();
                    throw new Exception($"{requiredAtributes.Length} attributes are required but {attributes.Length}" +
                        $"attributes were specified in the file: {fileName}");
                }

            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task AddUser(string id, string email, string username, string phoneNumber,
            FullName fullName, ContactDetails contactDetails, Address address, string password, string role)
        {
            var user = new ApplicationUser(id, email, username, phoneNumber, fullName, contactDetails, address);

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role);
                await _userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.Role, role));
                await _userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.Id, user.Id));
                await _userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.Name, user.FirstName));
                await _userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.FamilyName, user.LastName));
                await _userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.Email, user.Email));
            }
        }
    }
}
