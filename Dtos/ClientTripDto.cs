using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace WebApplication4.Dtos;

public class ClientTripDto
{
    public required int IdClient { get; set; }
    [MaxLength(120)]
    public required string FirstName { get; set; }
    [MaxLength(120)]
    public required string LastName { get; set; }
    [MaxLength(120)]
    [EmailAddress]
    public required String Email { get; set; }
    [MaxLength(120)]
    public required String Telephone { get; set; }
    [MaxLength(120)]
    public required String Pesel { get; set; }
    public String PaymentTime { get; set; }
}