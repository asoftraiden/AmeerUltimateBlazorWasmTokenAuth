namespace UltimateBlazorWasmTokenAuth.DataTransferObjects;

public class PartDto
{
    public Guid Id { get; set; }
    public string PartNumber { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public Guid SupplierId { get; set; }
}