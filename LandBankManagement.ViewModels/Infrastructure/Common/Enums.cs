using System;
using System.Collections.Generic;
using System.Text;

namespace LandBankManagement.Enums
{
    public enum NavigationScreen
    {
        Default = 0,
        Company = 1,
        Vendor,
        Bank,
        Cash,
        ExpenseHead,
        Taluk,
        Hobli,
        Village,
        PropChecklistMaster,
        PropertyType,
        Property,
        PropertyCheckList,
        PropertyDeals,
        MergeProperties,
        Payments,
        FundTransfer,
        Receipt,
        SaleDeal,
        UserInfo,
        UserRole,
        Role,
        RolePermission,
        ViewLogs,
        CurrentMonthPaymentReport,
        PropertyChecklistReport,
        PartyStatementOfAccountReport,
        PostDatedCheckReport,
        DealReport
    }

    public enum Role
    {
        Admin = 2,
        Accounts,
        Legal
    }
}
