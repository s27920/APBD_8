using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace WebApplication4.Dtos;

public class ClientTripDto
{
    [Required]
    public int IdClient { get; set; }
    [Required]
    public int IdTrip { get; set; }
    [Required] 
    public int RegisteredAt { get; set; }
    public int? PaymentDate { get; set; }
}