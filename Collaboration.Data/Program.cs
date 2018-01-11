using Collaboration.Data.Contexts;
using Collaboration.Data.Seed;
using Microsoft.EntityFrameworkCore;
using System;

namespace Collaboration.Data
{
    class Program
    {
        static void Main(string[] args)
        {           
            var dbHostName = Environment.GetEnvironmentVariable("SQLSERVER_HOST") ?? "localhost";
            Console.WriteLine($"SQL Server Host: {dbHostName}");
            var dbPassword = Environment.GetEnvironmentVariable("SQLSERVER_SA_PASSWORD") ?? "Password123";
            Console.WriteLine($"SQL Server Host: {dbPassword}");
            var connString = $"Data Source={dbHostName};Initial Catalog=Collaboration;User ID=sa;Password={dbPassword};";

            var optionsBuilder = new DbContextOptionsBuilder<ThreadContext>();
            optionsBuilder.UseSqlServer(connString);

            using (var db = new ThreadContext(optionsBuilder.Options))
            {
                ThreadSeeder.Seed(db);
            }
        }
    }
}
