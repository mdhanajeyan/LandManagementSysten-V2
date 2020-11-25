using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LandBankManagement.Data.Services
{
    public class SQLServerDb : DbContext, IDataSource
    {
        private string _connectionString = null;

        public SQLServerDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Party> Parties { get; set; }
        public DbSet<Taluk> Taluks { get; set; }
        public DbSet<Hobli> Hoblis { get; set; }
        public DbSet<Village> Villages { get; set; }
        public DbSet<AccountType> AccountTypes { get; }
        public DbSet<BankAccount> BankAccounts { get; }
        public DbSet<CashAccount> CashAccounts { get; }
        public DbSet<DocumentType> DocumentTypes { get; }

		public DbSet<ExpenseHead> ExpenseHeads { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RemovePluralizingTableNameConvention();
        }

    }

    public static class ModelBuilderExtensions
    {
        public static void RemovePluralizingTableNameConvention(this ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.DisplayName();
            }
        }
    }
}
