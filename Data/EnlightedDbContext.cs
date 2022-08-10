using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnlightedWorkService.Data
{
    public class EnlightedDbContext : DbContext
    {
        public virtual DbSet<Floor> Floors { get; set; }
        public virtual DbSet<Fixture> Fixtures { get; set; }

        public EnlightedDbContext(DbContextOptions<EnlightedDbContext> options)
            : base(options)
        {
        }
    }
}
