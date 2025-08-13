using Application.CQRS.User;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateUserCommand).Assembly);
        });

        builder.Services.AddDbContext<ProgramDbContext>(options =>
        {
            options.UseSqlServer("server=.;database=LibraryManagement;TrustServerCertificate=true;Trusted_Connection=true");
        });
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
        app.MapGet("/", () => Results.Redirect("/swagger"));
        app.Run();
    }
}