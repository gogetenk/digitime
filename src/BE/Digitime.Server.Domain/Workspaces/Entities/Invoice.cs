using Digitime.Server.Domain.Models;

namespace Digitime.Server.Domain.Workspaces.Entities;

public class Invoice : Entity<string>
{
    public string SubscriptionId { get; private set; }
    public string InvoiceNumber { get; private set; }
    public float TaxlessAmount { get; private set; }
    public float TaxAmount { get; private set; }
    public float TotalAmount { get; private set; }

    private Invoice(string id, string subscriptionId, string invoiceNumber, float taxlessAmount, float taxAmount, float totalAmount)
        : base(id)
    {
        SubscriptionId = subscriptionId;
        InvoiceNumber = invoiceNumber;
        TaxlessAmount = taxlessAmount;
        TaxAmount = taxAmount;
        TotalAmount = totalAmount;
    }

    public static Invoice Create(string id, string subscriptionId, string invoiceNumber, float taxlessAmount, float taxAmount, float totalAmount)
    {
        return new Invoice(id, subscriptionId, invoiceNumber, taxlessAmount, taxAmount, totalAmount);
    }
}
