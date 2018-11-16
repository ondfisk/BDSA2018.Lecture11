using BDSA2018.Lecture11.Services;
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
    public class ActorsController : ControllerBase
    {
        private readonly IActorRepository _repository;

        public ActorsController(IActorRepository repository)
        {
            _repository = repository;
        }

        // GET: api/actors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActorDTO>>> Get()
        {
            return await _repository.Read().ToListAsync();
        }

        // GET: api/actors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ActorDetailedDTO>> Get(int id)
        {
            var actor = await _repository.FindAsync(id);

            if (actor == null)
            {
                return NotFound();
            }

            return actor;
        }

        // POST: api/actors
        [HttpPost]
        public async Task<ActionResult<ActorDetailedDTO>> Post([FromBody] ActorCreateUpdateDTO actor)
        {
            var created = await _repository.CreateAsync(actor);

            return CreatedAtAction(nameof(Get), new { created.Id }, created);
        }

        // PUT: api/actors/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ActorCreateUpdateDTO actor)
        {
            var result = await _repository.UpdateAsync(actor);

            if (result)
            {
                return NoContent();
            }

            return NotFound();
        }

        // DELETE: api/actors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _repository.DeleteAsync(id);

            if (result)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
