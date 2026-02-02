using System.ComponentModel.DataAnnotations;

namespace MyStockApp.Models;

public class User
{
    [Key]
    public int UserId { get; set; }
    
    [Required]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    public string Role { get; set; } = "User"; // "Admin" or "User"
}
