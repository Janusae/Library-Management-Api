using Application.Common;
using Application.CQRS.User;
using Application.Services;
using Infrastructure.Context;
using Infrastructure.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter JWT token with the 'Bearer' scheme, e.g., 'Bearer {token}'",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });

        // Configure JWT Authentication
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "Hospital",
                ValidAudience = "Admin",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("c5f0e609953d1e8721005d3b275e85c6cc387f8f74883ec152cd06c5cc3e8029"))
            };
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Log.Error("Authentication failed: {0}", context.Exception.Message);
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    Log.Information("Token validated successfully for user: {0}", context.Principal.Identity.Name);
                    return Task.CompletedTask;
                }
            };
        });

        builder.Services.AddScoped<IPasswordManagement, Password_Management>();
        builder.Services.AddScoped<ILoggerService, LoggerService>();
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateUserCommand).Assembly);
        });
        builder.Services.AddDbContext<ProgramDbContext>(options =>
        {
            options.UseSqlServer("server=.;database=LibraryManagement;TrustServerCertificate=true;Trusted_Connection=true");
        });
        builder.Services.AddScoped<ResponseHandler>();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("Logs/Logger.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication(); 
        app.UseAuthorization();
        app.MapControllers();
        app.MapGet("/", () => Results.Redirect("/swagger/index.html"))
            .ExcludeFromDescription();

        app.Run();
    }
}