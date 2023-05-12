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
        

        public async Task<(InventoryMovement, string)> CreateInventoryMovement(InventoryMovement movement)
        {
            string message = null;

            movement.Type = _type.ToString();

            if (movement.Date > DateTime.Today)
            {
                message = "Date not valid";
                return (null, message);
            }

            movement.Details.ForEach(f =>
            {
                Product prod = _context.Products.FirstOrDefault(p => p.Id == f.ProductId);

                if (prod == null)
                {
                    message = $"The item {f.ProductId} not found";
                    return;
                }

                if (movement.Type == InventoryMovementType.Out.ToString())
                {
                    if (f.Quantity > prod.Availability)
                    {
                        message = $"The item {f.ProductId} is out of stock";
                        return;
                    }
                }
            });

            if (message != null)
            {
                return (null, message);
            }

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

            return (await GetInventoyMovement(result.Entity.Id), message);
        }
        

        public async Task<(InventoryMovement, string)> UpdateInventoryMovement(InventoryMovement movement)
        {
            string message = null;

            InventoryMovement im = await GetInventoyMovement(movement.Id);

            if (im is null)
            {
                message = "Inventory movement not found";
                return (null, message);
            }

            if (movement.Date > DateTime.Today)
            {
                message = "Date not valid";
                return (null, message);
            }

            movement.Details.ForEach(f =>
            {
                Product prod = _context.Products.FirstOrDefault(p => p.Id == f.ProductId);

                if (prod == null)
                {
                    message = $"The item {f.ProductId} not found";
                    return;
                }

                if (movement.Type == InventoryMovementType.Out.ToString())
                {
                    if (f.Quantity > prod.Availability)
                    {
                        message = $"The item {f.ProductId} is out of stock";
                        return;
                    }
                }
            });

            if (message != null)
            {
                return (null, message);
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

            return (movement, message);
        }

    }
}
