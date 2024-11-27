using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class ProductRelatedWord(string word)
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; init; }

        [Column(TypeName = "citext")]
        public string Word { get; set; } = word;
        public ICollection<Product> Products { get; set; } = [];
    }

}