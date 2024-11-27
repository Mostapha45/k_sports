using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace backend.Controller
{


    /// <summary>
    /// Controller for handling checkout-related requests.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController() : ControllerBase
    {
        private static readonly string? EnvMode = Environment.GetEnvironmentVariable("ENV_MODE");

        private readonly string _domain = EnvMode == "production" ? "https://k-sports.vercel.app" : "http://localhost:3000";


        /// <summary>
        /// Initializes a new instance of the <see cref="CheckoutRequest"/> class.
        /// </summary>
        /// <param name="checkoutProducts">The list of products to checkout.</param>
        /// <param name="email">The email address of the customer.</param>
        [method: JsonConstructor]
        public class CheckoutRequest(List<CheckoutProduct> checkoutProducts, string email)
        {

            /// <summary>
            /// Gets or sets the list of products to checkout.
            /// </summary>
            public List<CheckoutProduct> CheckoutProducts { get; set; } = checkoutProducts;

            /// <summary>
            /// Gets or sets the email address of the customer.
            /// </summary>
            public string Email { get; set; } = email;
        }


        /// <summary>
        /// Creates a checkout session.
        /// </summary>
        /// <param name="checkoutRequest">The checkout request containing products and customer email.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost]
        public IActionResult CreateCheckoutSession([FromBody] CheckoutRequest checkoutRequest)

        {

            const int DELIVERY_DAYS = 7;
            const string PAYMENT_MODE = "payment";
            const string PAYMENT_TYPE = "card";
            const int PRICE_MULTIPLIER = 100;

            try
            {
                if (checkoutRequest.CheckoutProducts == null || checkoutRequest.CheckoutProducts.Count == 0)
                {
                    return BadRequest("No products provided for checkout");
                }

                if (string.IsNullOrEmpty(checkoutRequest.Email))
                {
                    return BadRequest("Email is required");
                }

                var deliveryDate = DateTime.UtcNow.AddDays(DELIVERY_DAYS);

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = [PAYMENT_TYPE],
                    LineItems = checkoutRequest.CheckoutProducts.Select(p => new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = p.PriceData.UnitAmount * PRICE_MULTIPLIER,
                            Currency = p.PriceData.Currency,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = p.PriceData.ProductData.Name,
                            },
                        },
                        Quantity = p.Quantity,
                    }).ToList(),
                    Mode = PAYMENT_MODE,
                    CustomerEmail = checkoutRequest.Email,
                    SuccessUrl = $"{_domain}/paymentsuccess?orderID={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = $"{_domain}/paymentfailed",
                    Metadata = new Dictionary<string, string>
                {
                    { "deliverDate", deliveryDate.ToString("o") }
                }

                };

                var service = new SessionService();
                var session = service.Create(options);

                return new JsonResult(new { url = session.Url });
            }
            catch
            {
                throw;
            }
        }
    }

    public struct CheckoutProduct
    {
        public required PriceData PriceData { get; set; }
        public long Quantity { get; set; }
    }

    public struct PriceData
    {
        public required string Currency { get; set; }
        public required ProductData ProductData { get; set; }
        public long UnitAmount { get; set; }
    }

    public struct ProductData
    {
        public required string Name { get; set; }
    }
}