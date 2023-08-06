using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;
public class Product {

    [Required]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "The Product Name must not be more than 50 characters.")]
    public string ProductName { get; set; } = string.Empty;

    [Required]
    [Range(0.0, Double.MaxValue, ErrorMessage = "The ammount must not be zero.")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "The ammount must not be zero.")]
    [Required]
    public int Ammount { get; set; }
}

