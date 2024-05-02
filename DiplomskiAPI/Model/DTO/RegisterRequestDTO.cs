namespace DiplomskiAPI.Model.DTO
{
    public class RegisterRequestDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }

        public string Prezime { get; set; }
        public string Role { get; set; }

        public int Steps { get; set; } = 0;
    }
}
