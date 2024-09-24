using System.ComponentModel.DataAnnotations;

namespace BookStore.Shared.Entities
{
    public class Book
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        public string Author { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int NoOfPages { get; set; }

        [Required]
        [StringLength(50)]
        public string Language { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }
    }
}
