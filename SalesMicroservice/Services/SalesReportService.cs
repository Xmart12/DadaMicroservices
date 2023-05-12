using DadaRepositories.Contexts;
using DadaRepositories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesMicroservice.Services
{
    public class SalesReportService
    {
        private readonly DadaDbContext _context;

        public SalesReportService(DadaDbContext context)
        {
            _context = context;
        }


        public async Task<List<SalesReport>> GetSalesReports() => await _context.SalesReports
            .Include(i => i.Customer).Include(i => i.Details).ThenInclude(t => t.Product).ToListAsync();


        public async Task<SalesReport> GetSalesReport(int id) => await _context.SalesReports
            .Include(i => i.Customer).Include(i => i.Details).ThenInclude(t => t.Product)
            .FirstOrDefaultAsync(s => s.Id == id);


        public async Task<(SalesReport, string)> CreateSalesReport(SalesReport sales)
        {
            //set message
            string message = null;

            if (sales.CustomerId is null)
            {
                if (sales.Customer is null)
                {
                    message = "Customer not found";
                    return (null, message);
                }

                if ((await _context.Customers.AsQueryable()
                    .FirstOrDefaultAsync(c => c.Id == sales.Customer.Document)) is null)
                {
                    sales.Customer.Id = sales.Customer.Document;
                    sales.CustomerId = sales.Customer.Document;
                    await _context.Customers.AddAsync(sales.Customer);
                    await _context.SaveChangesAsync();
                }
            }

            if (sales.Customer is null)
            {
                sales.Customer = await _context.Customers.AsQueryable()
                    .FirstOrDefaultAsync(c => c.Id == sales.CustomerId);

                if (sales.Customer is null)
                {
                    message = "Unable to create customer";
                    return (null, message);
                }
            }

            if (sales.Date > DateTime.Today)
            {
                message = "Date is not valid";
                return (null, message);
            }

            sales.Details.ForEach(f =>
            {
                Product prod = _context.Products.FirstOrDefault(p => p.Id == f.ProductId);

                if (prod == null)
                {
                    message = $"The item {f.ProductId} cannot be found";
                    return;
                }
            });

            if (message != null)
            {
                return (null, message);
            }

            var result = await _context.SalesReports.AddAsync(sales);
            await _context.SaveChangesAsync(); 

            if (result.State == EntityState.Added)
            {
                sales.Details.ForEach(f =>
                {
                    f.SalesReportId = result.Entity.Id;

                    _context.SalesReportDetails.Add(f);
                });

                await _context.SaveChangesAsync();
            }

            return (await GetSalesReport(result.Entity.Id), null);
        }


        public async Task<(SalesReport, string)> UpdateSalesReport(SalesReport sales)
        {
            string message = null;

            SalesReport sr = await GetSalesReport(sales.Id);

            if (sr is null)
            {
                message = "Sales report not found";
            }

            sales.Details.ForEach(f =>
            {
                Product prod = _context.Products.FirstOrDefault(p => p.Id == f.ProductId);

                if (prod == null)
                {
                    message = $"The item {f.ProductId} cannot be found";
                    return;
                }
            });

            sr.Description = sales.Description;
            _context.Entry(sr).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            sr.Details.ForEach(f =>
            {
                _context.SalesReportDetails.Remove(f);
            });
            sales.Details.ForEach(f =>
            {
                _context.SalesReportDetails.Add(f);
            });
            await _context.SaveChangesAsync();

            return (sales, null);
        }

    }
}
