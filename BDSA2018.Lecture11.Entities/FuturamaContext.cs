using Microsoft.EntityFrameworkCore;

namespace BDSA2018.Lecture11.Entities
{
    public class FuturamaContext : DbContext, IFuturamaContext
    {
        public virtual DbSet<Actor> Actors { get; set; }
        public virtual DbSet<Character> Characters { get; set; }
        public virtual DbSet<Episode> Episodes { get; set; }
        public virtual DbSet<EpisodeCharacter> EpisodeCharacters { get; set; }

        public FuturamaContext(DbContextOptions<FuturamaContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EpisodeCharacter>()
                .HasKey(e => new { e.EpisodeId, e.CharacterId });

            modelBuilder.Entity<Actor>().HasData(
                new Actor { Id = 1, Name = "Billy West" },
                new Actor { Id = 2, Name = "Katey Sagal" },
                new Actor { Id = 3, Name = "John DiMaggio" },
                new Actor { Id = 4, Name = "Lauren Tom" },
                new Actor { Id = 5, Name = "Phil LaMarr" }
            );

            modelBuilder.Entity<Character>().HasData(
                new Character { Id = 1, ActorId = 1, Name = "Philip J. Fry", Species = "Human", Planet = "Earth" },
                new Character { Id = 2, ActorId = 2, Name = "Leela Turanga", Species = "Mutant, Human", Planet = "Earth" },
                new Character { Id = 3, ActorId = 3, Name = "Bender Bending Rodriguez", Species = "Robot", Planet = "Earth" },
                new Character { Id = 4, ActorId = 1, Name = "John A. Zoidberg", Species = "Decapodian", Planet = "Decapod 10" },
                new Character { Id = 5, ActorId = 4, Name = "Amy Wong", Species = "Human", Planet = "Mars" },
                new Character { Id = 6, ActorId = 5, Name = "Hermes Conrad", Species = "Human", Planet = "Earth" },
                new Character { Id = 7, ActorId = 1, Name = "Hubert J. Farnsworth", Species = "Human", Planet = "Earth" }
            );
        }
    }
}
