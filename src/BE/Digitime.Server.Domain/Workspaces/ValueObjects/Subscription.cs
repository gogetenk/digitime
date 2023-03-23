using System;
using System.Collections.Generic;
using Digitime.Server.Domain.Workspaces.Entities;

namespace Digitime.Server.Domain.Workspaces.ValueObjects;
public class Subscription
{
    private readonly List<Invoice> _invoices = new();

    public string PricingTierId { get; private set; }
    public DateTime? ExpirationDate { get; private set; }
    public bool IsTrial { get; private set; }
    public bool IsExpired => ExpirationDate.HasValue && ExpirationDate.Value < DateTime.UtcNow;
    public IReadOnlyList<Invoice> Invoices => _invoices.AsReadOnly();
}
