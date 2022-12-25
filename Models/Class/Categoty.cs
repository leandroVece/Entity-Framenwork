using System.ComponentModel.DataAnnotations;

namespace Platzi.Models
{

    public class Categoty
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}


