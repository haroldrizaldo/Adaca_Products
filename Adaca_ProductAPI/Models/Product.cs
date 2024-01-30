namespace Adaca_ProductWebApp.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price should be a positive number")]
        public decimal Price { get; set; }
    }
}