using LandBankManagement.Models;
using LandBankManagement.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
        private IList<PropertyCheckListModel> _propertyModelList = null;
        public IList<PropertyCheckListModel> PropertyModelCheckList
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

        public async Task LoadData()
        {
            var items = new List<PropertyCheckListModel>
            {
                new PropertyCheckListModel
                {
                    PropertyName="Property CheckList 1",
                    PropertyDescription="test@test.com",
                    LandAreaInSqft="9087654400",
                    CompanyID=1,
                    AKarabAreaInAcres="CBE"
                }
            };
            Items = items;
        }

    }
}
