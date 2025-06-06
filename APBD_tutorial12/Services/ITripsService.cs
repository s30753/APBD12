using APBD_tutorial12.DTOs;

namespace APBD_tutorial12.Services;

public interface ITripsService
{
    Task<object> GetTripAsync();
    Task<int> AssignClientToTripAsync(int idTrip, ClientTripDTO clientTrip);
}