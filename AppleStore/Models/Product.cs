using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AppleStore.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please type product name"), StringLength(100, ErrorMessage = "Length of product name is less than 100 characters")]      
        public string Name { get; set; }

        [Range(1.00, 10000.0, ErrorMessage = "Price is limited between 1-10000")]
        public decimal Price { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Product Image")]
        public string? ImageUrl { get; set; }

        public List<ProductImage>? ImageUrls { get; set; }

        [ForeignKey("Category")]
       
        public int CategoryId { get; set; }

        [DisplayName("Category")]
        public Category? Category { get; set; }
    }
}
