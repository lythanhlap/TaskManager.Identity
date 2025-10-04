using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TaskManager.Identity.Persistence.EFCore;

public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
{
    public IdentityDbContext CreateDbContext(string[] args)
    {
        var opts = new DbContextOptionsBuilder<IdentityDbContext>()
            .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TaskManager;Trusted_Connection=True")
            .Options;
        return new IdentityDbContext(opts);
    }
}