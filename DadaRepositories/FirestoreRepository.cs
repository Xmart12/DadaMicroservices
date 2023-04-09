using DadaRepositories.Interfaces;
using DadaRepositories.Utilities;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DadaRepositories
{
    public class FirestoreRepository : IBaseRepository
    {
        private readonly Collection _collection;
        public FirestoreDb _firestoreDb;

        public FirestoreRepository(Collection collection)
        {
            // This should live in the appsetting file and injected - This is just an example.
            _collection = collection;
            var filepath = @"C:\Users\PieterLi\Downloads\test-6a89e-firebase-adminsdk-f41k9-5d045d5ead.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);
            _firestoreDb = FirestoreDb.Create("test-6a89e");

        }

        /// <inheritdoc />
        public async Task<List<T>> GetAllAsync<T>() where T : IBaseFirestoreData
        {
            Query query = _firestoreDb.Collection(_collection.ToString());
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

        /// <inheritdoc />
        public async Task<object> GetAsync<T>(string id) where T : IBaseFirestoreData
        {
            var docRef = _firestoreDb.Collection(_collection.ToString()).Document(id);
            var snapshot = await docRef.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                var entity = snapshot.ConvertTo<T>();
                entity.Id = snapshot.Id;
                return entity;
            }

            return null;
        }

        /// <inheritdoc />
        public async Task<T> AddAsync<T>(T entity) where T : IBaseFirestoreData
        {
            var colRef = _firestoreDb.Collection(_collection.ToString());
            var doc = await colRef.AddAsync(entity);
            // GO GET RECORD FROM DATABASE:
            // return (T) await GetAsync(entity); 
            return entity;
        }

        /// <inheritdoc />
        public async Task<T> UpdateAsync<T>(T entity) where T : IBaseFirestoreData
        {
            var recordRef = _firestoreDb.Collection(_collection.ToString()).Document(entity.Id);
            await recordRef.SetAsync(entity, SetOptions.MergeAll);
            // GO GET RECORD FROM DATABASE:
            // return (T)await GetAsync(entity);
            return entity;
        }

        /// <inheritdoc />
        public async Task DeleteAsync<T>(T entity) where T : IBaseFirestoreData
        {
            var recordRef = _firestoreDb.Collection(_collection.ToString()).Document(entity.Id);
            await recordRef.DeleteAsync();
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
