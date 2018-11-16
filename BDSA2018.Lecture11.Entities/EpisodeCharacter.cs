namespace BDSA2018.Lecture11.Entities
{
    public class EpisodeCharacter
    {
        public int CharacterId { get; set; }

        public int EpisodeId { get; set; }

        public Character Character { get; set; }

        public Episode Episode { get; set; }
    }
}
