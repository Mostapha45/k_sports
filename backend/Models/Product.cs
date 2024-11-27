using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Product(string ItemName, string ImageSrc, string Description, double Price) : IValidatableObject
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; init; }

        [Column(TypeName = "citext")]
        public string ItemName { get; set; } = ItemName;

        public string? AuthorLink { get; set; }

        public string? AuthorName { get; set; }

        public string? ImageCredit { get; set; }
        public string ImageSrc { get; set; } = ImageSrc;

        public string Description { get; set; } = Description;
        
        [Column(TypeName = "decimal(10,2)")]
        public double Price { get; set; } = Price;

        public ICollection<ProductRelatedWord> ProductRelatedWords { get; set; } = [];

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (AuthorLink == null != (AuthorName == null) != (ImageCredit == null))
            {
                yield return new ValidationResult(
                    "All author and credit fields should be null together, or defined together",
                    [nameof(AuthorName), nameof(AuthorName), nameof(ImageCredit)]);
            }
        }
    }

}