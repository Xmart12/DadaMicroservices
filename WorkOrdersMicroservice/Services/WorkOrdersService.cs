using DadaRepositories;
using DadaRepositories.Contexts;
using DadaRepositories.Models;
using DadaRepositories.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
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


        public async Task<WorkOrder> CreateWorkOrder(WorkOrder work)
        {
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

            return await GetWorkOrder(result.Entity.Id);
        }

    }
}
