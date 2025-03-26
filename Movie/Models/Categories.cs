using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Movie.Models;

[Index("CategoryName", Name = "UQ__Categori__8517B2E0BC8DDCCD", IsUnique = true)]
public partial class Categories
{
    [Key]
    [Column("CategoryId")]
    public int CategoryId { get; set; }

    [StringLength(50)]
    public string CategoryName { get; set; } = null!;

    [InverseProperty("Categories")]
    public virtual ICollection<MovieCategory> MovieCategories { get; set; } = new List<MovieCategory>();

    [InverseProperty("Categories")]
    public virtual ICollection<SeriesCategory> SeriesCategories { get; set; } = new List<SeriesCategory>();
}
