using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RpgGameApi.Dtos.Character;
using RpgGameApi.Models;

namespace RpgGameApi.Services.CharacterService
{
    public interface ICharacterService
    {
        ServiceResponse<List<GetCharacterDto>> GetAllCharacters();
        ServiceResponse<GetCharacterDto> GetCharacterById(int id);
        ServiceResponse<List<GetCharacterDto>> AddCharacter(AddCharacterDto newCharacter);
        ServiceResponse<GetCharacterDto> UpdateCharacter (UpdateCharacterDto updatedCharacter);
        ServiceResponse<List<GetCharacterDto>> DeleteCharacter(int id);
    }
}