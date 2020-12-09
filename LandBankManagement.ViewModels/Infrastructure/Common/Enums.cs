using System;
using System.Collections.Generic;
using System.Text;

namespace LandBankManagement.Enums
{
    public enum NavigationScreen
    {
        Default = 0,
        Company = 1,
        Vendor=2,
        Bank=3,
        Cash=4,
        ExpenseHead=5,
        Taluk=6,
        Hobli=7,
        Village=8,
        PropChecklistMaster=9,
        PropertyType=10,
        Property=11,
        PropertyCheckList=12,
        PropertyDeals=13,
        MergeProperties=14,
        Payments=15,
        FundTransfer=16,
        Receipt=17,
        SaleDeal=18,
        UserInfo=19,
        UserRole=20,
        Role=21,
        RolePermission=22,
        ViewLogs=23,
        CurrentMonthPaymentReport=24,
        PropertyChecklistReport=25,
        PartyStatementOfAccountReport=26,
        PostDatedCheckReport=27,
        DealReport=28,
        Party=29,
    }

    public enum Role
    {
        Admin = 2,
        Accounts,
        Legal
    }
}
