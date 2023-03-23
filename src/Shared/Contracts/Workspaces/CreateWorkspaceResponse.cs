namespace Digitime.Shared.Contracts.Workspaces;

public record CreateWorkspaceResponse(string Id, string Name, string Description, SubscriptionDto Subscription);
public record SubscriptionDto(string PricingTier, DateTime? ExpirationDate, bool IsTrial, bool IsExpired, List<InvoiceDto> Invoices);
public record InvoiceDto(string SubscriptionId, string InvoiceNumber, string TaxlessAmount, string TaxAmount, string TotalAmount);
