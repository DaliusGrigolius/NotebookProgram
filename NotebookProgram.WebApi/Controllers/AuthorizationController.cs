using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IConfiguration _configuration;

        public AuthorizationController(IConfiguration configuration, NotebookDbContext context)
        {
            _configuration = configuration;
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

            var user = _context.Users
                .Include(i => i.RefreshTokens)
                .FirstOrDefault(u => u.Username == request.Username);

            if (!PasswordHashIsVerified(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong username or/and password");
            }

            string token = CreateToken(user);

            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken, user);

            return Ok(new { Token = token });
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Created = DateTime.Now,
                Expires = DateTime.Now.AddMinutes(15),
            };

            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newRefreshToken, User user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            user.RefreshTokens.Add(newRefreshToken);
            _context.RefreshTokens.Add(newRefreshToken);
            _context.SaveChanges();
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
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return tokenString;
        }
    }
}
