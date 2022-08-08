using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess
{
    /// <summary>
    /// Used at design-time to enable Package Manager Console commands like
    /// Add-Migration, Update-Database etc.
    /// </summary>
    public class HomeworkDbContextFactory : IDesignTimeDbContextFactory<HomeworkDBContext>
    {
        public HomeworkDBContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<HomeworkDBContext>()
                .UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ORM_EF;Integrated Security=True")
                .Options;

            return new HomeworkDBContext(options);
        }
    }
}
