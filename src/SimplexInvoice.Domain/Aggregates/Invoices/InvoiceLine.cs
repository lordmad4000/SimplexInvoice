using SimplexInvoice.Domain.ValueObjects;
using System;

namespace SimplexInvoice.Domain.Invoices;
public partial class InvoiceLine
{
    public Guid InvoiceId { get; private set; } = Guid.Empty;
    public int LineNumber { get; private set; } = 0;
    public string ProductCode { get; private set; } = string.Empty;
    public string ProductName { get; private set; } = string.Empty;
    public string ProductDescription { get; private set; } = string.Empty;
    public int Packages { get; private set; } = 0;
    public double Quantity { get; private set; } = 0;
    public Money Price { get; private set; } = new Money("USD", 0.0);
    public string TaxName { get; private set; } = string.Empty;
    public int TaxRate { get; private set; } = 0;
    public int DiscountRate { get; private set; } = 0;
    public Money Tax { get; private set; } = new Money("USD", 0.0);
    public Money Discount { get; private set; } = new Money("USD", 0.0);
    public Money TaxBase { get; private set; } = new Money("USD", 0.0);
    public Money Total { get; private set; } = new Money("USD", 0.0);

}