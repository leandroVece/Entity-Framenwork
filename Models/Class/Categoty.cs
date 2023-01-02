using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Platzi.Models
{

    public class Categoty
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public string Salubrity { get; set; }

        [JsonIgnore]
        public virtual ICollection<Task> Tasks { get; set; }
    }
}


