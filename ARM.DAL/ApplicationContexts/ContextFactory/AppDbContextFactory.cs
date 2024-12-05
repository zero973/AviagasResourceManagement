using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ARM.DAL.ApplicationContexts.ContextFactory;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{

    private const string ConnectionString =
        "Host=ep-polished-forest-a2nftm1d-pooler.eu-central-1.aws.neon.tech; Port=5432; Database=aviagas; User ID=neondb_owner; Password=MLaqDySPp48B";
    
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(ConnectionString, 
            opt => opt.MigrationsAssembly(typeof(AppDbContext).Assembly));
        
        return new AppDbContext(optionsBuilder.Options, null);
    }
}