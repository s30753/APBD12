using APBD_tutorial12.Data;
using APBD_tutorial12.Services;
using Microsoft.EntityFrameworkCore;

namespace APBD_tutorial12;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddControllers();
        builder.Services.AddDbContext<TravelDbContext>(opt => 
            opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
        
        builder.Services.AddScoped<IClientsService, ClientsService>();
        builder.Services.AddScoped<ITripsService, TripsService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}