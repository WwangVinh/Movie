using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Movie.Models;

[PrimaryKey("SeriesId", "ActorId")]
[Table("SeriesActors")]
public partial class SeriesActors
{
  
    [Key]
    [Column("SeriesID")]
    public int SeriesId { get; set; }

    [Key]
    [Column("ActorId")]
    public int ActorId { get; set; }

    [Column("SeriesActorID")]
    public int SeriesActorId { get; set; }

    [ForeignKey("ActorId")]
    [InverseProperty("SeriesActors")]
    public virtual Actor Actors { get; set; } = null!;

    [ForeignKey("SeriesId")]
    [InverseProperty("SeriesActors")]
    public virtual Series Series { get; set; } = null!;
}
