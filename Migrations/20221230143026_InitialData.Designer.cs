// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using EF.Models;

#nullable disable

namespace PlatziEmtityF.Migrations
{
    [DbContext(typeof(TaskContext))]
    [Migration("20221230143026_InitialData")]
    partial class InitialData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Platzi.Models.Categoty", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Salubrity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Category", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("7b5e9399-8e95-4ae8-8745-9542a01e2cc0"),
                            Name = "Asunto domesticos",
                            Salubrity = "Insalubre"
                        },
                        new
                        {
                            Id = new Guid("0a9fa564-0604-4dfa-88df-3636fe395651"),
                            Name = "Actividad recreativa",
                            Salubrity = "sadudable y recomendable"
                        });
                });

            modelBuilder.Entity("Platzi.Models.Task", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("IdCategory")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PriorityTask")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("Id");

                    b.HasIndex("IdCategory");

                    b.ToTable("TasK", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("f5d327bf-be98-4786-81d5-0a2412b7807e"),
                            Date = new DateTime(2022, 12, 30, 11, 30, 26, 753, DateTimeKind.Local).AddTicks(6141),
                            IdCategory = new Guid("7b5e9399-8e95-4ae8-8745-9542a01e2cc0"),
                            PriorityTask = "medium",
                            Title = "Limpiar Baño"
                        },
                        new
                        {
                            Id = new Guid("629f9587-abc8-4c85-859f-acb762b754ed"),
                            Date = new DateTime(2022, 12, 30, 11, 30, 26, 753, DateTimeKind.Local).AddTicks(6157),
                            IdCategory = new Guid("0a9fa564-0604-4dfa-88df-3636fe395651"),
                            PriorityTask = "medium",
                            Title = "Practica con el arco"
                        });
                });

            modelBuilder.Entity("Platzi.Models.Task", b =>
                {
                    b.HasOne("Platzi.Models.Categoty", "Categoty")
                        .WithMany("Tasks")
                        .HasForeignKey("IdCategory")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categoty");
                });

            modelBuilder.Entity("Platzi.Models.Categoty", b =>
                {
                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}
