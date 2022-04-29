using Microsoft.EntityFrameworkCore;

namespace CnbApp.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<CurrencyData> CurrencyDatas { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

}