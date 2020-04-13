using AutoMapper;
using DbUp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Staffinfo.Divers.Data.Repositories;
using Staffinfo.Divers.Data.Repositories.Contracts;
using Staffinfo.Divers.Infrastructure.Middleware;
using Staffinfo.Divers.Models;
using Staffinfo.Divers.Services;
using Staffinfo.Divers.Services.Contracts;
using System;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Staffinfo.Divers
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string userDbConnectionString = Configuration.GetConnectionString("DefaultConnection");

            EnsureDatabase.For.PostgresqlDatabase(userDbConnectionString);

            var upgrader =
                DeployChanges.To
                .PostgresqlDatabase(userDbConnectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

            var result = upgrader.PerformUpgrade();

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            Settings.SecurityKey = Configuration["SecurityKey"];

            services.AddTransient<IRescueStationRepository, RescueStationRepository>(provider => new RescueStationRepository(userDbConnectionString));
            services.AddTransient<IDiverRepository, DiverRepository>(provider => new DiverRepository(userDbConnectionString));
            services.AddTransient<IUserRepository, UserRepository>(provider => new UserRepository(userDbConnectionString));
            services.AddTransient<IDivingTimeRepository, DivingTimeRepository>(provider => new DivingTimeRepository(userDbConnectionString));
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRescueStationService, RescueStationService>();
            services.AddTransient<IDiverService, DiverService>();
            services.AddSingleton<UserManager>();
            services.AddHttpContextAccessor();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JwtIssuer"],
                    ValidAudience = Configuration["JwtAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSecurityKey"])),
                    SaveSigninToken = true,
                    ClockSkew = TimeSpan.FromSeconds(5)
                };
                //options.Events = new JwtBearerEvents()
                //{
                //    // handle 401 manually to send custom response
                //    OnAuthenticationFailed = c =>
                //    {
                //        c.NoResult();
                //        c.Response.StatusCode = 401;
                //        c.Response.ContentType = "application/json";
                //        c.Response.WriteAsync(new ErrorResponse("AuthenticationFailed").ToString()).Wait();

                //        return Task.CompletedTask;
                //    },
                //    OnChallenge = c =>
                //    {
                //        if (!c.Response.HasStarted)
                //        {
                //            c.Response.StatusCode = 401;
                //            c.Response.ContentType = "application/json";
                //            c.Response.WriteAsync(new ErrorResponse("AuthenticationFailed").ToString()).Wait();
                //        }

                //        c.HandleResponse();

                //        return Task.CompletedTask;
                //    }
                //};
            });

            services.AddCors(options =>
            {
                options.AddPolicy("StaffinfoDiversAPICors",
                    builder =>
                        builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin());
            });


            services.AddDistributedMemoryCache();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.IsEssential = true;
            });
            services.AddAutoMapper(typeof(Startup));

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession();

            app.UseMiddleware<JwtManagerMiddleware>();

            app.UseStatusCodePages(async context =>
            {
                await Task.Run(() =>
                {
                    var response = context.HttpContext.Response;

                    if (response.StatusCode == (int)HttpStatusCode.Unauthorized ||
                        response.StatusCode == (int)HttpStatusCode.Forbidden)
                        response.Redirect("/Login");
                });
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Dashboard}/{action=Index}/{id?}");
            });
        }
    }
}
