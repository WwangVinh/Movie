using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Movie.Models;

[Table("Payment")]
public partial class Payment
{
    [Key]
    [Column("SubPaymentID")]
    public int SubPaymentId { get; set; }

    [Column("UserID")]
    public int? UserId { get; set; }

    [StringLength(50)]
    public string? PlanName { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Price { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? Status { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Payments")]
    public virtual User? User { get; set; }
}
