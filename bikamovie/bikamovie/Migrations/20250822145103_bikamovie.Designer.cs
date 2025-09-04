using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using bikamovie.Models;

#nullable disable

namespace bikamovie.Migrations
{
    [DbContext(typeof(FilmDbContext))]
    [Migration("20250822145103_bikamovie")]
    partial class bikamovie : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {

        }

        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.8");

            modelBuilder.Entity("User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("bikamovie.Models.Film", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Ad")
                        .HasColumnType("TEXT");

                    b.Property<int>("CikisYili")
                        .HasColumnType("INTEGER");

                    b.Property<string>("GorselUrl")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("IMDb")
                        .HasColumnType("TEXT");

                    b.Property<string>("Konu")
                        .HasColumnType("TEXT");

                    b.Property<float?>("Sure")
                        .HasColumnType("REAL");

                    b.Property<string>("Tur")
                        .HasColumnType("TEXT");

                    b.Property<string>("Yonetmen")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Filmler");
                });
#pragma warning restore 612, 618
        }
    }
}
