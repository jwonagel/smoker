// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using AuthServer.Data;
using AuthServer.Models;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;

namespace AuthServer
{



    public class Startup
    {

        private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // configures IIS out-of-proc settings (see https://github.com/aspnet/AspNetCore/issues/14882)
            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            // configures IIS in-proc settings
            services.Configure<IISServerOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("*")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                                  });
            });

            var connectionString = System.Environment.GetEnvironmentVariable("connectionString");
            services.AddDbContext<ApplicationDbContext>(options => options      
                .UseMySql(connectionString,      
                    mysqlOptions =>      
                        mysqlOptions.ServerVersion(new ServerVersion(new Version(10, 4, 6), ServerType.MariaDb))));  

            // services.AddDbContext<ApplicationDbContext>(options =>

            //     options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            
            var builder = services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddInMemoryApiResources(Config.Apis)
                .AddInMemoryIdentityResources(Config.GetIdentityResources)
                .AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<ApplicationUser>();

            if(Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {   
                string pwd = System.Environment.GetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Password");
                var certificate = System.Environment.GetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Path");

                var cert = new X509Certificate2(
                                certificate,
                                pwd,
                                X509KeyStorageFlags.MachineKeySet |
                                X509KeyStorageFlags.PersistKeySet |
                                X509KeyStorageFlags.Exportable
                            );

                builder.AddSigningCredential(cert);
            }
        }

        public void Configure(IApplicationBuilder app)
        {

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseCors(MyAllowSpecificOrigins);

            // app.UseStaticFiles();

            // app.UseRouting();
            // app.UseIdentityServer();
            // app.UseAuthorization();

            // app.UseEndpoints(endpoints =>
            // {
            //     endpoints.MapDefaultControllerRoute();
            // });

            
            app.Map("/auth", builder => {
                builder.UseCors(MyAllowSpecificOrigins);
                builder.UseStaticFiles();
                builder.UseRouting();
                builder.UseIdentityServer();
                builder.UseAuthorization();

                builder.UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                });
            });
        }

    }
}