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

    [InverseProperty("Actors")]
    public virtual ICollection<MovieActors> MovieActor { get; set; } = new List<MovieActors>();

    [InverseProperty("Actors")]
    public virtual ICollection<SeriesActors> SeriesActors { get; set; } = new List<SeriesActors>();
}
