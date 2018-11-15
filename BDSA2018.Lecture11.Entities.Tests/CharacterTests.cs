using System;
using System.Collections.Generic;
using Xunit;

namespace BDSA2018.Lecture11.Entities.Tests
{
    public class CharacterTests
    {
        [Fact]
        public void EpisodeCharacters_is_HashSet_of_EpisodeCharacter()
        {
            var character = new Character();

            Assert.IsType<HashSet<EpisodeCharacter>>(character.EpisodeCharacters);
        }
    }
}
