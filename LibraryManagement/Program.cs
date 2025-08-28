using Application.Common;
using Application.CQRS.User;
using Application.Services;
using Infrastructure.Context;
using Infrastructure.Logging;
using Microsoft.EntityFrameworkCore;
using Serilog;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
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
            .WriteTo.File("Logs/Logger.txt" , rollingInterval:RollingInterval.Day)
            .CreateLogger();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseSwagger();

        app.UseSwaggerUI();

        app.UseAuthorization();

        app.MapControllers();
        app.MapGet("/", () => Results.Redirect("/swagger/index.html"))
            .ExcludeFromDescription();
        app.Run();
    }
}