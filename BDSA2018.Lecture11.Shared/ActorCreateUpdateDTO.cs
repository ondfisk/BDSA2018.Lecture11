using System.ComponentModel.DataAnnotations;

namespace BDSA2018.Lecture11.Shared
{
    public class ActorCreateUpdateDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
