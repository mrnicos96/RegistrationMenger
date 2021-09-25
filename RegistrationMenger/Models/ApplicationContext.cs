using System.Data.Entity;

namespace RegistrationMenger.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("DefaultConnection")
        {
        }
        public DbSet<Acceptance> Acceptances { get; set; }
        public DbSet<Nomenclature> Nomenclatures { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
    }
}
