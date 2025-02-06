namespace UltimateBlazorWasmTokenAuth.DataTransferObjects;

public class PartForUpdateDto
{
    public string PartNumber { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public Guid SupplierId { get; set; }
}
