using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RpgGameApi.Models;

namespace RpgGameApi.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        public ServiceResponse<string> Login(string username, string password)
        {
            throw new NotImplementedException();
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
    }
}