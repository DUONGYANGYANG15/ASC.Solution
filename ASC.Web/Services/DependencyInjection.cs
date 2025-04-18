﻿using ASC.DataAccess.Interfaces;
using ASC.DataAccess;
using ASC.Solution.Configuration;
using ASC.Solution.Data;
using ASC.Solution.Services;
using ASC.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ASC.Web.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration config)
        {

            // Add AddDbContext with connectionString to mirage database

            var connectionString = config.GetConnectionString("DefaultConnection") ??

                                   throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            //Add Options and get data from appsettings.json with "AppSettings"

            services.AddOptions(); // 10ption

            services.Configure<ApplicationSettings>(config.GetSection("AppSettings"));
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection = config.GetSection("Authentication:Google");
                    options.ClientId = config["Google:Identity:ClientID"];
                    options.ClientSecret = config["Google:Identity:ClientSecret"];
                });

            return services;

        }

        //Add service

        public static IServiceCollection AddMyDependencyGroup(this IServiceCollection services)
        {

            //Add ApplicationDbContext

            services.AddScoped<DbContext, ApplicationDbContext>();

            //Add Identity User IdentityUser

            services.AddIdentity<IdentityUser, IdentityRole>((options) =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            //Add services

            services.AddTransient<IEmailSender, AuthMessageSender>();

            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddSingleton<IIdentitySeed, IdentitySeed>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSession();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDistributedMemoryCache();
            services.AddSingleton<INavigationCacheOperations, NavigationCacheOperations>();
            //Add RazorPages, MVC

            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddControllersWithViews();

            return services;
        }
    }
}