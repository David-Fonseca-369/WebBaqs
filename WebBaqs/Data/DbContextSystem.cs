using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBaqs.Data.Mapping;
using WebBaqs.Entities;

namespace WebBaqs.Data
{
    public class DbContextSystem : DbContext
    {
        public DbSet<BAQ> BAQs { get; set; }

        public DbContextSystem(DbContextOptions<DbContextSystem> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new BAQMap());
          

        }
    }
}
