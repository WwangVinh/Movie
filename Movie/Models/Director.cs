using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Movie.Models;

public partial class Director
{
    [Key]
    [Column("DirectorID")]
    public int DirectorId { get; set; }

    [StringLength(225)]
    public string NameDir { get; set; } = null!;

    public string? Description { get; set; }

    [StringLength(100)]
    public string? Nationality { get; set; }

    [StringLength(255)]
    public string? Professional { get; set; }

    [StringLength(255)]
    public string? AvatarUrl { get; set; }

    [InverseProperty("Director")]
    public virtual ICollection<Movies> Movie { get; set; } = new List<Movies>();

    [InverseProperty("Director")]
    public virtual ICollection<Series> Series { get; set; } = new List<Series>();
}
