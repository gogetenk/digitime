using Digitime.Server.Domain.Workspaces.Entities;

namespace Digitime.Server.Infrastructure.Entities;

public class SubscriptionEntity
{
    private readonly List<Invoice> _invoices = new();

    public PricingTier PricingTier { get;  set; }
    public DateTime? ExpirationDate { get;  set; }
    public bool IsTrial { get; set; }
    public bool IsExpired => ExpirationDate.HasValue && ExpirationDate.Value < DateTime.UtcNow;
    public IReadOnlyList<Invoice> Invoices => _invoices.AsReadOnly();
}

public record PricingTier(string Id, string Name, string Description, decimal Price, int MaxProjects, int MaxMembers);