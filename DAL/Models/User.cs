using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

public class User {
    [Required]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    public string UserName { get; set; } = string.Empty;
    [Required]
    [StringLength(75)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    [StringLength(300, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 10)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;            
}

