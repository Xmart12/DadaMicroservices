using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DadaRepositories.Interfaces
{
    public interface IBaseRepository
    {
        /// <summary>
        /// Gets all record from the repository.
        /// </summary> 
        /// <returns>a records of type T</returns>
        Task<List<TEntity>> GetAllAsync<TEntity>() where TEntity : class, IBaseFirestoreData;

        /// <summary>
        /// Gets a record from the repository.
        /// </summary>
        /// <param name="id">Index record to get</param>
        /// <returns>a record of type T</returns>
        Task<TEntity> GetAsync<TEntity>(string id) where TEntity : class, IBaseFirestoreData;

        /// <summary>
        /// Adds a record to the repository.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>a record of type T</returns>
        Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity : class, IBaseFirestoreData;

        /// <summary>
        /// Updates a record in the repository.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>a record of type T</returns>
        Task<TEntity> UpdateAsync<TEntity>(TEntity entity) where TEntity : class, IBaseFirestoreData;

        /// <summary>
        /// Adds a record to the repository.
        /// </summary>
        /// <param name="entity"></param> S
        Task<bool> DeleteAsync<TEntity>(TEntity entity) where TEntity : class, IBaseFirestoreData;

        /// <summary>
        /// Query all record from the repository.
        /// </summary> 
        /// <returns>a records of type T</returns>
        Task<List<TEntity>> QueryRecordsAsync<TEntity>(string pathField, object value) where TEntity : class, IBaseFirestoreData;
    }
}
