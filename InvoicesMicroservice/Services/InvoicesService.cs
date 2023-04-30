using DadaRepositories.Contexts;
using DadaRepositories.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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


        public async Task<Invoice> CreateInvoice(Invoice invoice)
        {
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

            return await GetInvoice(result.Entity.Id);
        }
    }
}
