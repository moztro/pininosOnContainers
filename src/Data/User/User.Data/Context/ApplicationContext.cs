using Containers.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Data.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Person> Persons { set; get; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            this.Database.Migrate();
            this.Database.EnsureCreated();
        }        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);            
        }

    }
}
