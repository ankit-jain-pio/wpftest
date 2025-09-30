using System.ComponentModel.DataAnnotations;

namespace SimpleWpfMvvmAppV8.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Email { get; set; }
}
