using DadaRepositories.Contexts;
using DadaRepositories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesMicroservice.Services
{
    public class InvoicesService
    {
        private readonly DadaDbContext _context;

        public InvoicesService(DadaDbContext context)
        {
            _context = context;
        }


        public async Task<List<Invoice>> GetInvoices() => await _context.Invoices
            .Include(i => i.Customer).Include(i => i.Details).ThenInclude(i => i.Product)
            .ToListAsync();


        public async Task<Invoice> GetInvoice(int id) => await _context.Invoices
            .Include(i => i.Customer).Include(i => i.Details).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(f => f.Id == id);


        public async Task<(Invoice, string)> CreateInvoice(Invoice invoice)
        {
            string message = null;

            Customer cus = _context.Customers.FirstOrDefault(f => f.Id == invoice.CustomerId);

            if (cus == null)
            {
                message = "Customer not found";
                return (null, message);
            }

            if (invoice.Date > DateTime.Today)
            {
                message = "Date not valid";
                return (null, message);
            }

            invoice.Details.ForEach(f =>
            {
                Product prod = _context.Products.FirstOrDefault(p => p.Id == f.ProductId);

                if (prod == null)
                {
                    message = $"The item {f.ProductId} not found";
                    return;
                }
            });

            if (message != null)
            {
                return (null, message);
            }

            var result = await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();

            if (result.State == EntityState.Added)
            {
                invoice.Details.ForEach(f =>
                {
                    f.InvoiceId = result.Entity.Id;

                    _context.InvoiceDetails.Add(f);
                });

                await _context.SaveChangesAsync();
            }

            return (await GetInvoice(result.Entity.Id), message);
        }
    }
}
