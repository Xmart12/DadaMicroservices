using DadaRepositories;
using DadaRepositories.Contexts;
using DadaRepositories.Models;
using DadaRepositories.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkOrdersMicroservice.Services
{
    public class WorkOrdersService
    {
        private readonly DadaDbContext _context;


        public WorkOrdersService(DadaDbContext context)
        {
            _context = context;
        }


        public async Task<List<WorkOrder>> GetWorkOrders() => await _context.WorkOrders
            .Include(i => i.Customer).Include(i => i.Details).ThenInclude(i => i.Product)
            .ToListAsync();


        public async Task<WorkOrder> GetWorkOrder(int id) => await _context.WorkOrders
            .Include(i => i.Customer).Include(i => i.Details).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(f => f.Id == id);


        public async Task<(WorkOrder, string)> CreateWorkOrder(WorkOrder work)
        {
            string message = null;

            Customer cus = _context.Customers.FirstOrDefault(f => f.Id == work.CustomerId);

            if (cus == null)
            {
                message = "Customer not found";
                return (null, message);
            }
            
            if (work.Date > DateTime.Today)
            {
                message = "Date not valid";
                return (null, message);
            }

            work.Details.ForEach(f =>
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

            var result = await _context.WorkOrders.AddAsync(work);
            await _context.SaveChangesAsync();

            if (result.State == EntityState.Added)
            {
                work.Details.ForEach(f =>
                {
                    f.WorkOrderId = result.Entity.Id;

                    _context.WorkOrderDetails.Add(f);
                });

                await _context.SaveChangesAsync();
            }

            return (await GetWorkOrder(result.Entity.Id), message);
        }

    }
}
