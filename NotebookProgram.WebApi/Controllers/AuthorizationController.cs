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
            AddRefreshTokenToDatabase(refreshToken, user);

            return Ok(new
            {
                Token = token,
                RefreshToken = new
                {
                    Token = refreshToken.Token,
                    Expires = refreshToken.Expires
                }
            });
        }

        [HttpPost("refresh-tokens")]
        public IActionResult RefreshTokens(string refreshToken)
        {
            var user = _context?.Users?
                .Include(i => i.RefreshTokens)
                .FirstOrDefault(i => i.RefreshTokens
                    .Any(i => i.Token == refreshToken));

            if (user == null)
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if (user.RefreshTokens.Last().Expires < DateTime.UtcNow)
            {
                return Unauthorized("Token expired.");
            }

            string token = CreateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            AddRefreshTokenToDatabase(newRefreshToken, user);

            return Ok(new
            {
                Token = token,
                RefreshToken = new
                {
                    Token = newRefreshToken.Token,
                    Expires = newRefreshToken.Expires
                }
            });
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
            var accessTokenExpiryDate = DateTime.UtcNow.AddDays(1);

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

            return tokenString;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(7),
            };

            return refreshToken;
        }

        private void AddRefreshTokenToDatabase(RefreshToken newRefreshToken, User user)
        {
            user.RefreshTokens.Add(newRefreshToken);
            _context?.RefreshTokens?.Add(newRefreshToken);
            _context?.SaveChanges();
        }
    }
}
