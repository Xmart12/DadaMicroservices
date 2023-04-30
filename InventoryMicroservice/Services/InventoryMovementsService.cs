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

namespace InventoryMicroservice.Services
{
    public class InventoryMovementsService
    {
        private readonly DadaDbContext _context; 
        private readonly InventoryMovementType _type;


        public InventoryMovementsService(DadaDbContext context, InventoryMovementType type)
        {
            _context = context;
            _type = type;
        }


        public async Task<List<InventoryMovement>> GetInventoyMovements() => await _context.InventoryMovements
            .Include(i => i.Details).ThenInclude(i => i.Product).Where(w => w.Type == _type.ToString())
            .ToListAsync();


        public async Task<InventoryMovement> GetInventoyMovement(int id) => await _context.InventoryMovements
            .Include(i => i.Details).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(f => f.Type == _type.ToString() && f.Id == id);
        

        public async Task<InventoryMovement> CreateInventoryMovement(InventoryMovement movement)
        {
            movement.Type = _type.ToString();

            var result = await _context.InventoryMovements.AddAsync(movement);
            await _context.SaveChangesAsync();

            if (result.State == EntityState.Added)
            {
                movement.Details.ForEach(f =>
                {
                    f.InventoryMovementId = result.Entity.Id;

                    _context.InventoryMovementDetails.Add(f);
                });

                await _context.SaveChangesAsync();
            }

            return await GetInventoyMovement(result.Entity.Id);
        }
        

        public async Task<InventoryMovement> UpdateInventoryMovement(InventoryMovement movement)
        {
            InventoryMovement im = await GetInventoyMovement(movement.Id);

            if (im is null)
            {
                throw new Exception();
            }

            im.Description = movement.Description;
            _context.Entry(im).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            im.Details.ForEach(f =>
            {
                _context.InventoryMovementDetails.Remove(f);
            });
            movement.Details.ForEach(f =>
            {
                _context.InventoryMovementDetails.Add(f);
            });
            await _context.SaveChangesAsync();

            return movement;
        }

    }
}
