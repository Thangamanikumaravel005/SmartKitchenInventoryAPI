using System.ComponentModel.DataAnnotations;

namespace SmartKitchenInventoryAPI.DTOs
{
    public class RegisterRequestDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "Employee";
    }
}