using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Movie.Models;

[PrimaryKey("SeriesId", "CategoryId")]
public partial class SeriesCategories
{
    [Key]
    [Column("SeriesID")]
    public int SeriesId { get; set; }

    [Key]
    [Column("CategoryId")]
    public int CategoryId { get; set; }

    [Column("SeriesCategoryId")]
    public int SeriesCategoryId { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("SeriesCategories")]
    public virtual Categories Categories { get; set; } = null!;

    [ForeignKey("SeriesId")]
    [InverseProperty("SeriesCategories")]
    public virtual Series Series { get; set; } = null!;
}
