namespace Booking.API
{
#pragma warning disable
    using Booking.API.Data;
    using Booking.API.Entities;
    using Booking.API.Exceptions;
    using Booking.API.Interfaces;
    using Booking.API.Models;
    using Booking.API.Services;
    using HotelSystem.SharedKernel.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using NJsonSchema;
    using NSwag;
    using NSwag.AspNetCore;
    using NSwag.SwaggerGeneration.Processors.Security;
    using Swashbuckle.AspNetCore.Swagger;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using static Booking.API.Models.Clients;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env, BookingContext context)
        {
            SeedAmenties(context);

            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionMiddleware();
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Booking API V1");
                options.RoutePrefix = string.Empty;
                options.OAuthClientId("swaggerui");
                options.OAuthAppName("Swagger UI");
            });
            app.UseSwaggerUi(typeof(Startup).GetTypeInfo().Assembly, settings =>
            {
                settings.GeneratorSettings.DefaultPropertyNameHandling = PropertyNameHandling.CamelCase;
                settings.GeneratorSettings.DocumentProcessors.Add(
                new SecurityDefinitionAppender("oauth2", new SwaggerSecurityScheme
                {
                    Type = SwaggerSecuritySchemeType.OAuth2,
                    Flow = SwaggerOAuth2Flow.Implicit,
                    AuthorizationUrl = "https://localhost:5001/connect/authorize",
                    Scopes = new Dictionary<string, string> { { "booking_api", "Booking API" } }
                }));
                settings.GeneratorSettings.OperationProcessors.Add(new OperationSecurityScopeProcessor("oauth2"));
                settings.OAuth2Client = new OAuth2ClientSettings
                {
                    ClientId = "swaggerui",
                    AppName = "Swagger UI"
                };
            });

            app.UseCors("default");

            app.UseAuthentication();

            app.UseMvc();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            services.AddAuthentication("Bearer")
            .AddIdentityServerAuthentication(options =>
            {
                options.Authority = "https://localhost:5001";
                options.RequireHttpsMetadata = true;

                options.ApiName = Configuration["Api:name"];
            });

            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins("https://localhost:5003")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddDbContext<BookingContext>(options =>
                options.UseSqlServer(Configuration["DefaultConnection"], sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                    //Configuring Connection Resiliency
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                }));

            services.AddApiVersioning();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Booking API",
                    Version = "v1",
                    Description = @"REST services for Blue Orb Hotels.
## Services Provided ##
* Booking
* Managing Hotels
* Payment",
                    Contact = new Contact
                    {
                        Name = "Blue Orb Hotels",
                        Email = "support@bluehotels.com",
                        Url = "https://localhost:5005/"
                    },
                    License = new License
                    {

                    },
                    TermsOfService = "None"
                });

                options.IncludeXmlComments(@".\Booking.API.xml");

                // Handle OAuth
                options.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "implicit",
                    AuthorizationUrl = "https://localhost:5001/connect/authorize",
                    TokenUrl = "https://localhost:5001/connect/token",
                    Scopes = new Dictionary<string, string> { { "booking_api", "Booking API" } }
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();

                services.Configure<SMTPEmailConfig>(Configuration.GetSection("SMTP"));
                services.Configure<POP3EmailConfig>(Configuration.GetSection("POP3"));
            });

            services.AddScoped<IHotelRepository, HotelRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IBookerRepository, BookerRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IManagerRepository, ManagerRepository>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory =>
            {
                var actionContext =
                implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });

            ConfigureMapperProfiles();

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 5003;
            });
        }

        private void ConfigureMapperProfiles()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<HotelMapperProfile>();
            });

            AutoMapper.Mapper.Configuration.AssertConfigurationIsValid();
        }

        private void SeedAmenties(BookingContext context)
        {
            List<Room> rooms = context.Rooms.ToList();
            List<Amenity> amenities = context.Amenities.ToList();
            if (!context.RoomAmenities.Any())
            {
                List<RoomAmenity> roomAmenities = new List<RoomAmenity>();

                foreach (Room room in rooms)
                {
                    foreach (Amenity amenity in amenities)
                    {
                        roomAmenities.Add(new RoomAmenity(room, amenity));
                    }
                }
                context.RoomAmenities.AddRange(roomAmenities);
                context.SaveChanges();
            }
        }


    }
}
