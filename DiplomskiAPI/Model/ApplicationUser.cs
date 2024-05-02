using Microsoft.AspNetCore.Identity;

namespace DiplomskiAPI.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }

        override
        public string Email { get; set; }

        public int Steps { get; set; } = 0;

    }
}
