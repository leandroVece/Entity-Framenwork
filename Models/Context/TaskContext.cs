using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Platzi.Models
{
    class TaskContext : DbContext
    {

        public DbSet<Categoty> Categoties { get; set; }

        public DbSet<Task> Tasks { get; set; }

        public TaskContext(DbContextOptions<TaskContext> options) : base(options) { }

        //con esto puedo crear una base de datos con especificaciones mas detalladas que con las propiedades de atributos
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoty>(categoty =>
            {
                categoty.ToTable("Category");
                categoty.HasKey(p => p.Id);

                categoty.Property(p => p.Name).IsRequired().HasMaxLength(150);
                categoty.Property(p => p.Description);
            });

            modelBuilder.Entity<Task>(task =>
            {
                task.ToTable("TasK");
                task.HasKey(p => p.Id);

                //aqui le digo a fluet que cree una base que tenga una relacion de unos a muchos y que la clave foranea va a ser idCategory
                task.HasOne(p => p.Categoty).WithMany(p => p.Tasks).HasForeignKey(p => p.IdCategory);

                task.Property(p => p.Title).IsRequired().HasMaxLength(150);
                task.Property(p => p.Description);
                task.Property(p => p.Date);
                task.Property(p => p.PriorityTask).HasConversion(x => x.ToString(), x => (Priority)Enum.Parse(typeof(Priority), x));
                task.Ignore(p => p.summary);

            });
        }

    }
}