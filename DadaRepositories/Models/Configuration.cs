using DadaRepositories.Interfaces;

namespace DadaRepositories.Models
{
    public class Configuration : IBaseFirestoreData
    {
        public string Id { get; set; }
        public string LastId { get; set; }
    }
}
