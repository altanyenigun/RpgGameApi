using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using RpgGameApi.Models;

namespace RpgGameApi.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private IEnumerable<Claim>? claims;

        public AuthRepository(DataContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }
        public ServiceResponse<string> Login(string username, string password)
        {
            var response = new ServiceResponse<string>();
            var user = _context.Users.FirstOrDefault(u=>u.Username.ToLower().Equals(username.ToLower()));
            if(user is null)
            {
                response.Success=false;
                response.Message="User not found.";
            }
            else if(!VerifyPasswordHash(password,user.PasswordHash,user.PasswordSalt))
            {
                response.Success=false;
                response.Message="Wrong Password.";
            }
            else
            {
                response.Data = CreateToken(user);
            }

            return response;

        }

        public ServiceResponse<int> Register(User user, string password)
        {
            //şifreyi plaintext olarak saklamak iyi değil.
            var response = new ServiceResponse<int>();
            if(UserExist(user.Username))
            {
                response.Success=false;
                response.Message="User Already Exists.";
                return response;
            }

            CreatePasswordHash(password,out byte[] passwordHash,out byte[] passwordSalt); // outtan dolayı returna gerek kalmadan referans üzerinden erişim sağlayabiliyoruz.
            
            user.PasswordHash = passwordHash; // outtan dolayı burada direk passwordHash çağırabildik.
            user.PasswordSalt = passwordSalt; // outtan dolayı burada direk passwordSalt çağırabildik.

            _context.Users.Add(user);
            _context.SaveChanges();

            response.Data=user.Id;
            return response;
        }

        public bool UserExist(string username)
        {
            if(_context.Users.Any(u=> u.Username.ToLower() == username.ToLower()))
                return true;
            
            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) // outlar referansi temsil ediyor.
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password,byte[] passwordHash,byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Username)
            };

            var appSettingsToken = _configuration.GetSection("appSettings:Token").Value;
            if(appSettingsToken is null){
                throw new Exception("appSettings Token is null!");
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appSettingsToken));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires =  DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}