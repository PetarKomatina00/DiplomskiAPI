using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace DiplomskiAPI.Model.DTO
{
    public class LekUpdateDTO
    {
        [Key]
        public int LekID { get; set; }
        public string NazivLeka { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ISBN { get; set; }

        [ValidateNever]
        public IFormFile Image { get; set; }

        [Range(1, double.MaxValue)]
        public double Price { get; set; }
    }
}
