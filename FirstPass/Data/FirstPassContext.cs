using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FirstPass.Models
{
    public class FirstPassContext : DbContext
    {
        public FirstPassContext (DbContextOptions<FirstPassContext> options)
            : base(options)
        {
        }

        public DbSet<FirstPass.Models.Address> Address { get; set; }
    }
}
