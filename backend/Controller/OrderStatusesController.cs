using backend.Mappers;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace backend.Controller
{

    /// <summary>
    /// Controller for handling order status-related requests.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderStatusesController : ControllerBase
    {

        /// <summary>
        /// Gets the order status data.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <param name="email">The email address of the customer.</param>
        /// <returns>An <see cref="IActionResult"/> containing the order status data.</returns>
        [HttpGet]
        public IActionResult GetData(string orderId, string email)
        {
            try
            {

                var sessionService = new SessionService();
                var session = sessionService.Get(orderId);

                if (session.CustomerEmail != email)
                {
                    return Unauthorized("Unauthorized");
                }

                var metadata = session.Metadata;

                var deliveryDate = Convert.ToDateTime(metadata["deliverDate"]);
                return Ok(sessionService.ListLineItems(orderId).MapToTrackDeliveryResponseDto(deliveryDate));
            }
            catch
            {
                throw;
            }
        }

    }
}