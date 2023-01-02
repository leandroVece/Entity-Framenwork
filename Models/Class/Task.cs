using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platzi.Models
{

    public class Task
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("IdCategory")]
        public Guid IdCategory { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }
        public Priority PriorityTask { get; set; }
        public DateTime Date { get; set; }

        public virtual Categoty Categoty { get; set; }
        [NotMapped]
        public string summary { get; set; }



    }

    public enum Priority
    {
        low,
        medium,
        high
    }
}

