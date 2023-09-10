using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RpgGameApi.Models;

namespace RpgGameApi.Data
{
    public interface IAuthRepository
    {
        ServiceResponse<int> Register(User user, string password);
        ServiceResponse<string> Login(string username,string password);
        bool UserExist(string username);
    }
}