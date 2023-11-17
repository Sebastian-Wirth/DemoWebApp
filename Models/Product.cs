using System.ComponentModel.DataAnnotations;

namespace DemoWebApp.Models
{
    public class Product
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Price { get; set; }

        [Required]
        public string ImageUrl { get; set; }

    }
}
