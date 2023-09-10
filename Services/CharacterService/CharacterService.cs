using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RpgGameApi.Dtos.Character;
using RpgGameApi.Models;

namespace RpgGameApi.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character>{
            new Character(), // default olarak tanımladığımız tüm özellikleri alır.
            new Character{ Id=1, Name = "Sam"} // default olarak verdiğimiz değerlerden sadece Name'i değiştirdik.
        };

        private readonly IMapper _mapper;

        public CharacterService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ServiceResponse<List<GetCharacterDto>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var character = _mapper.Map<Character>(newCharacter); // newCharacter(AddCharacterDto) objesini Character objesine mapliyor.
            character.Id = characters.Max(c => c.Id) + 1;
            characters.Add(character);
            serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList(); // birden fazla character olabileceği için tek tek hepsini GetCharacterDto'ya mapledik, en sonunda da listeye dönüştürdük.
            return serviceResponse;
        }

        public ServiceResponse<List<GetCharacterDto>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList(); // select inumarable döner, ancak biz list istediğimiz için en sonda tolist kullanıyoruz.
            return serviceResponse;
        }

        public ServiceResponse<GetCharacterDto> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var character = characters.FirstOrDefault(c => c.Id == id);
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character); //character objesini GetCharacterDto objesine mapleme işlemi
            return serviceResponse;
        }

        public ServiceResponse<GetCharacterDto> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = characters.FirstOrDefault(c => c.Id == updatedCharacter.Id);
                if(character is null)
                    throw new Exception($"Character with ID '{updatedCharacter.Id}' not found.");
                
                _mapper.Map(updatedCharacter,character); // updatedCharacter objesini character objesine maple

                //without mapper
                // character.Name = updatedCharacter.Name;
                // character.HitPoints = updatedCharacter.HitPoints;
                // character.Strength = updatedCharacter.Strength;
                // character.Defense = updatedCharacter.Defense;
                // character.Intelligence = updatedCharacter.Intelligence;
                // character.Class = updatedCharacter.Class;

                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            }catch(Exception ex)
            {
                serviceResponse.Success=false;
                serviceResponse.Message=ex.Message;
            }
            return serviceResponse;
        }
    }
}