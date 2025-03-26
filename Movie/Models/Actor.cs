using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Movie.Models;

public partial class Actor
{
    [Key]
    [Column("ActorId")]
    public int ActorId { get; set; }

    [StringLength(225)]
    public string NameAct { get; set; } = null!;

    public string? Description { get; set; }

    [StringLength(100)]
    public string? Nationality { get; set; }

    [StringLength(255)]
    public string? Professional { get; set; }

    [StringLength(255)]
    public string? AvatarUrl { get; set; }

    public int Status { get; set; }

    [InverseProperty("Actors")]
    public virtual ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();

    [InverseProperty("Actors")]
    public virtual ICollection<SeriesActor> SeriesActors { get; set; } = new List<SeriesActor>();
}
