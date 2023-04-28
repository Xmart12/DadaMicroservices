using System.Data.Entity;

namespace DadaRepositories.Contexts
{
    public class MySqlDbContext : DbContext
    {
        public MySqlDbContext() : base("")
        {
            
        }
    }
}
