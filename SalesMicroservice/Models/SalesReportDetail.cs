namespace SalesMicroservice.Models
{
    public class SalesReportDetail
    {
        public int Id { get; set; }
        public int SalesReportId { get; set; }
        public int Line { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public double Quantity { get; set; }
        public double UnitCost { get; set; }
        public double UnitPrice { get; set; }
    }
}
