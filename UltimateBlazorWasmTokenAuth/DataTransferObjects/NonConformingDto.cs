namespace UltimateBlazorWasmTokenAuth.DataTransferObjects;

public record NonConformingDto(
    Guid Id,
    string NcNumber,
    string Description,
    DateTime DetectionDate,
    string Status,
    string Type,
    int QuantityAffected,

    Guid? PartId,
    string? PartDescription,

    Guid? CellId,
    string? CellDescription,

    Guid? ModelId,
    string? ModelDescription,

    string DetectedBy,
    string RootCause,
    bool RequiresCustomerNotification
);