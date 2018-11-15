using BDSA2018.Lecture11.Entities;
using BDSA2018.Lecture11.Models;
using BDSA2018.Lecture11.Shared;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BDSA2018.Lecture11.Models.Tests
{
    public class ActorRepositoryTests
    {
        [Fact]
        public async Task CreateAsync_given_dto_creates_new_Actor()
        {
            using (var connection = await CreateConnectionAsync())
            using (var context = await CreateContextAsync(connection))
            {
                var repository = new ActorRepository(context);
                var dto = new ActorCreateUpdateDTO
                {
                    Name = "Katey Sagal"
                };

                var actor = await repository.CreateAsync(dto);

                Assert.Equal(1, actor.Id);

                var entity = await context.Actors.FindAsync(1);

                Assert.Equal("Katey Sagal", entity.Name);
            }
        }

        [Fact]
        public async Task CreateAsync_given_dto_returns_created_Actor()
        {
            using (var connection = await CreateConnectionAsync())
            using (var context = await CreateContextAsync(connection))
            {
                var repository = new ActorRepository(context);
                var dto = new ActorCreateUpdateDTO
                {
                    Name = "Katey Sagal"
                };

                var actor = await repository.CreateAsync(dto);

                Assert.Equal(1, actor.Id);
                Assert.Equal("Katey Sagal", actor.Name);
            }
        }

        [Fact]
        public async Task FindAsync_given_id_exists_returns_dto()
        {
            using (var connection = await CreateConnectionAsync())
            using (var context = await CreateContextAsync(connection))
            {
                var entity = new Actor
                {
                    Name = "Billy West",
                    Characters = new[] { new Character { Name = "Fry", Species = "Human" }, new Character { Name = "Zoidberg", Species = "Decapodian" } }
                };
                context.Actors.Add(entity);
                await context.SaveChangesAsync();

                var repository = new ActorRepository(context);

                var actor = await repository.FindAsync(1);

                Assert.Equal(1, actor.Id);
                Assert.Equal("Billy West", actor.Name);
                Assert.Equal(new Dictionary<int, string> { { 1, "Fry" }, { 2, "Zoidberg" } }, actor.Characters);
            }
        }

        [Fact]
        public async Task Read_returns_projection_of_all_actors()
        {
            using (var connection = await CreateConnectionAsync())
            using (var context = await CreateContextAsync(connection))
            {
                var entity = new Actor { Name = "Billy West" };
                context.Actors.AddRange(entity);
                await context.SaveChangesAsync();

                var repository = new ActorRepository(context);

                var actors = repository.Read();

                var actor = await actors.SingleAsync();

                Assert.Equal(1, actor.Id);
                Assert.Equal("Billy West", actor.Name);
            }
        }

        [Fact]
        public async Task UpdateAsync_given_non_existing_dto_returns_false()
        {
            using (var connection = await CreateConnectionAsync())
            using (var context = await CreateContextAsync(connection))
            {
                var repository = new ActorRepository(context);
                var dto = new ActorCreateUpdateDTO
                {
                    Id = 0,
                    Name = "Lauren Tom"
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

                var repository = new ActorRepository(context);
                var dto = new ActorCreateUpdateDTO
                {
                    Id = 1,
                    Name = "Billy West"
                };

                var updated = await repository.UpdateAsync(dto);

                Assert.True(updated);

                var entity = await context.Actors.FindAsync(1);

                Assert.Equal("Billy West", entity.Name);
            }
        }

        [Fact]
        public async Task DeleteAsync_given_existing_actorId_deletes_and_returns_true()
        {
            using (var connection = await CreateConnectionAsync())
            using (var context = await CreateContextAsync(connection))
            {
                var entity = new Actor { Name = "Lauren Tom" };
                context.Actors.Add(entity);
                await context.SaveChangesAsync();

                var id = entity.Id;

                var repository = new ActorRepository(context);

                var deleted = await repository.DeleteAsync(id);

                Assert.True(deleted);
            }
        }

        [Fact]
        public async Task DeleteAsync_given_non_existing_actorId_returns_false()
        {
            using (var connection = await CreateConnectionAsync())
            using (var context = await CreateContextAsync(connection))
            {
                var repository = new ActorRepository(context);

                var deleted = await repository.DeleteAsync(42);

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
