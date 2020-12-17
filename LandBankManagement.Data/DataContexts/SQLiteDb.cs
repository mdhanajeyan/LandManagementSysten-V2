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
       public DbSet<CompanyDocuments> CompanyDocuments { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<VendorDocuments> VendorDocuments { get; set; }
        public DbSet<Party> Parties { get; set; }
        public DbSet<PartyDocuments> PartyDocuments { get; set; }
        public DbSet<ExpenseHead> ExpenseHeads { get; set; }
        public DbSet<Taluk> Taluks { get; set; }
        public DbSet<Hobli> Hoblis { get; set; }
        public DbSet<Village> Villages { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<CashAccount> CashAccounts { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<CheckList> CheckLists { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<PropCheckListMaster> PropCheckListMasters { get; set; }
        public DbSet<FundTransfer> FundTransfers { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<PropertyParty> PropertyParty { get; set; }
        public DbSet<PropPaySchedule> PropPaySchedules { get; set; }
        public DbSet<ScreenList> ScreenList { get; set; }
        public DbSet<PropertyDocuments> PropertyDocuments { get; set; }

        public DbSet<PaymentList> paymentLists { get; set; }

        public DbSet<PropertyCheckList> PropertyCheckList { get; set; }
        public DbSet<PropertyCheckListDocuments> PropertyCheckListDocuments { get; set; }
        public DbSet<PropertyCheckListVendor> PropertyCheckListVendor { get; set; }
        public DbSet<CheckListOfProperty> CheckListOfProperty { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }

       
    }
}
