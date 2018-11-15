using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BDSA2018.Lecture11.Entities
{
    public partial class Actor
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public ICollection<Character> Characters { get; set; }

        public Actor()
        {
            Characters = new HashSet<Character>();
        }
    }
}
