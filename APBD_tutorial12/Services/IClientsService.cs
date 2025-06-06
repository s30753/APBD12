namespace APBD_tutorial12.Services;

public interface IClientsService
{
    Task<int> DeleteClientAsync(int id);
}