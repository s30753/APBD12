using APBD_tutorial12.Data;
using APBD_tutorial12.DTOs;
using APBD_tutorial12.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD_tutorial12.Services;

public class TripsService : ITripsService
{
    private readonly TravelDbContext _context;

    public TripsService(TravelDbContext context)
    {
        _context = context;
    }

    public async Task<object> GetTripAsync()
    {
        var trips = await _context.Trips
            .OrderByDescending(t => t.DateFrom)
            .Select(t => new TripDTO
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.CountryTrips.Select(ct => new CountryDTO
                {
                    Name = ct.Country.Name
                }).ToList(),
                Clients = t.ClientTrips.Select(ct => new ClientDTO
                {
                    FirstName = ct.IdClientNavigation.FirstName,
                    LastName = ct.IdClientNavigation.LastName
                }).ToList()
            }).ToListAsync();

        int count = trips.Count;
        return new
        {
            pageNum = 1,
            pageSize = 10,
            allPages = (int)Math.Ceiling((double)count / 10),
            trips
        };
    }

    public async Task<int> AssignClientToTripAsync(int idTrip, ClientTripDTO clientTrip)
    {
        var existingClient = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == clientTrip.Pesel);
        if (existingClient != null) return -1;
        
        var trip = await _context.Trips.FirstOrDefaultAsync(t => t.IdTrip == idTrip);
        if (trip == null || trip.DateFrom < DateTime.Now) return -2;

        var newClient = new Client
        {
            FirstName = clientTrip.FirstName,
            LastName = clientTrip.LastName,
            Email = clientTrip.Email,
            Telephone = clientTrip.Telephone,
            Pesel = clientTrip.Pesel
        };
        
        await _context.Clients.AddAsync(newClient);
        await _context.SaveChangesAsync();
        
        var id = await _context.Clients.Where(c => c.Pesel == clientTrip.Pesel)
            .Select(c => c.IdClient).FirstOrDefaultAsync();
        
        var alreadyRegisteredClient = await _context.ClientTrips
            .FirstOrDefaultAsync(ct => ct.IdClient == id && ct.IdTrip == idTrip);
        if (alreadyRegisteredClient != null) return -3;

        await _context.ClientTrips.AddAsync(new ClientTrip
        {
            IdClient = newClient.IdClient,
            IdTrip = idTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = clientTrip.PaymentDate
        });
        await _context.SaveChangesAsync();
        return 1;
    }
}