using BDSA2018.Lecture11.Models;
using BDSA2018.Lecture11.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BDSA2018.Lecture11.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private ICharacterRepository _repository;

        public CharactersController(ICharacterRepository repository)
        {
            _repository = repository;
        }

        // GET api/characters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CharacterDTO>>> Get()
        {
            return await _repository.Read().ToListAsync();
        }

        // GET api/characters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CharacterDTO>> Get(int id)
        {
            var character = await _repository.FindAsync(id);

            if (character == null)
            {
                return NotFound();
            }

            return character;
        }

        // POST api/characters
        [HttpPost]
        public async Task<ActionResult<CharacterDTO>> Post([FromBody] CharacterCreateUpdateDTO character)
        {
            var dto = await _repository.CreateAsync(character);

            return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }

        // PUT api/characters/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CharacterCreateUpdateDTO character)
        {
            var updated = await _repository.UpdateAsync(character);

            if (updated)
            {
                return NoContent();
            }

            return NotFound();
        }

        // DELETE api/characters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repository.DeleteAsync(id);

            if (deleted)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
