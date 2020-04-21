using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.RepositoryContext
{
    public class myContext : IdentityDbContext
    {
        public myContext(DbContextOptions<myContext> options) : base(options) { }

        public DbSet<Department> department { get; set; }
        public DbSet<Employee> employee { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region "Seed Data"

            builder.Entity<IdentityRole>().HasData(
                new { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new { Id = "2", Name = "Employee", NormalizedName = "EMPLOYEE" }
            );

            #endregion
        }
    }
}
