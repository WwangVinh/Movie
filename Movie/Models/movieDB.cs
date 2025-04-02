using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Movie.Models;

public partial class movieDB : DbContext
{
    public movieDB()
    {
    }

    public movieDB(DbContextOptions<movieDB> options)
        : base(options)
    {
    }

    public virtual DbSet<Actor> Actors { get; set; }

    public virtual DbSet<Categories> Categories { get; set; }

    public virtual DbSet<Director> Directors { get; set; }

    public virtual DbSet<Episode> Episodes { get; set; }

    public virtual DbSet<Movies> Movies { get; set; }

    public virtual DbSet<MovieActors> MovieActor { get; set; }

    public virtual DbSet<MovieCategories> MovieCategories { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Series> Series { get; set; }

    public virtual DbSet<SeriesActors> SeriesActors { get; set; }

    public virtual DbSet<SeriesCategories> SeriesCategories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source =DESKTOP-GP36FMA;Database=movieDB;User ID=sa;Password=Djchip123@;Encrypt=false;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Actor>(entity =>
        {
            entity.HasKey(e => e.ActorId).HasName("PK__Actors__E60C94727B530F37");
        });

        modelBuilder.Entity<Categories>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__EFF907B09DB23D2A");
        });

        modelBuilder.Entity<Director>(entity =>
        {
            entity.HasKey(e => e.DirectorId).HasName("PK__Director__26C69E26795B95C7");
        });

        modelBuilder.Entity<Episode>(entity =>
        {
            entity.HasKey(e => e.EpisodeId).HasName("PK__Episodes__AC6676151E8973F9");

            entity.HasOne(d => d.Series).WithMany(p => p.Episodes).HasConstraintName("FK__Episodes__Series__49C3F6B7");
        });

        modelBuilder.Entity<Movies>(entity =>
        {
            entity.HasKey(e => e.MovieId).HasName("PK__Movie__4BD2943AD52A39F2");

            entity.Property(e => e.Status).HasDefaultValue(1);

            entity.HasOne(d => d.Director).WithMany(p => p.Movie).HasConstraintName("FK__Movie__Director__4316F928");
        });

        modelBuilder.Entity<MovieActors>(entity =>
        {
            entity.HasKey(e => new { e.MovieId, e.ActorId }).HasName("PK__MovieAct__75B25D7DE91BD012");

            entity.Property(e => e.MovieActorId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Actors).WithMany(p => p.MovieActor).HasConstraintName("FK__MovieActo__Actor__5812160E");

            entity.HasOne(d => d.Movie).WithMany(p => p.MovieActor).HasConstraintName("FK__MovieActo__Movie__571DF1D5");
        });

        modelBuilder.Entity<MovieCategories>(entity =>
        {
            entity.HasKey(e => new { e.MovieId, e.CategoryId }).HasName("PK__MovieCat__552D04414C23C869");

            entity.Property(e => e.MovieCategoryId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Categories).WithMany(p => p.MovieCategories).HasConstraintName("FK__MovieCate__Categ__778AC167");

            entity.HasOne(d => d.Movie).WithMany(p => p.MovieCategories).HasConstraintName("FK__MovieCate__Movie__76969D2E");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.SubPaymentId).HasName("PK__Payment__813806001037AB0E");

            entity.HasOne(d => d.User).WithMany(p => p.Payments).HasConstraintName("FK__Payment__UserID__4CA06362");
        });

        modelBuilder.Entity<Series>(entity =>
        {
            entity.HasKey(e => e.SeriesId).HasName("PK__Series__F3A1C101CC1C20B4");

            entity.Property(e => e.Season).HasDefaultValue(1);

            entity.HasOne(d => d.Director).WithMany(p => p.Series).HasConstraintName("FK__Series__Director__46E78A0C");
        });

        modelBuilder.Entity<SeriesActors>(entity =>
        {
            entity.HasKey(e => new { e.SeriesId, e.ActorId }).HasName("PK__SeriesAc__CDC108463B24E581");

            entity.Property(e => e.SeriesActorId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Actors).WithMany(p => p.SeriesActors).HasConstraintName("FK__SeriesAct__Actor__5BE2A6F2");

            entity.HasOne(d => d.Series).WithMany(p => p.SeriesActors).HasConstraintName("FK__SeriesAct__Serie__5AEE82B9");
        });

        modelBuilder.Entity<SeriesCategories>(entity =>
        {
            entity.HasKey(e => new { e.SeriesId, e.CategoryId }).HasName("PK__SeriesCa__ED5E517A2E8C616E");

            entity.Property(e => e.SeriesCategoryId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Categories).WithMany(p => p.SeriesCategories).HasConstraintName("FK__SeriesCat__Categ__5441852A");

            entity.HasOne(d => d.Series).WithMany(p => p.SeriesCategories).HasConstraintName("FK__SeriesCat__Serie__534D60F1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC901230A1");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
