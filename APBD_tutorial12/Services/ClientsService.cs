using APBD_tutorial12.Data;

namespace APBD_tutorial12.Services;

public class ClientsService : IClientsService
{
    private readonly TravelDbContext _context;

    public ClientsService(TravelDbContext context)
    {
        _context = context;
    }

    public async Task<int> DeleteClientAsync(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null) return 0;
        
        var searchedClient = await _context.ClientTrips.FindAsync(id);
        if (searchedClient != null) return -1;
        
        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return 1;
    }
}