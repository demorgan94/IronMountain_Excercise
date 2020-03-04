using System;
using IronMountain_Excercise.Models;
using Microsoft.EntityFrameworkCore;

namespace IronMountain_Excercise.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public DbSet<ImageFile> ImageFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ImageFile>().ToTable("ImageFile");
        }
    }
}
