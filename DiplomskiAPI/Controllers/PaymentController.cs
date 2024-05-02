using DiplomskiAPI.Data;
using DiplomskiAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Stripe;
namespace DiplomskiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        protected readonly APIResponse _response;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        public PaymentController(APIResponse response, IConfiguration configuration, ApplicationDbContext context)
        {
            _response = response;
            _configuration = configuration;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> MakePayment(string userID)
        {
            ShoppingCart cart = _context.ShoppingCarts
                .Include(x => x.CartItems)
                .ThenInclude(x => x.Lek)
                .FirstOrDefault(x => x.UserID == userID);
            if(cart == null || cart.CartItems == null || cart.CartItems.Count() == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            #region Create Payment Intent
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            double cartTotal = cart.CartItems.Sum(x => x.Kolicina * x.Lek.Price);
            cart.TotalPrice = cartTotal;
            PaymentIntentCreateOptions options = new PaymentIntentCreateOptions
            {
                Amount = (int)(cart.TotalPrice * 100),
                Currency = "usd",
                PaymentMethodTypes = new List<string> {"card"}
            };
            PaymentIntentService service = new PaymentIntentService();
            PaymentIntent response = service.Create(options);
            cart.StripePaymentID = response.Id;
            cart.ClientSecret = response.ClientSecret;

            #endregion

            _response.Result = cart;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);

        }
    }
}
