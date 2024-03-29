﻿using SimplexInvoice.Domain.Invoices;

namespace SimplexInvoice.Application.Common.Interfaces.Persistance
{
    public interface IInvoiceRepository : ICacheableRepository<Invoice>
    {
    }
}