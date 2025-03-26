using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Movie.Models;

[PrimaryKey("MovieId", "ActorId")]
[Table("MovieActors")]
public partial class MovieActor
{
    public int MovieActorId { get; set; }

    [Key]
    [Column("MovieId")]
    public int MovieId { get; set; }

    [Key]
    [Column("ActorId")]
    public int ActorId { get; set; }

    [ForeignKey("ActorId")]
    [InverseProperty("MovieActors")]
    public virtual Actor Actors { get; set; } = null!;

    [ForeignKey("MovieId")]
    [InverseProperty("MovieActors")]
    public virtual Movies Movie { get; set; } = null!;
}
