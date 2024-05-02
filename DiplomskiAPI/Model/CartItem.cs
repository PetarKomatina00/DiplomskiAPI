using System.ComponentModel.DataAnnotations.Schema;

namespace DiplomskiAPI.Model
{
    public class CartItem
    {
        public int CartItemID { get; set; }

        public int LekID { get; set; }

        [ForeignKey("LekID")]
        public Lek Lek { get; set; }

        public int Kolicina { get; set; }

        public int ShoppingCartID { get; set; }
    }
}
