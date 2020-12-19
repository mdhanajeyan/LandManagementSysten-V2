using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace LandBankManagement.Data.Services
{
    public interface IDataService : IDisposable
    {

        Task<Company> GetCompanyAsync(long id);
        Task<IList<Company>> GetCompaniesAsync(int skip, int take, DataRequest<Company> request);
        Task<IList<Company>> GetCompanyKeysAsync(int skip, int take, DataRequest<Company> request);
        Task<int> GetCompaniesCountAsync(DataRequest<Company> request);
        Task<int> UpdateCompanyAsync(Company company);
        Task<int> DeleteCompanyAsync(params Company[] company);
        Task<int> UploadCompanyDocumentsAsync(List<CompanyDocuments> company);
        Task<int> DeleteCompanyDocumentAsync(CompanyDocuments documents);
        Task<IList<Company>> GetCompaniesAsync();
        List<CompanyDocuments> GetCompanyDocumentsAsync(Guid id);

        // Task<int> AddVendorAsync(Vendor model);
        Task<Vendor> GetVendorAsync(long id);
        Task<IList<Vendor>> GetVendorsAsync(DataRequest<Vendor> request);
        Task<IList<Vendor>> GetVendorsAsync(int skip, int take, DataRequest<Vendor> request);
        Task<int> GetVendorsCountAsync(DataRequest<Vendor> request);
        Task<int> UpdateVendorAsync(Vendor model);
        Task<int> DeleteVendorAsync(Vendor model);
        Task<int> UploadVendorDocumentsAsync(List<VendorDocuments> company);
        Task<int> DeleteVendorDocumentAsync(VendorDocuments documents);

        //  Task<int> AddPartyAsync(Party model);
        Task<Party> GetPartyAsync(long id);
        Task<IList<Party>> GetPartiesAsync(DataRequest<Party> request);
        Task<IList<Party>> GetPartiesAsync(int skip, int take, DataRequest<Party> request);
        Task<int> GetPartiesCountAsync(DataRequest<Party> request);
        Task<int> UpdatePartyAsync(Party model);
        Task<int> DeletePartyAsync(Party model);
        Task<int> UploadPartyDocumentsAsync(List<PartyDocuments> company);
        Task<int> DeletePartyDocumentAsync(PartyDocuments documents);

        Task<int> AddExpenseHeadAsync(ExpenseHead model);
        Task<ExpenseHead> GetExpenseHeadAsync(long id);
        // Task<IList<ExpenseHead>> GetExpenseHeadsAsync(DataRequest<ExpenseHead> request);
        Task<IList<ExpenseHead>> GetExpenseHeadsAsync(int skip, int take, DataRequest<ExpenseHead> request);
        Task<int> GetExpenseHeadsCountAsync(DataRequest<ExpenseHead> request);
        Task<int> UpdateExpenseHeadAsync(ExpenseHead model);
        Task<int> DeleteExpenseHeadAsync(ExpenseHead model);

        Task<int> AddTalukAsync(Taluk model);
        Task<Taluk> GetTalukAsync(long id);
        Task<IList<Taluk>> GetTaluksAsync(DataRequest<Taluk> request);
        Task<IList<Taluk>> GetTaluksAsync(int skip, int take, DataRequest<Taluk> request);
        Task<int> GetTaluksCountAsync(DataRequest<Taluk> request);
        Task<int> UpdateTalukAsync(Taluk model);
        Task<int> DeleteTalukAsync(Taluk model);
        Dictionary<int, string> GetTalukOptions();

        Task<int> AddHobliAsync(Hobli model);
        Task<Hobli> GetHobliAsync(long id);
        Task<IList<Hobli>> GetHoblisAsync(DataRequest<Hobli> request);
        Task<IList<Hobli>> GetHoblisAsync(int skip, int take, DataRequest<Hobli> request);
        Task<int> GetHoblisCountAsync(DataRequest<Hobli> request);
        Task<int> UpdateHobliAsync(Hobli model);
        Task<int> DeleteHobliAsync(Hobli model);
        Dictionary<int, string> GetHobliOptions();
        Dictionary<int, string> GetAccountTypeOptions();
        Task<int> AddVillageAsync(Village model);
        Task<Village> GetVillageAsync(long id);
        Task<IList<Village>> GetVillagesAsync(DataRequest<Village> request);
        Task<IList<Village>> GetVillagesAsync(int skip, int take, DataRequest<Village> request);
        Task<int> GetVillagesCountAsync(DataRequest<Village> request);
        Task<int> UpdateVillageAsync(Village model);
        Task<int> DeleteVillageAsync(Village model);
        Dictionary<int, string> GetVillageOptions();
        Dictionary<int, string> GetCompanyOptions();
        Task<int> AddAccountTypeAsync(AccountType model);
        Task<AccountType> GetAccountTypeAsync(long id);
        Task<IList<AccountType>> GetAccountTypesAsync(DataRequest<AccountType> request);
        Task<IList<AccountType>> GetAccountTypesAsync(int skip, int take, DataRequest<AccountType> request);
        Task<int> GetAccountTypesCountAsync(DataRequest<AccountType> request);
        Task<int> UpdateAccountTypeAsync(AccountType model);
        Task<int> DeleteAccountTypeAsync(AccountType model);

        Task<int> AddBankAccountAsync(BankAccount model);
        Task<BankAccount> GetBankAccountAsync(long id);
        Task<IList<BankAccount>> GetBankAccountsAsync(DataRequest<BankAccount> request);
        Task<IList<BankAccount>> GetBankAccountsAsync(int skip, int take, DataRequest<BankAccount> request);
        Task<int> GetBankAccountsCountAsync(DataRequest<BankAccount> request);
        Task<int> UpdateBankAccountAsync(BankAccount model);
        Task<int> DeleteBankAccountAsync(BankAccount model);

        Task<int> AddCashAccountAsync(CashAccount model);
        Task<CashAccount> GetCashAccountAsync(long id);
        Task<IList<CashAccount>> GetCashAccountsAsync(DataRequest<CashAccount> request);
        Task<IList<CashAccount>> GetCashAccountsAsync(int skip, int take, DataRequest<CashAccount> request);
        Task<int> GetCashAccountsCountAsync(DataRequest<CashAccount> request);
        Task<int> UpdateCashAccountAsync(CashAccount model);
        Task<int> DeleteCashAccountAsync(CashAccount model);


        Task<int> AddCheckListAsync(CheckList model);
        Task<CheckList> GetCheckListAsync(long id);
        Task<IList<CheckList>> GetCheckListsAsync(DataRequest<CheckList> request);
        Task<IList<CheckList>> GetCheckListsAsync(int skip, int take, DataRequest<CheckList> request);
        Task<int> GetCheckListsCountAsync(DataRequest<CheckList> request);
        Task<int> UpdateCheckListAsync(CheckList model);
        Task<int> DeleteCheckListAsync(CheckList model);

        Task<int> AddDocumentTypeAsync(DocumentType model);
        Task<DocumentType> GetDocumentTypeAsync(long id);
        Task<IList<DocumentType>> GetDocumentTypesAsync(DataRequest<DocumentType> request);
        Task<IList<DocumentType>> GetDocumentTypesAsync(int skip, int take, DataRequest<DocumentType> request);
        Task<int> GetDocumentTypesCountAsync(DataRequest<DocumentType> request);
        Task<int> UpdateDocumentTypeAsync(DocumentType model);
        Task<int> DeleteDocumentTypeAsync(DocumentType model);


        Task<int> AddPropertyAsync(Property model);
        Task<Property> GetPropertyAsync(long id);
        Task<IList<Property>> GetPropertiesAsync(DataRequest<Property> request);
        Task<IList<Property>> GetPropertiesAsync(int skip, int take, DataRequest<Property> request);
        Task<int> GetPropertiesCountAsync(DataRequest<Property> request);
        Task<int> UpdatePropertyAsync(Property model);
        Task<int> DeletePropertyAsync(Property model);
        Task<int> AddPropertyParty(List<PropertyParty> propertyParties);
        Task<int> AddPropPaySchedule(List<PropPaySchedule> schedules, decimal Sale1, decimal Sale2);
        Task<PropertyCostDetails> GetPropertyCostDetails(int propertyId);
        Task<int> UploadPropertyDocumentsAsync(List<PropertyDocuments> model);
        Task<int> DeletePropertyDocumentAsync(PropertyDocuments documents);
        Task<List<Property>> GetPropertyByGroupGuidAsync(Guid guid);

        Task<int> AddPropertyTypeAsync(PropertyType model);
        Task<PropertyType> GetPropertyTypeAsync(long id);
        Task<IList<PropertyType>> GetPropertyTypesAsync(DataRequest<PropertyType> request);
        Task<IList<PropertyType>> GetPropertyTypesAsync(int skip, int take, DataRequest<PropertyType> request);
        Task<int> GetPropertyTypesCountAsync(DataRequest<PropertyType> request);
        Task<int> UpdatePropertyTypeAsync(PropertyType model);
        Task<int> DeletePropertyTypeAsync(PropertyType model);
        Task<List<PropertyParty>> GetPartiesOfProperty(int propertyId);
        Task<int> DeletePropertyPartyAsync(PropertyParty model);

        Task<int> AddPropCheckListMasterAsync(PropCheckListMaster model);
        Task<PropCheckListMaster> GetPropCheckListMasterAsync(long id);
        Task<IList<PropCheckListMaster>> GetPropCheckListMastersAsync(DataRequest<PropCheckListMaster> request);
        Task<IList<PropCheckListMaster>> GetPropCheckListMastersAsync(int skip, int take, DataRequest<PropCheckListMaster> request);
        Task<int> GetPropCheckListMastersCountAsync(DataRequest<PropCheckListMaster> request);
        Task<int> UpdatePropCheckListMasterAsync(PropCheckListMaster model);
        Task<int> DeletePropCheckListMasterAsync(PropCheckListMaster model);


        Task<int> AddFundTransferAsync(FundTransfer model);
        Task<FundTransfer> GetFundTransferAsync(long id);
        Task<IList<FundTransfer>> GetFundTransfersAsync(DataRequest<FundTransfer> request);
        Task<IList<FundTransfer>> GetFundTransfersAsync(int skip, int take, DataRequest<FundTransfer> request);
        Task<int> GetFundTransfersCountAsync(DataRequest<FundTransfer> request);
        Task<int> UpdateFundTransferAsync(FundTransfer model);
        Task<int> DeleteFundTransferAsync(FundTransfer model);

        Task<int> AddReceiptAsync(Receipt model);
        Task<Receipt> GetReceiptAsync(long id);
        Task<IList<Receipt>> GetReceiptsAsync(DataRequest<Receipt> request);
        Task<IList<Receipt>> GetReceiptsAsync(int skip, int take, DataRequest<Receipt> request);
        Task<int> GetReceiptsCountAsync(DataRequest<Receipt> request);
        Task<int> UpdateReceiptAsync(Receipt model);
        Task<int> DeleteReceiptAsync(Receipt model);

        Task<int> AddPaymentAsync(Payment model);
        Task<Payment> GetPaymentAsync(long id);
        Task<IList<Payment>> GetPaymentsAsync(DataRequest<Payment> request);
        Task<IList<Payment>> GetPaymentsAsync(int skip, int take, DataRequest<Payment> request);
        Task<int> GetPaymentsCountAsync(DataRequest<Payment> request);
        Task<int> UpdatePaymentAsync(Payment model);
        Task<int> DeletePaymentAsync(Payment model);
        Task<int> DeletePaymentListAsync(PaymentList model);
        Task<int> AddUserRoleAsync(UserRole model);
        Task<UserRole> GetUserRoleAsync(long id);
        Task<IList<UserRole>> GetUserRolesAsync(DataRequest<UserRole> request);
        Task<IList<UserRole>> GetUserRolesAsync(int skip, int take, DataRequest<UserRole> request);
        Task<int> GetUserRolesCountAsync(DataRequest<UserRole> request);
        Task<int> UpdateUserRoleAsync(UserRole model);
        Task<int> DeleteUserRoleAsync(UserRole model);

        Task<IList<UserRole>> GetUserRolesForUserAsync(int userId);
        Task<int> AddUserRoleForUserAsync(List<UserRole> model, int userId);
        UserInfo AuthenticateUser(string username, string password);

        Task<int> AddRoleAsync(Role model);
        Task<Role> GetRoleAsync(long id);
        Task<IList<Role>> GetRolesAsync(DataRequest<Role> request);
        Task<IList<Role>> GetRolesAsync(int skip, int take, DataRequest<Role> request);
        Task<int> GetRolesCountAsync(DataRequest<Role> request);
        Task<int> UpdateRoleAsync(Role model);
        Task<int> DeleteRoleAsync(Role model);
 		Task<IList<Role>> GetRolesAsync();
        Task<int> AddUserInfoAsync(UserInfo model);
        Task<UserInfo> GetUserInfoAsync(long id);
        Task<IList<UserInfo>> GetUserInfosAsync(DataRequest<UserInfo> request);
        Task<IList<UserInfo>> GetUserInfosAsync(int skip, int take, DataRequest<UserInfo> request);
        Task<int> GetUserInfosCountAsync(DataRequest<UserInfo> request);
        Task<int> UpdateUserInfoAsync(UserInfo model);
        Task<int> DeleteUserInfoAsync(UserInfo model);

        Task<int> AddRolePermissionAsync(RolePermission model);
        Task<RolePermission> GetRolePermissionAsync(long id);
        Task<IList<RolePermission>> GetRolePermissionsAsync(DataRequest<RolePermission> request);
        Task<IList<RolePermission>> GetRolePermissionsAsync(int skip, int take, DataRequest<RolePermission> request);
        Task<int> GetRolePermissionsCountAsync(DataRequest<RolePermission> request);
        Task<int> UpdateRolePermissionAsync(RolePermission model);
        Task<int> DeleteRolePermissionAsync(RolePermission model);
        IList<RolePermission> GetRolePermisions(int roleId);
        Task<List<RolePermission>> GetRolePermissionsByRoleIDAsync(int roleId);
        Task<int> AddRolePermissionsAsync(List<RolePermission> models);

        Dictionary<int, string> GetExpenseHeadOptions();
        Dictionary<int, string> GetPartyOptions();
        Dictionary<int, string> GetDocumentTypeOptions();
        Dictionary<int, string> GetPropertyOptions();
        Dictionary<int, string> GetCashOptions();
        Dictionary<int, string> GetBankOptions();
        Dictionary<int, string> GetVendorOptions();
        Dictionary<int, string> GetReportingToOptions();
        Dictionary<int, string> GetGenderOptions();
        Dictionary<int, string> GetPartyOptions(string party);
        Dictionary<int, string> GetPropertyTypeOptions();
        Dictionary<int, string> GetRoleOptions();
        Dictionary<int, string> GetVendorOptions(string party);
        Dictionary<int, string> GetCheckListOptions();

        Task<int> AddPropertyCheckListAsync(PropertyCheckList model);
        Task<PropertyCheckList> GetPropertyCheckListAsync(long id);
        Task<IList<PropertyCheckList>> GetPropertyCheckListAsync(DataRequest<PropertyCheckList> request);
        Task<IList<PropertyCheckList>> GetPropertyCheckListAsync(int skip, int take, DataRequest<PropertyCheckList> request);
        Task<int> GetPropertyCheckListCountAsync(DataRequest<PropertyCheckList> request);
        Task<int> UpdatePropertyCheckListAsync(PropertyCheckList model);
        Task<int> DeletePropertyCheckListAsync(PropertyCheckList model);
        Task<int> DeletePropertyCheckListDocumentAsync(PropertyCheckListDocuments documents);
        Task<int> DeletePropertyCheckListVendorAsync(PropertyCheckListVendor vendor);
        List<PropertyCheckListVendor> GetPropertyCheckListVendors(int id);

        Task<int> UpdatePropertyCheckListStatusAsync(int id,int status,string remarks);

    }
}
