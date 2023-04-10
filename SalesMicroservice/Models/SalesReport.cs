using DadaRepositories.Interfaces;
using System.Collections.Generic;

namespace SalesMicroservice.Models
{
    
    public class SalesReport : IBaseFirestoreData
    {
        public string Id { get; set; }
        public int Correlative { get; set; }
        public int CustomerId { get; set; }
        //public Customer Customer { get; set; }
        public string Description { get; set; }
        //public List<SalesReportDetail> Details { get; set; }
    }
}
