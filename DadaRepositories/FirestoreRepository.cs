using DadaRepositories.Interfaces;
using DadaRepositories.Utilities;
using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace DadaRepositories
{
    /// <summary>
    /// Google Firestore Repository
    /// </summary>
    public class FirestoreRepository : IBaseRepository
    {
        /// <summary>
        /// Collection access on instance
        /// </summary>
        private readonly Collection _collection;

        /// <summary>
        /// Firestore Service
        /// </summary>
        private readonly FirestoreDb _firestoreDb;


        /// <summary>
        /// Repository Constructor
        /// </summary>
        /// <param name="collection"></param>
        public FirestoreRepository(string filePath, string projectId, Collection collection)
        {
            //Set Credentials 
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filePath);

            //Set FirestoreDB in Firebase Project
            _firestoreDb = FirestoreDb.Create(projectId);

            //Get Collection to access
            _collection = collection;
        }



        /// <summary>
        /// Get all records from repository
        /// </summary>
        /// <typeparam name="T">Model base from collection</typeparam>
        /// <returns>List of records from collection</returns>
        public async Task<List<T>> GetAllAsync<T>() where T : IBaseFirestoreData
        {
            //List to return 
            List<T> list = new List<T>();

            //Get access to collection
            Query query = _firestoreDb.Collection(_collection.ToString());

            //Get Query Snapshot
            var querySnapshot = await query.GetSnapshotAsync();
           
            //Fill Model List
            foreach (var documentSnapshot in querySnapshot.Documents)
            {
                if (!documentSnapshot.Exists) continue;
                var data = documentSnapshot.ConvertTo<ExpandoObject>();
                if (data == null) continue;
                var entity = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(data));
                entity.Id = documentSnapshot.Id;
                list.Add(entity);
            }

            //Return list
            return list;
        }


        /// <summary>
        /// Get record from collection by Id
        /// </summary>
        /// <typeparam name="T">Model base from collection</typeparam>
        /// <param name="id">Id record from collection</param>
        /// <returns>Reccord model from collection</returns>
        public async Task<object> GetAsync<T>(string id) where T : IBaseFirestoreData
        {
            //Get access to collection and return document
            var docRef = _firestoreDb.Collection(_collection.ToString()).Document(id);

            //Get snapshot from document
            var snapshot = await docRef.GetSnapshotAsync();

            //Verify snapshot and return object
            if (snapshot.Exists)
            {
                var data = snapshot.ConvertTo<ExpandoObject>();
                var entity = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(data));
                entity.Id = snapshot.Id;
                return entity;
            }

            //Return null if snapshot doesn't exist
            return null;
        }


        /// <summary>
        /// Create record in collection
        /// </summary>
        /// <typeparam name="T">Model base from collection</typeparam>
        /// <param name="entity">Model object to create in collection</param>
        /// <returns>Record created in collection</returns>
        public async Task<T> AddAsync<T>(T entity) where T : IBaseFirestoreData
        {
            try
            {
                //Get collection reference
                var colRef = _firestoreDb.Collection(_collection.ToString()).Document(entity.Id);

                //Convert data to jsonobject
                var data = JsonConvert.DeserializeObject<ExpandoObject>(JsonConvert.SerializeObject(entity));

                //Add record to collection
                var doc = await colRef.CreateAsync(data);

                //Get and return new object from collection
                return (T)await GetAsync<T>(entity.Id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// Update a record in collection 
        /// </summary>
        /// <typeparam name="T">Model base from collection</typeparam>
        /// <param name="entity">Model object to update in collection</param>
        /// <returns>Record updated in collection</returns>
        public async Task<T> UpdateAsync<T>(T entity) where T : IBaseFirestoreData
        {
            try
            {
                //Get record from collection to update
                var recordRef = _firestoreDb.Collection(_collection.ToString()).Document(entity.Id);

                //Convert data to json object
                var data = JsonConvert.DeserializeObject<ExpandoObject>(JsonConvert.SerializeObject(entity));
                
                //Uodate record
                await recordRef.SetAsync(data, SetOptions.MergeAll);
                
                return (T) await GetAsync<T>(recordRef.Id);
                
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }


        /// <summary>
        /// Delete record from collection
        /// </summary>
        /// <typeparam name="T">Model base from collection</typeparam>
        /// <param name="entity">Model object to delete in collection</param>
        /// <returns>Boolean flag to proccess status</returns>
        public async Task<bool> DeleteAsync<T>(T entity) where T : IBaseFirestoreData
        {
            try
            {
                //Get record from collection
                var recordRef = _firestoreDb.Collection(_collection.ToString()).Document(entity.Id);

                //Delete record
                await recordRef.DeleteAsync();

                //Return success
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<List<T>> QueryRecordsAsync<T>(Query query) where T : IBaseFirestoreData
        {
            var querySnapshot = await query.GetSnapshotAsync();
            var list = new List<T>();
            foreach (var documentSnapshot in querySnapshot.Documents)
            {
                if (!documentSnapshot.Exists) continue;
                var data = documentSnapshot.ConvertTo<T>();
                if (data == null) continue;
                data.Id = documentSnapshot.Id;
                list.Add(data);
            }

            return list;
        }
    }
}
