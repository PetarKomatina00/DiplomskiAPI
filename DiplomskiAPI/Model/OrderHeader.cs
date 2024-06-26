﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiplomskiAPI.Model
{
    public class OrderHeader
    {

        [Key]
        public int OrderHeaderID { get; set; }

        [Required]
        public string PickupName { get; set; }

        [Required]
        public string PickupPhoneNumber { get; set; }

        [Required]
        public string PickupEmail { get; set; }

        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser User { get; set; }

        public double OrderTotal { get; set; }

        public DateTime OrderDate { get; set; }

        public string StripePaymentID { get; set; }

        public string Status { get; set; }

        public int TotalItems { get; set; }

        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
