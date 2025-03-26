using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Movie.Models;

[PrimaryKey("SeriesId", "ActorsId")]
[Table("SeriesActor")]
public partial class SeriesActor
{
    [Key]
    [Column("SeriesID")]
    public int SeriesId { get; set; }

    [Key]
    [Column("ActorsID")]
    public int ActorsId { get; set; }

    [Column("SeriesActorID")]
    public int SeriesActorId { get; set; }

    [ForeignKey("ActorsId")]
    [InverseProperty("SeriesActors")]
    public virtual Actor Actors { get; set; } = null!;

    [ForeignKey("SeriesId")]
    [InverseProperty("SeriesActors")]
    public virtual Series Series { get; set; } = null!;
}
