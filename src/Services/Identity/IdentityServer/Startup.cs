using IdentityServer.API.Configuration;
using IdentityServer.API.Data;
using IdentityServer.API.Models;
using IdentityServer.Models;
using IdentityServer4;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Quickstart.UI;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            HostingEnvironment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddCors(options =>
            {
                options.AddPolicy("non_identity_endpoints_cors", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });

            AddDbContext(services);
            AddAuthentication(services);
            AddExternalAuthentication(services);
            services.AddTransient<DataContextSeedData>();

            /*services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 5001;
            });*/
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DataContextSeedData seedData)
        {
            InitializeDatabase(app, seedData);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("non_identity_endpoints_cors");
            app.UseIdentityServer();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseCookiePolicy();
        }

        private void InitializeDatabase(IApplicationBuilder app, DataContextSeedData seedData)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in Config.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Config.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                seedData.EnsureSeedData().Wait();
            }
        }

        private void AddAuthentication(IServiceCollection services)
        {
            string connectionString = string.Empty;
            string password = string.Empty;

            if(!HostingEnvironment.IsDevelopment())
            {
                connectionString = Environment.GetEnvironmentVariable("default_connection");
                password = Environment.GetEnvironmentVariable("password");
            }
            else
            {
                connectionString = Configuration.GetConnectionString("DefaultConnection");
                password = Configuration["Password"];
            }
                
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            //.AddSigningCredential(new X509Certificate2(@".\Certification\IdentityServer4Auth.pfx", password))
            .AddDeveloperSigningCredential()
            .AddAspNetIdentity<ApplicationUser>()
            .AddTestUsers(TestUsers.Users)
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(migrationsAssembly);
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                        });
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(migrationsAssembly);
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                        });
                options.EnableTokenCleanup = true;
            })
            .AddProfileService<ProfileService>();

        }

        private void AddDbContext(IServiceCollection services)
        {
            string connectionString = string.Empty;
            if (!HostingEnvironment.IsDevelopment())
            {
                connectionString = Environment.GetEnvironmentVariable("default_connection");
            }
            else
            {
                connectionString = Configuration.GetConnectionString("DefaultConnection");
            }

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
               {
                   sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                   sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
               }));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();            
        }

        private void AddExternalAuthentication(IServiceCollection services)
        {
            string FaceBookClientId = string.Empty;
            string FaceBookClientSecret = string.Empty;

            string GoogleClientId = string.Empty;
            string GoogleClientSecret = string.Empty;

            if(!HostingEnvironment.IsDevelopment())
            {
                FaceBookClientId = Environment.GetEnvironmentVariable("facebook_client_id");
                FaceBookClientSecret = Environment.GetEnvironmentVariable("facebook_client_secret");

                GoogleClientId = Environment.GetEnvironmentVariable("google_client_id");
                GoogleClientSecret = Environment.GetEnvironmentVariable("google_client_secret");
            }
            else
            {
                FaceBookClientId = Configuration["Facebook:client_id"];
                FaceBookClientSecret = Configuration["Facebook:client_secret"];

                GoogleClientId = Configuration["Google:client_id"];
                GoogleClientSecret = Configuration["Google:client_secret"];
            }

            services.AddAuthentication()
          .AddGoogle("Google", options =>
          {
              options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

              options.ClientId = GoogleClientId;
              options.ClientSecret = GoogleClientSecret;

          })
          .AddFacebook("Facebook", options =>
          {
              options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

              options.ClientId = FaceBookClientId;
              options.ClientSecret = FaceBookClientSecret;

          });
        }
    }
}
