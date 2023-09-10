using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RpgGameApi.Data;
using RpgGameApi.Dtos.User;
using RpgGameApi.Models;

namespace RpgGameApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo; // AuthRepository injected
        }

        [HttpPost("Register")]
        public ActionResult<ServiceResponse<int>> Register(UserRegisterDto request)
        {
            var response = _authRepo.Register(
                new User { Username = request.Username}, request.Password
            );

            if(!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}