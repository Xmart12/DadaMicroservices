using DadaRepositories;
using DadaRepositories.Models;
using DadaRepositories.Utilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryMicroservice.Services
{
    public class InventoryMovementsService
    {
        private readonly FirestoreRepository _movement;
        private readonly FirestoreRepository _conf;


        private readonly InventoryMovementType _type;

        public InventoryMovementsService(IConfiguration configuration, InventoryMovementType type)
        {
            string filepath = configuration.GetValue<string>("Settings:FirebaseSettings:FilePath");
            string projectid = configuration.GetValue<string>("Settings:FirebaseSettings:ProjectId");
            _movement = new FirestoreRepository(filepath, projectid, Collection.Inventory);
            _conf = new FirestoreRepository(filepath, projectid, Collection.Configurations);

            _type = type;
        }


        public async Task<List<InventoryMovement>> GetInventoyMovements() => await _movement
            .QueryRecordsAsync<InventoryMovement>(nameof(InventoryMovement.Type), _type.ToString());


        public async Task<InventoryMovement> GetInventoyMovement(string id) => await _movement
            .GetAsync<InventoryMovement>(id);


        public async Task<InventoryMovement> CreateInventoryMovement(InventoryMovement movement)
        {
            Configuration movementConf = await _conf.GetAsync<Configuration>("InventoryMovements");

            int.TryParse(movementConf.LastId, out int correlative);
            correlative++;

            movement.Id = correlative.ToString();
            //movement.Date = DateTime.SpecifyKind((DateTime)movement.Date, DateTimeKind.Utc);
            int detailId = 1;
            movement.Details.ForEach(f =>
            {
                f.InventoryMovementId = correlative.ToString();
                f.Id = detailId.ToString();
                detailId++;
            });

            movement = await _movement.AddAsync(movement);

            movementConf.LastId = correlative.ToString();
            await _conf.UpdateAsync(movementConf);

            return movement;
        }
        
        public async Task<InventoryMovement> UpdateInventoryMovement(InventoryMovement movement)
        {
            InventoryMovement sr = await _movement.GetAsync<InventoryMovement>(movement.Id);

            if (sr is null)
            {
                throw new Exception();
            }

            sr.Description = movement.Description;
            sr.Details = movement.Details;

            return await _movement.UpdateAsync(sr);
        }

    }
}
