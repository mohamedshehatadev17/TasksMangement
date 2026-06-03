using System;
using System.Security.Claims;
using System.Text;
using Application;
using Asp.Versioning;
using FluentValidation;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskMangement.API.Middlewares;
using TaskMangement.Application.Abstractions.Authentication;
using TaskMangement.Domain.Models;
using TaskMangement.Infrastructure.Authentication;
using TaskMangement.Infrastructure.Persistance.Authentication.Seed;
using TaskMangement.Infrastructure.Persistance.Authentication.Seeders;
using TaskMangement.Infrastructure.Persistance.Contexts;

namespace TaskMangement.API
{
    public class Program
    {
        public static async System.Threading.Tasks.Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Controllers
            builder.Services.AddControllers();

            // FluentValidation
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();
            // swagger
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddApplication();
            builder.Services.AddInfrastructure();

            // Database
            #region Database

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"), 
                    sql => sql.EnableRetryOnFailure()
                    ));
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration =
                    builder.Configuration.GetConnectionString("RedisConnection");
            });
            #endregion

            // JWT Generator
            builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            // Identity + Roles
            #region Identity + Roles

            builder.Services
                .AddIdentity<User, IdentityRole<Guid>>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = true;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            #endregion

            // Authentication
            #region Authentication

            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme =
                        JwtBearerDefaults.AuthenticationScheme;

                    options.DefaultChallengeScheme =
                        JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer =
                                builder.Configuration["Jwt:Issuer"],

                            ValidAudience =
                                builder.Configuration["Jwt:Audience"],

                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Convert.FromBase64String(
                                        builder.Configuration["Jwt:Key"]!)),

                            RoleClaimType = ClaimTypes.Role
                        };

                    options.MapInboundClaims = false;
                });
            #endregion

            #region API Versioning

            builder.Services
                .AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1, 0);

                    options.AssumeDefaultVersionWhenUnspecified = true;

                    options.ReportApiVersions = true;

                    options.ApiVersionReader = new UrlSegmentApiVersionReader();
                })
                .AddMvc(); // 🔥 REQUIRED for controllers
            #endregion


            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Seed Roles and Admin User
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                await RoleSeeder.SeedAsync(roleManager);
                await AdminSeeder.SeedAsync(scope.ServiceProvider);

            }
            // Swagger/OpenAPI
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            // Middlewares
            app.UseHttpsRedirection();
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();

            // Controllers
            app.MapControllers();

            app.Run();
        }
    }
}
