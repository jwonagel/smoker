using Microsoft.EntityFrameworkCore;

namespace api.Model.Dababase
{
    public class SmokerDBContext : DbContext
    {

        public SmokerDBContext(DbContextOptions<SmokerDBContext> options) : base(options) { }

        public DbSet<Measurement> Measurements { get; set; }

    }
}