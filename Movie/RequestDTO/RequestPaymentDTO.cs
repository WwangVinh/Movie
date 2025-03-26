using System;

namespace Movie.RequestDTO;

public partial class RequestPaymentDTO
{
    public int SubPayment { get; set; }
    public int? UserId { get; set; }
    public string? PlanName { get; set; }
    public decimal? Price { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? Status { get; set; }

}
