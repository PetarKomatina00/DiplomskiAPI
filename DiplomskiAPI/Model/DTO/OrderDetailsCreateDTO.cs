using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiplomskiAPI.Model.DTO
{
    public class OrderDetailsCreateDTO
    {

        [Required]
        public int LekID { get; set; }

        [Required]
        public int Kolicina { get; set; }

        [Required]
        public string NazivLeka { get; set; }

        [Required]
        public double Cena { get; set; }
    }
}
