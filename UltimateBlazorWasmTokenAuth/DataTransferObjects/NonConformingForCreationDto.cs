namespace UltimateBlazorWasmTokenAuth.DataTransferObjects;

public record NonConformingForCreationDto
{
   
    public string Description { get; set; }
    public DateTime? DetectionDate { get; set; }
    public string Status { get; set; }
    public string Type { get; set; }
    public int QuantityAffected { get; set; }
    public Guid? PartId { get; set; }
    public Guid? CellId { get; set; }
    public Guid? ModelId { get; set; }
    public string DetectedBy { get; set; }
    public string RootCause { get; set; }
    public bool RequiresCustomerNotification { get; set; }
}