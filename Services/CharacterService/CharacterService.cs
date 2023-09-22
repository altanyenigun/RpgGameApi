using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RpgGameApi.Data;
using RpgGameApi.Dtos.Character;
using RpgGameApi.Models;

namespace RpgGameApi.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public ServiceResponse<List<GetCharacterDto>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var character = _mapper.Map<Character>(newCharacter); // newCharacter(AddCharacterDto) objesini Character objesine mapliyor.

            character.User = _context.Users.FirstOrDefault(u => u.Id == GetUserId());

            _context.Characters.Add(character);
            _context.SaveChanges();
            serviceResponse.Data = _context.Characters.Where(c => c.User!.Id == GetUserId()).Select(c => _mapper.Map<GetCharacterDto>(c)).ToList(); // birden fazla character olabileceği için tek tek hepsini GetCharacterDto'ya mapledik, en sonunda da listeye dönüştürdük.
            return serviceResponse;
        }

        public ServiceResponse<List<GetCharacterDto>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                var character = _context.Characters.FirstOrDefault(c => c.Id == id && c.User!.Id == GetUserId());
                if (character is null)
                    throw new Exception($"Character with ID '{id}' not found.");

                _context.Characters.Remove(character);
                _context.SaveChanges();
                serviceResponse.Data = _context.Characters.Where(c => c.User!.Id == GetUserId()).Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public ServiceResponse<List<GetCharacterDto>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = _context.Characters.Include(c=>c.Weapon).Include(c=>c.Skills).Where(c => c.User!.Id == GetUserId()).ToList(); // sadece kullaniciya ait karakterleri getirme
            serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList(); // select inumarable döner, ancak biz list istediğimiz için en sonda tolist kullanıyoruz.
            return serviceResponse;
        }

        public ServiceResponse<GetCharacterDto> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var dbCharacter = _context.Characters.Include(c=>c.Weapon).Include(c=>c.Skills).FirstOrDefault(c => c.Id == id && c.User!.Id == GetUserId());
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter); //character objesini GetCharacterDto objesine mapleme işlemi
            return serviceResponse;
        }

        public ServiceResponse<GetCharacterDto> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = _context.Characters.Include(c => c.User).FirstOrDefault(c => c.Id == updatedCharacter.Id);
                if (character is null || character.User!.Id != GetUserId())
                    throw new Exception($"Character with ID '{updatedCharacter.Id}' not found.");

                _mapper.Map(updatedCharacter, character); // updatedCharacter objesini character objesine maple

                //without mapper
                // character.Name = updatedCharacter.Name;
                // character.HitPoints = updatedCharacter.HitPoints;
                // character.Strength = updatedCharacter.Strength;
                // character.Defense = updatedCharacter.Defense;
                // character.Intelligence = updatedCharacter.Intelligence;
                // character.Class = updatedCharacter.Class;
                _context.SaveChanges();
                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public ServiceResponse<GetCharacterDto> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = _context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .FirstOrDefault(c => c.Id == newCharacterSkill.CharacterId && c.User!.Id == GetUserId());
                
                if(character is null)
                {
                    response.Success = false;
                    response.Message = "Character not found.";
                    return response;
                }

                var skill = _context.Skills.FirstOrDefault(s=>s.Id == newCharacterSkill.SkillId);

                if(skill is null)
                {
                    response.Success = false;
                    response.Message = "Skill not found.";
                    return response;
                }

                character.Skills!.Add(skill);
                _context.SaveChanges();
                response.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}