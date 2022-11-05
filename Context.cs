using System.Data.Entity;

namespace VeterinaryPractice
{
    public class VeterinaryPracticeContext : DbContext
    {
        public DbSet<Model.Practice> Practice { get; set; }
        public DbSet<Model.Vet> Vet { get; set; }
        public DbSet<Model.Owner> Owner { get; set; }
        public DbSet<Model.Pet> Pet { get; set; }
        public DbSet<Model.Visit> Visit { get; set; }
    }
}
