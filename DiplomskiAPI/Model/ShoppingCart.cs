using System.ComponentModel.DataAnnotations.Schema;

namespace DiplomskiAPI.Model
{
    public class ShoppingCart
    {
        public int ShoppingCartID { get; set; }

        public string UserID { get; set; }

        public ICollection<CartItem> CartItems { get; set; }

        [NotMapped]
        public double TotalPrice { get; set; }

        [NotMapped]
        public string StripePaymentID { get; set; }

        [NotMapped]
        public string ClientSecret { get; set; }

    }
}
