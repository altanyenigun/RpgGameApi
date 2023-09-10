using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RpgGameApi.Models;

namespace RpgGameApi.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character>{
            new Character(), // default olarak tanımladığımız tüm özellikleri alır.
            new Character{ Id=1, Name = "Sam"} // default olarak verdiğimiz değerlerden sadece Name'i değiştirdik.
        };
        public ServiceResponse<List<Character>> AddCharacter(Character newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<Character>>();
            characters.Add(newCharacter);
            serviceResponse.Data = characters;
            return serviceResponse;
        }

        public ServiceResponse<List<Character>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<Character>>();
            serviceResponse.Data = characters;
            return serviceResponse;
        }

        public ServiceResponse<Character> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<Character>();
            var character = characters.FirstOrDefault(c => c.Id == id);
            serviceResponse.Data=character;
            if(character is null)
                serviceResponse.Message="Böyle bir idye sahip kayıt yok";
                serviceResponse.Sucess=false;
            return serviceResponse;
        }
    }
}