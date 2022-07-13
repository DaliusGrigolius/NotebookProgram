using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NotebookProgram.Dto.Models;
using NotebookProgram.Repository.DbContexts;
using NotebookProgram.Repository.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace NotebookProgram.WebApi.Controllers
{
    [Route("authorization")]
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
        public async Task<IActionResult> Register(UserRegisterDto request)
        {
            if (NameIsTaken(request.Username))
            {
                return BadRequest("Error: user already exists.");
            }

            CreatePasswordHash(request.Password, 
                out byte[] passwordHash, 
                out byte[] passwordSalt);

            var newUser = new User(request.Username, passwordHash, passwordSalt);

            _context?.Users?.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok("Success: user succesfully created.");
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginDto request)
        {
            if (!NameIsTaken(request.Username))
            {
                return BadRequest("Error: wrong username or/and password");
            }

            var user = _context?.Users?
                .Include(i => i.RefreshTokens)
                .FirstOrDefault(u => u.Username == request.Username);

            if (!PasswordHashIsVerified(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Error: wrong username or/and password");
            }

            string token = CreateToken(user);

            var refreshToken = GenerateRefreshToken();
            SetRefreshTokenToCookies(refreshToken, user);

            return Ok(new { Token = token });
        }

        [HttpPost("refresh-tokens"), Authorize]
        public IActionResult RefreshTokens()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var user = _context?.Users?
                .Include(i => i.RefreshTokens)
                .FirstOrDefault(i => i.RefreshTokens
                    .Any(i => i.Token == refreshToken));

            if (user != null)
            {
                string token = CreateToken(user);
                var newRefreshToken = GenerateRefreshToken();
                SetRefreshTokenToCookies(newRefreshToken, user);

                return Ok(new { Token = token });
            }
            else
            {
                return BadRequest("Error: something went wrong.");
            }
        }

        private bool NameIsTaken(string username)
        {
            return _context.Users.Any(u => u.Username == username);
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
            var accessTokenExpiryDate = DateTime.UtcNow.AddMinutes(1);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: accessTokenExpiryDate,
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            SetAccessTokenToCookies(tokenString, accessTokenExpiryDate);

            return tokenString;
        }

        private void SetAccessTokenToCookies(string token, DateTime accessTokenExpiryDate)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = accessTokenExpiryDate,
            };

            Response.Cookies.Append("accessToken", token, cookieOptions);
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(2),
            };

            return refreshToken;
        }

        private void SetRefreshTokenToCookies(RefreshToken newRefreshToken, User user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires,
            };

            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
            AddRefreshTokenToDatabase(newRefreshToken, user);
        }

        private void AddRefreshTokenToDatabase(RefreshToken newRefreshToken, User user)
        {
            user.RefreshTokens.Add(newRefreshToken);
            _context?.RefreshTokens?.Add(newRefreshToken);
            _context?.SaveChanges();
        }
    }
}
