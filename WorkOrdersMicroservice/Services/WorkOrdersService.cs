using DadaRepositories;
using DadaRepositories.Models;
using DadaRepositories.Utilities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WorkOrdersMicroservice.Services
{
    public class WorkOrdersService
    {
        private readonly FirestoreRepository _works;
        private readonly FirestoreRepository _customers;
        private readonly FirestoreRepository _conf;


        public WorkOrdersService(IConfiguration configuration)
        {
            string filepath = configuration.GetValue<string>("Settings:FirebaseSettings:FilePath");
            string projectid = configuration.GetValue<string>("Settings:FirebaseSettings:ProjectId");
            _works = new FirestoreRepository(filepath, projectid, Collection.WorkOrders);
            _customers = new FirestoreRepository(filepath, projectid, Collection.Customers);
            _conf = new FirestoreRepository(filepath, projectid, Collection.Configurations);
        }


        public async Task<List<WorkOrder>> GetWorkOrders() => await _works.GetAllAsync<WorkOrder>();


        public async Task<WorkOrder> GetWorkOrder(string id) => await _works.GetAsync<WorkOrder>(id);


        public async Task<WorkOrder> CreateWorkOrder(WorkOrder work)
        {
            Configuration workConf = await _conf.GetAsync<Configuration>("WorkOrder");

            int.TryParse(workConf.LastId, out int correlative);
            correlative++;

            work.Id = correlative.ToString();
            int detailId = 1;
            work.Details.ForEach(f =>
            {
                f.WorkOrderId = correlative.ToString();
                f.Id = detailId.ToString();
                detailId++;
            });

            work = await _works.AddAsync(work);

            workConf.LastId = correlative.ToString();
            await _conf.UpdateAsync(workConf);

            return work;

        }

    }
}
