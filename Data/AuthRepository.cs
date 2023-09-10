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
                response.Data = user.Id.ToString();
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
    }
}