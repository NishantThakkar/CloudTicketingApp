using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketingAppCloud.DBModels
{
    public class TicketingDbContext : DbContext
    {
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }
        public TicketingDbContext(DbContextOptions<TicketingDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
