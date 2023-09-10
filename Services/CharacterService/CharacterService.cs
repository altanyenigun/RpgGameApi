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
        public List<Character> AddCharacter(Character newCharacter)
        {
            characters.Add(newCharacter);
            return characters;
        }

        public List<Character> GetAllCharacters()
        {
            return characters;
        }

        public Character GetCharacterById(int id)
        {
            var character = characters.FirstOrDefault(c => c.Id == id);
            if(character is not null)
                return  character;
            throw new Exception("Character Not Found");
        }
    }
}