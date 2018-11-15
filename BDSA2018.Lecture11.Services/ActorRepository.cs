using System.Linq;
using System.Threading.Tasks;
using BDSA2018.Lecture11.Entities;
using BDSA2018.Lecture11.Shared;
using Microsoft.EntityFrameworkCore;

namespace BDSA2018.Lecture11.Models
{
    public class ActorRepository : IActorRepository
    {
        private readonly IFuturamaContext _context;

        public ActorRepository(IFuturamaContext context)
        {
            _context = context;
        }

        public async Task<ActorDetailedDTO> CreateAsync(ActorCreateUpdateDTO actor)
        {
            var entity = new Actor
            {
                Name = actor.Name
            };

            _context.Actors.Add(entity);
            await _context.SaveChangesAsync();

            return await FindAsync(entity.Id);
        }

        public async Task<ActorDetailedDTO> FindAsync(int actorId)
        {
            var entities = from a in _context.Actors
                           where a.Id == actorId
                           select new ActorDetailedDTO
                           {
                               Id = a.Id,
                               Name = a.Name,
                               Characters = a.Characters.ToDictionary(c => c.Id, c => c.Name)
                           };

            return await entities.FirstOrDefaultAsync();
        }

        public IQueryable<ActorDTO> Read()
        {
            var entities = from a in _context.Actors
                           select new ActorDTO
                           {
                               Id = a.Id,
                               Name = a.Name
                           };

            return entities;
        }

        public async Task<bool> UpdateAsync(ActorCreateUpdateDTO actor)
        {
            var entity = await _context.Actors.FindAsync(actor.Id);

            if (entity == null)
            {
                return false;
            }

            entity.Name = actor.Name;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int actorId)
        {
            var entity = await _context.Actors.FindAsync(actorId);

            if (entity == null)
            {
                return false;
            }

            _context.Actors.Remove(entity);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
