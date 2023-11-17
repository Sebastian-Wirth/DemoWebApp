using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DemoWebApp.Models;

namespace DemoWebApp.Data
{
    public class DemoDbContext : DbContext
    {
        public DemoDbContext (DbContextOptions<DemoDbContext> options)
            : base(options)
        {
        }

        public DbSet<DemoWebApp.Models.Product> Product { get; set; } = default!;
    }
}
