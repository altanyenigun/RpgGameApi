using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RpgGameApi.Dtos.Character;
using RpgGameApi.Dtos.Skill;
using RpgGameApi.Dtos.Weapon;
using RpgGameApi.Models;

namespace RpgGameApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character,GetCharacterDto>(); // Character'den GetCharacterDto'ya dönüşüm yapılır!
            CreateMap<AddCharacterDto,Character>(); // AddCharacterDto'dan Character'e dönüşüm yapılır!
            CreateMap<UpdateCharacterDto,Character>();
            CreateMap<Weapon,GetWeaponDto>();
            CreateMap<Skill,GetSkillDto>();
        }
    }
}