using DiplomskiAPI.Data;
using DiplomskiAPI.Model;
using DiplomskiAPI.Model.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using DiplomskiAPI.Utility;
using Microsoft.AspNetCore.Authorization;

namespace DiplomskiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private APIResponse _response;
        public OrderController(
            ApplicationDbContext context,
            APIResponse response)

        {
            _context = context;
            _response = response;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<APIResponse>> GetOrders(string? userID)
        {
            try
            {
                var orderHeaders = _context.OrderHeaders
                    .Include(x => x.OrderDetails)
                    .ThenInclude(x => x.Lek)
                    .OrderByDescending(x => x.OrderHeaderID);
                if (!string.IsNullOrEmpty(userID))
                {
                    _response.Result = orderHeaders.Where(x => x.ApplicationUserId == userID);
                }
                else
                {
                    _response.Result = orderHeaders;
                }
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.ToString());
            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "GetSpecific")]
        public async Task<ActionResult<APIResponse>> GetSpecificOrder(int id)
        {
            try
            {
                if(id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var orderHeaders = _context.OrderHeaders
                    .Include(x => x.OrderDetails)
                    .ThenInclude(x => x.Lek)
                    .Where(x => x.OrderHeaderID == id);
                if (orderHeaders == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                } 
                _response.Result = orderHeaders;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.ToString());
            }
            return _response;
        }
        
        [HttpPost]    
        public async Task<ActionResult<APIResponse>> CreateOrder([FromBody] OrderHeaderCreateDTO orderHeaderDTO)
        {
            try
            {
                OrderHeader order = new()
                {
                    ApplicationUserId = orderHeaderDTO.ApplicationUserID,
                    PickupEmail = orderHeaderDTO.PickupEmail,
                    PickupName = orderHeaderDTO.PickupName,
                    PickupPhoneNumber = orderHeaderDTO.PickupPhoneNumber,
                    OrderTotal = orderHeaderDTO.OrderTotal,
                    OrderDate = DateTime.Now,
                    StripePaymentID = orderHeaderDTO.StripePaymentID,
                    TotalItems = orderHeaderDTO.TotalItems,
                    Status = string.IsNullOrEmpty(orderHeaderDTO.Status) ? SD.status_pending : orderHeaderDTO.Status,
                };
                if (ModelState.IsValid)
                {
                    _context.OrderHeaders.Add(order);
                    _context.SaveChanges();
                    foreach(var orderDetailDTO in orderHeaderDTO.OrderDetailsDTO)
                    {
                        OrderDetail orderDetail = new()
                        {
                            OrderHeaderID = order.OrderHeaderID,
                            NazivLeka = orderDetailDTO.NazivLeka,
                            LekID = orderDetailDTO.LekID,
                            CenaLeka = orderDetailDTO.Cena,
                            Kolicina = orderDetailDTO.Kolicina,
                        };
                        _context.OrderDetails.Add(orderDetail);
                    }
                    _context.SaveChanges();
                    _response.Result = order;
                    order.OrderDetails = null;
                    _response.StatusCode = HttpStatusCode.Created;
                    return Ok(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.ToString());
            }
            return _response;
        }

        [HttpPut("{id:int}", Name = "UpdateOrderHeader")]
        public async Task<ActionResult<APIResponse>> UpdateOrderHeader(int id, [FromBody] OrderHeaderUpdateDTO orderHeaderUpdateDTO)
        {
            try
            {
                if(orderHeaderUpdateDTO == null || id != orderHeaderUpdateDTO.OrderHeaderID)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                OrderHeader orderHeaderDB = _context.OrderHeaders.FirstOrDefault(x => x.OrderHeaderID == id);
                if(orderHeaderDB == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                if (!string.IsNullOrEmpty(orderHeaderUpdateDTO.PickupName))
                {
                    orderHeaderDB.PickupName = orderHeaderUpdateDTO.PickupName;
                }
                if (!string.IsNullOrEmpty(orderHeaderUpdateDTO.PickupPhoneNumber))
                {
                    orderHeaderDB.PickupPhoneNumber = orderHeaderUpdateDTO.PickupPhoneNumber;
                }
                if (!string.IsNullOrEmpty(orderHeaderUpdateDTO.Status))
                {
                    orderHeaderDB.Status = orderHeaderUpdateDTO.Status;
                }
                if (!string.IsNullOrEmpty(orderHeaderUpdateDTO.StripePaymentID))
                {
                    orderHeaderDB.StripePaymentID = orderHeaderUpdateDTO.StripePaymentID;
                }
                if (!string.IsNullOrEmpty(orderHeaderUpdateDTO.PickupEmail))
                {
                    orderHeaderDB.PickupEmail = orderHeaderUpdateDTO.PickupEmail;
                }
                _context.SaveChanges();
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.ToString());
            }
            return _response;
        }
    }
}
