using DadaRepositories;
using DadaRepositories.Utilities;
using SalesMicroservice.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesMicroservice.Repositories
{
    public class SalesReportRepository
    {
        private readonly FirestoreRepository _repository;


        public SalesReportRepository()
        {
            _repository = new FirestoreRepository(Collection.SalesReports);
        }


        public async Task<List<SalesReport>> GetAllAsync() => await _repository.GetAllAsync<SalesReport>();

        public async Task<SalesReport> GetAsync(string id) => (SalesReport) await _repository.GetAsync<SalesReport>(id);

        public async Task<SalesReport> AddAsync(SalesReport entity) => await _repository.AddAsync(entity);

        public async Task<SalesReport> UpdateAsync(SalesReport entity) => await _repository.UpdateAsync(entity);

        public async Task DeleteAsync(SalesReport entity) => await _repository.DeleteAsync(entity);

    }
}