using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RpgGameApi.Dtos.Character;
using RpgGameApi.Dtos.Weapon;
using RpgGameApi.Models;

namespace RpgGameApi.Services.WeaponService
{
    public interface IWeaponService
    {
        ServiceResponse<GetCharacterDto> AddWeapon(AddWeaponDto newWeapon);
    }
}