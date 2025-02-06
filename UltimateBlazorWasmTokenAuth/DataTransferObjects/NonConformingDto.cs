namespace UltimateBlazorWasmTokenAuth.DataTransferObjects;

public record NonConformingDto(
    Guid Id,
    string NcNumber,
    string Description,
    DateTime DetectionDate,
    string Status,
    string Type,
    int QuantityAffected,
    Guid PartId,
    Guid CellId,
    Guid ModelId,
    string DetectedBy,
    string RootCause,
    bool RequiresCustomerNotification
);