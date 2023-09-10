using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RpgGameApi.Models;

namespace RpgGameApi.Services.CharacterService
{
    public interface ICharacterService
    {
        ServiceResponse<List<Character>> GetAllCharacters();
        ServiceResponse<Character> GetCharacterById(int id);
        ServiceResponse<List<Character>> AddCharacter(Character newCharacter);
    }
}