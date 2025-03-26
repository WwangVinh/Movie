using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Movie.Models;

[PrimaryKey("MovieId", "CategoryId")]
public partial class MovieCategory
{
    [Key]
    [Column("MovieId")]
    public int MovieId { get; set; }

    [Key]
    [Column("CategoryId")]
    public int CategoryId { get; set; }

    [Column("MovieCategoryID")]
    public int MovieCategoryId { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("MovieCategories")]
    public virtual Categories Categories { get; set; } = null!;

    [ForeignKey("MovieId")]
    [InverseProperty("MovieCategories")]
    public virtual Movies Movie { get; set; } = null!;
}
