using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiplomskiAPI.Model
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailsID { get; set; }
        [Required]
        public int OrderHeaderID { get; set; }

        [Required]
        public int LekID { get; set; }
        [ForeignKey("LekID")]
        public Lek Lek { get; set; }

        [Required]
        public int Kolicina { get; set; }

        [Required]
        public string NazivLeka { get; set; }

        [Required]
        public double CenaLeka { get; set; }
    }
}
