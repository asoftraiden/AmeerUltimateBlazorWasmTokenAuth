using System.ComponentModel.DataAnnotations;

namespace UltimateBlazorWasmTokenAuth.DataTransferObjects;

public class NonConformingForUpdateDto
{
   
    [Required(ErrorMessage = "Description is a required field.")]
    [MaxLength(500, ErrorMessage = "Maximum length for the Description is 500 characters.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Detection Date is a required field.")]
    public DateTime? DetectionDate { get; set; }

    [Required(ErrorMessage = "Status is a required field.")]
    public NCStatus Status { get; set; }

    [Required(ErrorMessage = "Type is a required field.")]
    public NCType Type { get; set; }

    public DispositionType? Disposition { get; set; }

    [MaxLength(500, ErrorMessage = "Maximum length for the Disposition Notes is 500 characters.")]
    public string? DispositionNotes { get; set; }

    [Required(ErrorMessage = "Quantity Affected is a required field.")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity Affected must be greater than 0.")]
    public int QuantityAffected { get; set; }

    public Guid? PartId { get; set; }
    
    public Guid? CellId { get; set; }
    
    public Guid? ModelId { get; set; }
    
    public Guid? ResponsibleSupplierId { get; set; }
    
    [MaxLength(50)]
    public string? DetectedBy { get; set; }

    [MaxLength(50)]
    public string? DispositionBy { get; set; }

    public DateTime? DispositionDate { get; set; }

    [MaxLength(1000, ErrorMessage = "Maximum length for the Root Cause is 1000 characters.")]
    public string? RootCause { get; set; }

    [MaxLength(1000, ErrorMessage = "Maximum length for the Corrective Action is 1000 characters.")]
    public string? CorrectiveAction { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Cost Impact cannot be negative.")]
    public decimal? CostImpact { get; set; }

    public bool RequiresCustomerNotification { get; set; }

    public DateTime CreatedAt { get; set; } 
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;


}



public enum NCStatus
{
    Open,
    UnderInvestigation,
    PendingDisposition,
    Rejected,
    Reworked,
    Scrapped,
    Closed
}

public enum NCType
{
    Dimensional,
    Visual,
    Functional,
    Documentation,
    Packaging,
    Other
}

public enum DispositionType
{
    UseAsIs,
    Rework,
    Repair,
    Scrap,
    ReturnToSupplier,
    Deviation
}