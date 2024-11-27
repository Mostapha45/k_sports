using backend.Models;

namespace backend.Mappers
{
    public static class ProductMapper
    {
        public static ProductResponseDto MapToProductResponseDto(this Product product)
        {
            return new ProductResponseDto(
                product.Id,
                product.ItemName,
                product.ImageSrc,
                product.Description,
                product.Price,
                product.AuthorLink,
                product.AuthorName,
                product.ImageCredit
            );
        }
    }
}