using LandBankManagement.Models;
using LandBankManagement.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace LandBankManagement.ViewModels
{
    public class PropertyCheckListList
    {
        public int CompanyID { get; set; }
        public string Name { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }

        public IEnumerable<PropertyCheckListList> Children { get; set; }
    }
    public class PropertyCheckListListViewModel : GenericListViewModel<PropertyCheckListModel>
    {
        private IList<PropertyCheckListList> _propertyModelList = null;
        public IList<PropertyCheckListList> PropertyModelCheckList
        {
            get => _propertyModelList;
            set => Set(ref _propertyModelList, value);
        }
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
            var items = new List<PropertyCheckListList>
            {
                new PropertyCheckListList
                {
                    Name="Property CheckList 1",
                    Email="test@test.com",
                    PhoneNo="9087654400",
                    CompanyID=1,
                    City="CBE",
                    Children=new List<PropertyCheckListList>
                    {
                    new PropertyCheckListList
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
            PropertyModelCheckList = items;
        }

    }
}
