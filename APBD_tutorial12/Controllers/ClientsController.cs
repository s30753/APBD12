using APBD_tutorial12.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_tutorial12.Controllers;

[Route("api/clients")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly ClientsService _clientService;

    public ClientsController(ClientsService clientService)
    {
        _clientService = clientService;
    }

    [HttpDelete("{idClient}")]
    public async Task<IActionResult> DeleteClientAsync(int idClient)
    {
        var result = await _clientService.DeleteClientAsync(idClient);
        if (result == 1) return Ok("Client deleted successfully");
        if (result == 0) return BadRequest("Client not found");
        return Conflict("Client has a trip so cannot be deleted");
        
    }
}