using System;
using System.Collections.Generic;
using Xunit;

namespace BDSA2018.Lecture11.Entities.Tests
{
    public class EpisodeTests
    {
        [Fact]
        public void EpisodeCharacters_is_HashSet_of_EpisodeCharacter()
        {
            var episode = new Episode();

            Assert.IsType<HashSet<EpisodeCharacter>>(episode.EpisodeCharacters);
        }
    }
}
