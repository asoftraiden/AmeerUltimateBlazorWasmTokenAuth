using System.ComponentModel.DataAnnotations;

namespace UltimateBlazorWasmTokenAuth.DataTransferObjects;

public class ModelForCreationDto
{
    [Required]
    [StringLength(100)]
    public string Description { get; set; }
    public Guid? FamilyId { get; set; }
}
