using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NotebookProgram.Repository.DbContexts;
using NotebookProgram.Repository.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace NotebookProgram.WebApi.Controllers
{
    [Route("api/authorization")]
    [ApiController]
    public class AuthorizationController : Controller
    {
        private readonly NotebookDbContext _context;

        public AuthorizationController(NotebookDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register(UserDto request)
        {
            if (NameIsTaken(request.Username))
            {
                return BadRequest("Name is taken. Choose another one.");
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = new User(request.Username, passwordHash, passwordSalt);
            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("Success: registration completed.");
        }

        [HttpPost, Route("login")]
        public IActionResult Login(UserDto request)
        {
            if (!NameIsTaken(request.Username))
            {
                return BadRequest("Wrong username or/and password");
            }

            var user = _context.Users.Where(u => u.Username == request.Username).FirstOrDefault();

            if (!PasswordHashIsVerified(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong username or/and password");
            }

            string token = CreateToken(user);

            return Ok(new { Token = token });
            //------------------------------------------------------
            //if (model == null)
            //{
            //    return BadRequest("Invalid client request");
            //}
            //if (model.Username == "string" && model.Password == "string")
            //{
            //var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@2410"));
            //var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            //var tokenOptions = new JwtSecurityToken(
            //    issuer: "guest",
            //    audience: "https://localhost:5090",
            //    claims: new List<Claim>(),
            //    expires: DateTime.Now.AddMinutes(5),
            //    signingCredentials: signinCredentials
            //);
            //var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            //return Ok(new { Token = tokenString });
            //}
            //else
            //{
            //    return Unauthorized();
            //}
            //------------------------------------------------------
        }

        private bool NameIsTaken(string username)
        {
            return _context.Users.Where(u => u.Username == username).ToList().Count > 0;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool PasswordHashIsVerified(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            };
        }

        private string CreateToken(User user)
        {

            return "";
        }
    }
}
