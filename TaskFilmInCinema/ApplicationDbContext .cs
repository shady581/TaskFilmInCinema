using Microsoft.EntityFrameworkCore;
using TaskFilmInCinema.Models;


    namespace TaskFilmInCinema
    {
        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            }

            public DbSet<Category> Categories { get; set; }
            public DbSet<Movie> Movies { get; set; }
            public DbSet<Actor> Actors { get; set; }
            public DbSet<Cinema> Cinemas { get; set; }
            public DbSet<MovieActor> MovieActors { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<MovieActor>()
                    .HasKey(ma => new { ma.Mov_Id, ma.Act_Id });

                modelBuilder.Entity<MovieActor>()
                    .HasOne(ma => ma.Movie)
                    .WithMany(m => m.MovieActors)
                    .HasForeignKey(ma => ma.Mov_Id);

                modelBuilder.Entity<MovieActor>()
                    .HasOne(ma => ma.Actor)
                    .WithMany(a => a.MovieActors)
                    .HasForeignKey(ma => ma.Act_Id);
            }
        }
    }
