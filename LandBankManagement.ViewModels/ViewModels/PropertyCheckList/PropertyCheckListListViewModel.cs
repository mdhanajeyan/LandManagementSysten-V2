using LandBankManagement.Models;
using LandBankManagement.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace LandBankManagement.ViewModels
{
    public class PropertyCheckListListViewModel : GenericListViewModel<PropertyCheckListModel>
    {
        public PropertyCheckListListViewModel(ICommonServices commonServices) : base(commonServices)
        {
        }

        protected override void OnDeleteSelection()
        {

        }

        protected override void OnNew()
        {

        }

        protected override void OnRefresh()
        {

        }

        public void LoadData()
        {
            var items = new List<PropertyCheckListModel>
            {
                new PropertyCheckListModel
                {
                    Name="Property CheckList 1",
                    Email="test@test.com",
                    PhoneNo="9087654400",
                    CompanyID=1,
                    City="CBE",
                    Children=new List<PropertyCheckListModel>
                    {
                    new PropertyCheckListModel
                   {
                    Name="Child Property CheckList 1",
                    Email="cild_test@test.com",
                    PhoneNo="9087654400",
                    CompanyID=2,
                    City="CBE Child"
                        }
                    }
                }
            };
            Items = items;
        }
    }
}
