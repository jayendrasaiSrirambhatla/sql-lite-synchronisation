using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
namespace sqliteinwebapi
{
    public class CustomerDbContext: DbContext
    {
        public string DbPath { get; }

        public CustomerDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "blogging.db");
            //Console.WriteLine(DbPath);
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        // sql lite database context
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
         public DbSet<Customer>? Customers { get; set; }
    }

    
    
}
