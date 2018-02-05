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
            var optionsBuilder = new DbContextOptionsBuilder<ThreadContext>();
            optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionString"));

            using (var db = new ThreadContext(optionsBuilder.Options))
            {
                ThreadSeeder.Seed(db);
            }
        }
    }
}
