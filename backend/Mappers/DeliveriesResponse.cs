using backend.Models;

namespace backend.Mappers
{
    public static class CoachMapper
    {
        public static TrackDeliveryResponseDto MapToTrackDeliveryResponseDto(this Stripe.StripeList<Stripe.LineItem> items, DateTime deliveryDate)
        {
            return new TrackDeliveryResponseDto {
                DeliveryDate = deliveryDate,
                Products = items.Select(i => new ProductItemDto {
                    Name = i.Description,
                    Price = i.AmountTotal,
                    Quantity = i.Quantity!.Value
                })
            };
        }
    }
}