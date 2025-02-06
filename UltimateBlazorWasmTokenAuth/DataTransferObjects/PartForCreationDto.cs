namespace UltimateBlazorWasmTokenAuth.DataTransferObjects;

public class PartForCreationDto
{
    public Guid SupplierId { get; set; }
    public string PartNumber { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
}
