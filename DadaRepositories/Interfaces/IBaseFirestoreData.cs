using Google.Cloud.Firestore;
using System.ComponentModel.DataAnnotations;

namespace DadaRepositories.Interfaces
{
    [FirestoreData]
    public interface IBaseFirestoreData
    {
        /// <summary>
        /// Gets and set the Id.
        /// </summary>
        [Required]
        public string Id { get; set; }
    }
}