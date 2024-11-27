namespace backend.Models
{
    public class ProductItemDto
    {
        public long Price { get; set; }
        public string Name { get; set; } = string.Empty;
        public long Quantity { get; set; }
    }

    public class TrackDeliveryResponseDto
    {
        public IEnumerable<ProductItemDto> Products { get; set; } = [];
        public DateTime DeliveryDate { get; set; }
    }
}