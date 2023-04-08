using Google.Cloud.Firestore;
using SalesMicroservice.Interfaces;
using System.Collections.Generic;

namespace SalesMicroservice.Models
{
    [FirestoreData]
    public class SalesReport : IBaseFirestoreData
    {
        public string Id { get; set; }
        public int Correlative { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public string Description { get; set; }
        public List<SalesReportDetail> Details { get; set; }
    }
}
