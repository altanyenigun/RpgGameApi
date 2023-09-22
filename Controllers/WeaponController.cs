using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpgGameApi.Dtos.Character;
using RpgGameApi.Dtos.Weapon;
using RpgGameApi.Models;
using RpgGameApi.Services.WeaponService;

namespace RpgGameApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeaponController : ControllerBase
    {
        private readonly IWeaponService _weaponService;
        public WeaponController(IWeaponService weaponService)
        {
            _weaponService = weaponService;
        }

        [HttpPost]
        public ActionResult<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
        {
            return Ok(_weaponService.AddWeapon(newWeapon));
        }
    }
}