using BDSA2018.Lecture11.Entities;
using BDSA2018.Lecture11.Shared;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BDSA2018.Lecture11.Models.Tests
{
    public class CharacterRepositoryTests
    {
        [Fact]
        public async Task CreateAsync_given_dto_creates_new_Character()
        {
            using (var connection = await CreateConnectionAsync())
            using (var context = await CreateContextAsync(connection))
            {
                context.Actors.Add(new Actor { Name = "John DiMaggio" });
                await context.SaveChangesAsync();

                var repository = new CharacterRepository(context);
                var dto = new CharacterCreateUpdateDTO
                {
                    ActorId = 1,
                    Name = "Bender",
                    Species = "Robot",
                    Planet = "Earth"
                };

                var character = await repository.CreateAsync(dto);

                Assert.Equal(1, character.Id);

                var entity = await context.Characters.FindAsync(1);

                Assert.Equal(1, entity.ActorId);
                Assert.Equal("Bender", entity.Name);
                Assert.Equal("Robot", entity.Species);
                Assert.Equal("Earth", entity.Planet);
            }
        }

        [Fact]
        public async Task CreateAsync_given_dto_returns_created_Character()
        {
            using (var connection = await CreateConnectionAsync())
            using (var context = await CreateContextAsync(connection))
            {
                context.Actors.Add(new Actor { Name = "John DiMaggio" });
                await context.SaveChangesAsync();

                var repository = new CharacterRepository(context);
                var dto = new CharacterCreateUpdateDTO
                {
                    ActorId = 1,
                    Name = "Bender",
                    Species = "Robot",
                    Planet = "Earth"
                };

                var character = await repository.CreateAsync(dto);

                Assert.Equal(1, character.Id);
                Assert.Equal(1, character.ActorId);
                Assert.Equal("Bender", character.Name);
                Assert.Equal("Robot", character.Species);
                Assert.Equal("Earth", character.Planet);
            }
        }

        [Fact]
        public async Task FindAsync_given_id_exists_returns_dto()
        {
            using (var connection = await CreateConnectionAsync())
            using (var context = await CreateContextAsync(connection))
            {
                var entity = new Character
                {
                    Name = "Fry",
                    Species = "Human",
                    Planet = "Earth",
                    Actor = new Actor { Name = "Billy West" },
                    EpisodeCharacters = new[]
                    {
                        new EpisodeCharacter { Episode = new Episode { Title = "Space Pilot 3000" } },
                        new EpisodeCharacter { Episode = new Episode { Title = "The Series Has Landed" } }
                    }
                };
                context.Characters.Add(entity);
                await context.SaveChangesAsync();

                var repository = new CharacterRepository(context);

                var character = await repository.FindAsync(1);

                Assert.Equal(1, character.Id);
                Assert.Equal("Fry", character.Name);
                Assert.Equal("Human", character.Species);
                Assert.Equal("Earth", character.Planet);
                Assert.Equal(1, character.ActorId);
                Assert.Equal("Billy West", character.ActorName);
                Assert.Equal(2, character.NumberOfEpisodes);
            }
        }

        [Fact]
        public async Task Read_returns_projection_of_all_characters()
        {
            using (var connection = await CreateConnectionAsync())
            using (var context = await CreateContextAsync(connection))
            {
                var episode1 = new Episode { Title = "Space Pilot 3000" };
                var episode2 = new Episode { Title = "The Series Has Landed" };
                context.Episodes.AddRange(episode1, episode2);

                var entity = new Character
                {
                    Name = "Fry",
                    Species = "Human",
                    Planet = "Earth",
                    Actor = new Actor { Name = "Billy West" },
                    EpisodeCharacters = new[]
                    {
                        new EpisodeCharacter { Episode = new Episode { Title = "Space Pilot 3000" } },
                        new EpisodeCharacter { Episode = new Episode { Title = "The Series Has Landed" } }
                    }
                };
                context.Characters.Add(entity);
                await context.SaveChangesAsync();

                var repository = new CharacterRepository(context);

                var characters = repository.Read();

                var character = characters.First();

                Assert.Equal(1, character.Id);
                Assert.Equal("Fry", character.Name);
                Assert.Equal("Human", character.Species);
                Assert.Equal("Earth", character.Planet);
                Assert.Equal(1, character.ActorId);
                Assert.Equal("Billy West", character.ActorName);
                Assert.Equal(2, character.NumberOfEpisodes);
            }
        }

        [Fact]
        public async Task UpdateAsync_given_non_existing_dto_returns_false()
        {
            using (var connection = await CreateConnectionAsync())
            using (var context = await CreateContextAsync(connection))
            {
                var repository = new CharacterRepository(context);
                var dto = new CharacterCreateUpdateDTO
                {
                    Id = 0,
                    Name = "Bender",
                    Species = "Robot",
                    Planet = "Earth"
                };

                var updated = await repository.UpdateAsync(dto);

                Assert.False(updated);
            }
        }

        [Fact]
        public async Task UpdateAsync_given_existing_dto_updates_entity()
        {
            using (var connection = await CreateConnectionAsync())
            using (var context = await CreateContextAsync(connection))
            {
                context.Actors.Add(new Actor { Name = "John DiMaggio" });
                await context.SaveChangesAsync();

                context.Characters.Add(new Character { Name = "Fry", Species = "Human" });
                await context.SaveChangesAsync();

                var repository = new CharacterRepository(context);
                var dto = new CharacterCreateUpdateDTO
                {
                    Id = 1,
                    ActorId = 1,
                    Name = "Bender",
                    Species = "Robot",
                    Planet = "Earth"
                };

                var updated = await repository.UpdateAsync(dto);

                Assert.True(updated);

                var entity = await context.Characters.FindAsync(1);

                Assert.Equal(1, entity.ActorId);
                Assert.Equal("Bender", entity.Name);
                Assert.Equal("Robot", entity.Species);
                Assert.Equal("Earth", entity.Planet);
            }
        }

        [Fact]
        public async Task DeleteAsync_given_existing_characterId_deletes_and_returns_true()
        {
            using (var connection = await CreateConnectionAsync())
            using (var context = await CreateContextAsync(connection))
            {
                var entity = new Character { Name = "Fry", Species = "Human", Planet = "Earth" };
                context.Characters.Add(entity);
                await context.SaveChangesAsync();

                var id = entity.Id;

                var repository = new CharacterRepository(context);

                var deleted = await repository.DeleteAsync(id);

                Assert.True(deleted);
            }
        }

        [Fact]
        public async Task DeleteAsync_given_non_existing_characterId_returns_false()
        {
            using (var connection = await CreateConnectionAsync())
            using (var context = await CreateContextAsync(connection))
            {
                var repository = new CharacterRepository(context);

                var deleted = await repository.DeleteAsync(0);

                Assert.False(deleted);
            }
        }

        private async Task<DbConnection> CreateConnectionAsync()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            return connection;
        }

        private async Task<IFuturamaContext> CreateContextAsync(DbConnection connection)
        {
            var builder = new DbContextOptionsBuilder<FuturamaContext>()
                              .UseSqlite(connection);

            var context = new FuturamaTestContext(builder.Options);
            await context.Database.EnsureCreatedAsync();

            return context;
        }
    }
}
