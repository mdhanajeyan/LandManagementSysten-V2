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
        public DbSet<Party> Parties { get; set; }
		public DbSet<ExpenseHead> ExpenseHeads { get; set; }
        public DbSet<Taluk> Taluks { get; set; }
        public DbSet<Hobli> Hoblis { get; set; }
        public DbSet<Village> Villages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }

       
    }
}
