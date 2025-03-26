using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Movie.Models;

[PrimaryKey("SeriesId", "CategoriesId")]
public partial class SeriesCategory
{
    [Key]
    [Column("SeriesID")]
    public int SeriesId { get; set; }

    [Key]
    [Column("CategoriesID")]
    public int CategoriesId { get; set; }

    [Column("SeriesCategoriesID")]
    public int SeriesCategoriesId { get; set; }

    [ForeignKey("CategoriesId")]
    [InverseProperty("SeriesCategories")]
    public virtual Categories Categories { get; set; } = null!;

    [ForeignKey("SeriesId")]
    [InverseProperty("SeriesCategories")]
    public virtual Series Series { get; set; } = null!;
}
