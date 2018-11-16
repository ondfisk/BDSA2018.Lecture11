using BDSA2018.Lecture11.Entities;
using Microsoft.EntityFrameworkCore;

namespace BDSA2018.Lecture11.Services.Tests
{
    public class FuturamaTestContext : FuturamaContext
    {
        public FuturamaTestContext(DbContextOptions<FuturamaContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EpisodeCharacter>()
               .HasKey(e => new { e.EpisodeId, e.CharacterId });
        }
    }
}
