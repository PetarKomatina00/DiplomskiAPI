using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace DiplomskiAPI.Model
{
    public class Lek
    {
        [Key]
        public int LekID { get; set; }

        [Required]
        public string NazivLeka { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ISBN { get; set; }

        [ValidateNever]
        public string Image { get; set; }

        [Range(1,double.MaxValue)]
        public double Price { get; set; }

        //Added later

        public bool BestSeller { get; set; } = false;

        [Required]
        public string MainCategory { get; set; }

        public int TimesBought { get; set; } = 0;
    }
}
