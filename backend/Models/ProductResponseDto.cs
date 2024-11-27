using System.Text.Json.Serialization;

namespace backend.Models
{
    public readonly record struct ProductResponseDto(
        int Id,
        string ItemName,
        string ImageSrc,
        string Description,
        double Price,
        [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? AuthorLink = null,
        [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? AuthorName = null,
        [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? ImageCredit = null
    );
}