using DadaRepositories.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DadaRepositories.Contexts
{
    public class DadaDbContext : DbContext
    {
        public DadaDbContext(DbContextOptions<DadaDbContext> options) : base(options)
        {
            
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<SalesReport> SalesReports { get; set; }
        public virtual DbSet<SalesReportDetail> SalesReportDetails { get; set; }
        public virtual DbSet<InventoryMovement> InventoryMovements { get; set; }
        public virtual DbSet<InventoryMovementDetail> InventoryMovementDetails { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
        public virtual DbSet<PurchaseDetail> PurchaseDetails { get; set; }
        public virtual DbSet<WorkOrder> WorkOrders { get; set; }
        public virtual DbSet<WorkOrderDetail> WorkOrderDetails { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            
        }
    }
}
