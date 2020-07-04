using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using Microsoft.OpenApi.Models;
using api.Model.Dababase;
using api.Services;
using AutoMapper;
using api.Model;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Http;
using api.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;


// dotnet new is4aspid --force -n foo

namespace api
{
    public class Startup
    {

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Environment.GetEnvironmentVariable("connectionString");
            services.AddDbContext<SmokerDBContext>(options => options
                .UseMySql(connectionString,
                    mysqlOptions =>
                        mysqlOptions.ServerVersion(new ServerVersion(new Version(10, 4, 6), ServerType.MariaDb))));

            
           services.AddCors(options =>
            {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("https://localhost:4200")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .SetIsOriginAllowed((x) => true)
                           .AllowCredentials();
                });
            });

            
            var mappingConfig = new MapperConfiguration(mc =>
              {
                  mc.AddProfile(new AutoMapping());
              });

            IMapper mapper = mappingConfig.CreateMapper();


            services.AddSingleton(mapper);
            // register an IHttpContextAccessor so we can access the current
            // HttpContext in services by injecting it
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ISmokerService, SmokerService>();
            services.AddScoped<IUserInfoService, UserInfoService>();
            services.AddScoped<INotficationService, NotficationService>();
            services.AddSingleton<ISmokerConnectionService, SmokerConnectionService>();
            services.AddSingleton<INotificationInfoService, NotificationInfoService>();

            services.AddHttpClient<ISlackMessageSender, SlackMessageSender>(c => 
            {
                c.BaseAddress = new Uri("https://hooks.slack.com");
            });

            services.AddControllers();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Smoker API", Version = "v1" });
                
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \n\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference 
                                { 
                                    Type = ReferenceType.SecurityScheme, 
                                    Id = "Bearer" 
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header
                            },
                            new string[] {}

                    }
                });
            });

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Environment.GetEnvironmentVariable("Authority");
                    options.ApiName = "smokerapi";
                    options.Events = new JwtBearerEvents{
                        OnMessageReceived = context => 
                        {
                            var accessToken = context.Request.Query["access_token"];
                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/messagehubB")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddSignalR();
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(MyAllowSpecificOrigins);

            UpdateDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Map("/api", builder => 
            {
                builder.UseCors(MyAllowSpecificOrigins);
                builder.UseSwagger();
                builder.UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "Smoker API V1");
                    });
                builder.UseRouting();
                builder.UseAuthentication();
                builder.UseAuthorization();

                builder.UseEndpoints(endpoints =>
                {
                    endpoints.MapHub<MessageHub>("/messagehub");
                    endpoints.MapControllers();
                });
            });
        }


        private void UpdateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<SmokerDBContext>();

            context.Database.Migrate();

            if(!context.Settings.Any())
            {
                var settings = new Settings
                {
                    FireNotifcationTemperatur = 80,
                    IsAutoMode = true,
                    LastSettingsActivation = DateTime.Now,
                    LastSettingsUpdateUser = "System",
                    OpenCloseTreshold = 120,
                    TemperaturUpdateCycleSeconds = 120,
                    LastSettingsUpdate = DateTime.Now,
                    SettingsId = Guid.NewGuid(),
                    Alerts = new List<Alert>() 
                };
                context.Settings.Add(settings);
                context.SaveChanges();
            }
        }
    }
}
