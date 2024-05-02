using System.ComponentModel.DataAnnotations;

namespace DiplomskiAPI.Model.DTO
{
    public class OrderHeaderCreateDTO
    {

        [Required]
        public string PickupName { get; set; }

        [Required]
        public string PickupPhoneNumber { get; set; }

        [Required]
        public string PickupEmail { get; set; }

        public string ApplicationUserID { get; set; }
        public double OrderTotal { get; set; }

        public string StripePaymentID { get; set; }

        public string Status { get; set; }

        public int TotalItems { get; set; }

        public IEnumerable<OrderDetailsCreateDTO> OrderDetailsDTO { get; set; }
    }
}
