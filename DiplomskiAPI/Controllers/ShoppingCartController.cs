using DiplomskiAPI.Data;
using DiplomskiAPI.Model;
using DiplomskiAPI.Model.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DiplomskiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private APIResponse _response;
        public ShoppingCartController(
            ApplicationDbContext context,
            APIResponse response,
            IWebHostEnvironment hostEnvironment)

        {
            _context = context;
            _response = response;
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetShoppingCart(string userID)
        {
            try
            {
                ShoppingCart shoppingCart;
                if (string.IsNullOrEmpty(userID))
                {
                    shoppingCart = new();
                }
                else
                {
                    shoppingCart = _context.ShoppingCarts
                    .Include(x => x.CartItems)
                    .ThenInclude(x => x.Lek)
                    .FirstOrDefault(x => x.UserID == userID);
                }
                if(shoppingCart != null && shoppingCart.CartItems != null && shoppingCart.CartItems.Count > 0)
                {
                    shoppingCart.TotalPrice = shoppingCart.CartItems.Sum(x => x.Kolicina * x.Lek.Price);
                }
                _response.Result = shoppingCart;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch(Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> AddOrUpdateItemInCart(string userID, int lekID, int quantity)
        {
            // Korisnik zeli da doda Lek po prvi put
            // Korisnik zeli da doda Lek u vec postojecoj korpi
            // Update
            // Remove

            ShoppingCart shoppingCart = _context.ShoppingCarts.Include(x => x.CartItems).FirstOrDefault(x => x.UserID == userID);
            Lek lek = _context.Lekovi.FirstOrDefault(x => x.LekID == lekID);
            if(lek == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Lek ne postoji");
                return _response;
            }
            //Ako shoppingCart ne postoji
            if(shoppingCart == null && quantity > 0)
            {
                _response = new APIResponse();
                //Pravimo shopping cart
                shoppingCart = new ShoppingCart();
                shoppingCart.UserID = userID;
                _context.ShoppingCarts.Add(shoppingCart);
                _context.SaveChanges();
                //Pravimo nas Menu item;
                CartItem cartItem = new()
                {
                    LekID = lekID,
                    Kolicina = quantity,
                    ShoppingCartID = shoppingCart.ShoppingCartID,
                    Lek = null,
                };
                _context.CartItems.Add(cartItem);
                _context.SaveChanges();

            }
            else
            {
                //Shopping cart postoji
                //Dodajemo Lek ali moramo da vidimo da li taj Lek vec postoji u cartItems
                //Ono sto je bitno jeste da ovaj cart mora da pripada konkretnom korisniku!
                CartItem cartItem = shoppingCart.CartItems.FirstOrDefault(x => x.LekID == lekID);
                if(cartItem == null)
                {
                    CartItem newCartItem = new()
                    {
                        LekID = lekID,
                        Kolicina = quantity,
                        ShoppingCartID = shoppingCart.ShoppingCartID,
                        Lek = null,
                    };
                    _context.CartItems.Add(newCartItem);
                    _context.SaveChanges();
                }
                else
                {
                    //lek u cartItem vec postoji, samo treba kolicina da se updejtuje
                    int novaKolicina = cartItem.Kolicina + quantity;
                    if(novaKolicina <= 0 || quantity == 0)
                    {
                        _context.CartItems.Remove(cartItem);
                        if(shoppingCart.CartItems.Count() == 1)
                        {
                            _context.ShoppingCarts.Remove(shoppingCart);
                        }
                        _context.SaveChanges();
                    }
                    else
                    {
                        cartItem.Kolicina = novaKolicina;
                        _context.SaveChanges();
                    }
                }
            }

            return _response;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<APIResponse>> DeleteShoppingCart(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            var shoppingCart = await _context.ShoppingCarts
            .Include(s => s.CartItems) // Include related items
            .FirstOrDefaultAsync(s => s.ShoppingCartID == id);
            if (shoppingCart == null)
            {
                return NotFound();
            }
            try
            {
                _context.CartItems.RemoveRange(shoppingCart.CartItems);
                _context.ShoppingCarts.Remove(shoppingCart);
                _context.SaveChanges();
                _response.IsSuccess = true;
                _response.Result = shoppingCart;
            }
            catch(Exception e)
            {
                _response.IsSuccess = false;
                _response.Result = e.Message;
            }

            return _response;
        }
        private void setResponse(HttpStatusCode status, bool isSucces, string errorMessage, LoginResponseDTO result)
        {
            _response.ErrorMessages.Add(errorMessage);
            _response.StatusCode = status;
            _response.IsSuccess = isSucces;
            _response.Result = result;
        }
    }
}
