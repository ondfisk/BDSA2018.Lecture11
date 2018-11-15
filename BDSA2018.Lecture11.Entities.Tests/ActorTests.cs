using System;
using System.Collections.Generic;
using Xunit;

namespace BDSA2018.Lecture11.Entities.Tests
{
    public class ActorTests
    {
        [Fact]
        public void Characters_is_HashSet_of_Character()
        {
            var actor = new Actor();

            var characters = actor.Characters as HashSet<Character>;

            Assert.NotNull(characters);
        }
    }
}
