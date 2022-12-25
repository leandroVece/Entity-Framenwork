using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Platzi.Models
{
    class TaskContext : DbContext
    {

        public DbSet<Categoty> Categoties { get; set; }

        public DbSet<Task> Tasks { get; set; }

        public TaskContext(DbContextOptions<TaskContext> options) : base(options) { }

    }
}