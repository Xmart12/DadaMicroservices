using Google.Cloud.Firestore;

namespace DadaRepositories.Interfaces
{
    [FirestoreData]
    public interface IBaseFirestoreData
    {
        /// <summary>
        /// Gets and set the Id.
        /// </summary>
        public string Id { get; set; }
    }
}