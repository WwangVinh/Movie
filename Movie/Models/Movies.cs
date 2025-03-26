using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Movie.Models;

public partial class Movies
{
    [Key]
    [Column("MovieId")]
    public int MovieId { get; set; }

    [StringLength(255)]
    public string Title { get; set; } = null!;
    public string? Nation { get; set; }

    public string? Description { get; set; }

    [Column("DirectorId")]
    public int? DirectorId { get; set; }

    [Column(TypeName = "decimal(3, 1)")]
    public decimal? Rating { get; set; }

    [Column("PosterURL")]
    [StringLength(255)]
    public string? PosterUrl { get; set; }

    [Column("AvatarUrl")]
    [StringLength(255)]
    public string? AvatarUrl { get; set; }

    [Column("LinkFilmURl")]
    [StringLength(255)]
    public string? LinkFilmUrl { get; set; }

    public int? Status { get; set; }

    public bool? IsHot { get; set; }

    public DateTime? YearReleased { get; set; }

    [ForeignKey("DirectorId")]
    [InverseProperty("Movie")]
    public virtual Director? Director { get; set; }

    [InverseProperty("Movie")]
    public virtual ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();

    [InverseProperty("Movie")]
    public virtual ICollection<MovieCategory> MovieCategories { get; set; } = new List<MovieCategory>();
}
