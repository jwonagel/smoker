// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using AuthServer.Data;
using AuthServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace AuthServer
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString, string pathToUserJson)
        {
            var services = new ServiceCollection();
            services.AddLogging();

            services.AddDbContext<ApplicationDbContext>(options => options      
                .UseMySql(connectionString,      
                    mysqlOptions =>      
                        mysqlOptions.ServerVersion(new ServerVersion(new Version(10, 4, 6), ServerType.MariaDb))));  


            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var users = UserModel.LoadData(pathToUserJson);


            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    context.Database.Migrate();

                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                    foreach(var user in users)
                    {
                        var dbuser = userMgr.FindByNameAsync(user.GivenName).Result;
                        if (dbuser == null)
                        {
                            dbuser = new ApplicationUser
                            {
                                UserName = user.GivenName.ToLower()
                            };

                            var result = userMgr.CreateAsync(dbuser, user.Password).Result;

                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }

                            result = userMgr.AddClaimsAsync(dbuser, new Claim[]{
                                new Claim(JwtClaimTypes.Name, user.Name),
                                new Claim(JwtClaimTypes.GivenName, user.GivenName),
                                new Claim(JwtClaimTypes.Email, user.Email),
                                new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                                new Claim(JwtClaimTypes.Role, user.Role)
                            }).Result;
                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }
                            Log.Debug($"User {user.Name} created");
                        } 
                        else 
                        {
                            Log.Debug($"User {user.Name} already exists");
                        }
                    }



                }
            }
        }
    }
}
