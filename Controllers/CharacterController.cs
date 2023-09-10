using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RpgGameApi.Dtos.Character;
using RpgGameApi.Models;
using RpgGameApi.Services.CharacterService;

namespace RpgGameApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService; // CharacterService'i controllera inject etmiş olduk.
        }

        [HttpGet("GetAll")]
        //[Route("GetAll")]
        public ActionResult<ServiceResponse<List<GetCharacterDto>>> Get()
        {
            return Ok(_characterService.GetAllCharacters());
        }

        [HttpGet("{id}")]
        public ActionResult<ServiceResponse<List<GetCharacterDto>>> GetSingle(int id)
        {
            return Ok(_characterService.GetCharacterById(id));
        }

        [HttpPost]
        public ActionResult<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            return Ok(_characterService.AddCharacter(newCharacter));
        }

        [HttpPut]
        public ActionResult<ServiceResponse<List<GetCharacterDto>>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var response = _characterService.UpdateCharacter(updatedCharacter);
            if(response.Data is null)
                return NotFound(response);
            return Ok(response);
        }
    }
}