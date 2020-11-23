using System;

using Microsoft.EntityFrameworkCore;

namespace LandBankManagement.Data.Services
{
    public class SQLiteDb : DbContext, IDataSource
    {
        private string _connectionString = null;

        public SQLiteDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Vendor> Vendors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }

       
    }
}
