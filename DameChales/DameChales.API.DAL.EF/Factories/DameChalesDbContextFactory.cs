using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DameChales.API.DAL.EF.Factories
{
    public class DameChalesDbContextFactory : IDesignTimeDbContextFactory<DameChalesDbContext>
    {
        public DameChalesDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<DameChalesDbContextFactory>(optional: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<DameChalesDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            return new DameChalesDbContext(optionsBuilder.Options);
        }
    }
}
