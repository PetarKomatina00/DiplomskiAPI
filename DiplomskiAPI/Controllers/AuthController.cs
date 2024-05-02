using DiplomskiAPI.Data;
using DiplomskiAPI.Model;
using DiplomskiAPI.Model.DTO;
using DiplomskiAPI.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace DiplomskiAPI.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;
        private APIResponse _response;
        private string _secretKey;

        //Helper metode za dodavanje uloga
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthController(
            ApplicationDbContext context,
            APIResponse response,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)

        {
            _context = context;
            _response = response;
            _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _userManager = userManager;
            _roleManager = roleManager;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            ApplicationUser userFromDB = _context.Korisnici.FirstOrDefault(x => x.UserName.ToLower() == model.UserName.ToLower());
            if (userFromDB != null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("UserName already exist");
                return BadRequest();
            }
            ApplicationUser newUser = new()
            {
                UserName = model.UserName,
                Email = model.UserName,
                NormalizedEmail = model.UserName.ToUpper(),
                Ime = model.Name,
                Prezime = model.Prezime,
                Steps = Convert.ToInt32(model.Steps),
            };
            try
            {
                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync(SD.Admin_Role).GetAwaiter().GetResult())
                    {
                        //Ako uloge ne postoje napravi ih
                        await _roleManager.CreateAsync(new IdentityRole(SD.Admin_Role));
                        await _roleManager.CreateAsync(new IdentityRole(SD.Customer_Role));
                    }
                    if (model.Role.ToLower() == SD.Admin_Role)
                    {
                        await _userManager.AddToRoleAsync(newUser, SD.Admin_Role);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(newUser, SD.Customer_Role);
                    }
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.IsSuccess = true;
                    return Ok(_response);
                }
            }
            catch (Exception ex)
            {

            }
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.ErrorMessages.Add("Error while register");
            return Ok(_response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            ApplicationUser userFromDB = _context.Korisnici.FirstOrDefault(x => x.UserName.ToLower() == model.UserName.ToLower());

            //Password u bazi je hesiran
            //Proveravamo da li se sifra modela poklapa sa sifrom u bazi.
            bool isValid = await _userManager.CheckPasswordAsync(userFromDB, model.Password);
            if (isValid == false)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(SD.UserPassInvalid);
                _response.Result = new LoginRequestDTO();
                return BadRequest(_response);
            }

            try
            {
                var roles = await _userManager.GetRolesAsync(userFromDB);
                //JWT TOKEN
                JwtSecurityTokenHandler tokenHandler = new();
                //Treba nam sada nas kljuc.
                byte[] key = Encoding.ASCII.GetBytes(_secretKey);
                //Definisemo propertije za nas token.
                SecurityTokenDescriptor tokenDescriptor = new()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    //Moze custom claim
                    new Claim("fullName", userFromDB.Ime + userFromDB.Prezime),
                    new Claim("id", userFromDB.Id.ToString()),
                    //Moze i build in claim
                    new Claim(ClaimTypes.Email, userFromDB.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                    }),
                    //Koliko dugo vazi token
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                };

                //Generisemo token
                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);


                LoginResponseDTO loginResponse = new()
                {
                    Email = userFromDB.Email,
                    Token = tokenHandler.WriteToken(token)
                };
                if (loginResponse.Email == null || string.IsNullOrEmpty(loginResponse.Token))
                {

                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add(SD.UserPassInvalid);
                    _response.Result = null;
                    return BadRequest(_response);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = loginResponse;
            }
            catch (Exception e)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(e.Message);
            }
            return Ok(_response);
        }

        [HttpPut("Steps/{UserName}/{Steps}")]
        public async Task<IActionResult> UpdateSteps(string UserName, int Steps)
        {
            ApplicationUser userFromDB = _context.Korisnici.FirstOrDefault(x => x.UserName.ToLower() == UserName.ToLower());
            if(userFromDB == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("User does not exist.");
                return BadRequest(_response);
            }
            else
            {
                try
                {
                    userFromDB.Steps = Steps;
                    _context.Update(userFromDB);
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add(e.Message);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = userFromDB;
                return Ok(_response);
            }
        }


        [HttpGet("{userName}")]
        public async Task<IActionResult> GetSteps(string userName)
        {
            var userFromDB = _context.Korisnici.FirstOrDefault(x => x.UserName.ToLower() == userName.ToLower());
            if(userFromDB == null)
            {
                _response.IsSuccess = false;
                _response.Result = NotFound();
                _response.ErrorMessages.Add("Nije pronadjen konkretan korisnik");
            }
            else
            {
                _response.Result = userFromDB.Steps;
                _response.IsSuccess = true;
                
            }
            return Ok(_response);
        }
    }
}
