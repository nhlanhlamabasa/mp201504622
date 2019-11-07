namespace Web
{
    using IdentityModel;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Polly;
    using Polly.Extensions.Http;
    using Rotativa.AspNetCore;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Net.Http;
    using Web.Interfaces;
    using Web.Models;
    using Web.Services;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.Cookie.Name = ".BlueOrbHotels.Session";
                options.IdleTimeout = TimeSpan.FromHours(5);
                options.Cookie.HttpOnly = true;
            });
            services.Configure<CloudinaryDetails>(Configuration.GetSection("CloudinaryDetails"));

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });
            //Configuration
            ConfigureAuthentication(services);
            ConfigureHttpClients(services);
            ConfigureTransientServices(services);

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 5005;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("GUEST", policy => policy.RequireClaim(JwtClaimTypes.Role, "GUEST"));
                options.AddPolicy("ADMIN", policy => policy.RequireClaim(JwtClaimTypes.Role, "ADMIN"));
                options.AddPolicy("MANAGER", policy => policy.RequireClaim(JwtClaimTypes.Role, "MANAGER"));
                options.AddPolicy("FRONTDESK", policy => policy.RequireClaim(JwtClaimTypes.Role, "FRONTDESK"));
            });
            
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseCookiePolicy();

            // var webRootPath = env.WebRootPath;
            // call rotativa conf passing env to get web root path
            RotativaConfiguration.Setup(env);
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";

            })
            .AddCookie("Cookies", options =>
            {
                options.LoginPath = "/Account/Login/";
                options.AccessDeniedPath = "/Account/Forbidden/";
                options.LogoutPath = "/Account/Logout/";
            })
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "Cookies";
                options.Authority = Configuration["Identity:url"];
                options.RequireHttpsMetadata = true;

                options.ClientId = Configuration["Client:client_id"];
                options.ClientSecret = Configuration["Client:client_secret"];
                options.ResponseType = "code id_token";

                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;

                options.Scope.Add("booking_api");
                options.Scope.Add("offline_access");
            });
        }

        private void ConfigureHttpClients(IServiceCollection services)
        {
            services.AddHttpClient(NamedClients.BookingClient, client =>
            {
                client.BaseAddress = new Uri(BaseUri.BookingClient);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddHttpClient(NamedClients.IdentityClient, client =>
            {
                client.BaseAddress = new Uri(BaseUri.IdentityClient);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .RetryAsync(3);

            var noOp = Policy.NoOpAsync().AsAsyncPolicy<HttpResponseMessage>();

            services.AddHttpClient(NamedClients.BookingClient)
                .AddPolicyHandler(request => request.Method == HttpMethod.Get ? retryPolicy : noOp);

            services.AddHttpClient(NamedClients.IdentityClient)
                .AddPolicyHandler(request => request.Method == HttpMethod.Get ? retryPolicy : noOp);
        }

        private void ConfigureTransientServices(IServiceCollection services)
        {
            services.AddTransient<IBookingClient, BookingClient>();
            services.AddTransient<IManagerClient, ManagerClient>();
            services.AddTransient<IIdentityClient, IdentityClient>();
            services.AddTransient<IUserClaims, UserClaims>();
        }
    }
}
