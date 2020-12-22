using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LandBankManagement.Data;
using LandBankManagement.Data.Services;
using LandBankManagement.Models;
using LandBankManagement.Services.VirtualCollections;

namespace LandBankManagement.Services
{
    public class PropertyCheckListService: IPropertyCheckListService
    {
        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }

        public PropertyCheckListService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public async Task<int> AddPropertyCheckListAsync(PropertyCheckListModel model)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var property = new PropertyCheckList();
                if (property != null)
                {                 
                    UpdatePropertyCheckListFromModel(property, model);
                    property.PropertyGuid = Guid.NewGuid();
                    var propertyID = await dataService.AddPropertyCheckListAsync(property);
                    // model.Merge(await GetPropertyCheckListAsync(dataService, propertyID));
                    return propertyID;
                }
                return 0;
            }
        }

        static private async Task<PropertyCheckListModel> GetPropertyCheckListAsync(IDataService dataService, long id)
        {
            var item = await dataService.GetPropertyCheckListAsync(id);
            if (item != null)
            {
                //return await CreatePropertyCheckListModelAsync(item, includeAllFields: true);
                return await CreatePropertyCheckListModelWithDocsAsync(item);

            }
            return null;
        }

        public async Task<PropertyCheckListModel> GetPropertyCheckListAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetPropertyCheckListAsync(dataService, id);
            }
        }

        public async Task<IList<PropertyCheckListModel>> GetPropertyCheckListAsync(DataRequest<PropertyCheckList> request)
        {
            var collection = new PropertyCheckListCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<PropertyCheckListModel>> GetPropertyCheckListAsync(int skip, int take, DataRequest<PropertyCheckList> request)
        {
            var models = new List<PropertyCheckListModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetPropertyCheckListAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(await CreatePropertyCheckListModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }
      
        public async Task<int> GetPropertyCheckListCountAsync(DataRequest<PropertyCheckList> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetPropertyCheckListCountAsync(request);
            }
        }

        public async Task<int> UpdatePropertyCheckListAsync(PropertyCheckListModel model)
        {
            long id = model.PropertyCheckListId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var property = id > 0 ? await dataService.GetPropertyCheckListAsync(model.PropertyCheckListId) : new PropertyCheckList();
                if (property != null)
                {    
                    UpdatePropertyCheckListFromModel(property, model);
                    await dataService.UpdatePropertyCheckListAsync(property);
                    //model.Merge(await GetPropertyCheckListAsync(dataService, property.PropertyCheckListId));
                }
                return model.PropertyCheckListId;
            }
        }
        public async Task<int> UpdatePropertyCheckListStatusAsync(int id, int status, string remarks) {
            using (var dataService = DataServiceFactory.CreateDataService())
            {                
                 return   await dataService.UpdatePropertyCheckListStatusAsync(id,status,remarks);
            }
        }

        public async Task<int> DeletePropertyCheckListAsync(PropertyCheckListModel model)
        {
            var property = new PropertyCheckList { PropertyCheckListId = model.PropertyCheckListId };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeletePropertyCheckListAsync(property);
            }
        }

             
        public async Task<int> DeletePropertyDocumentAsync(PropertyCheckListDocumentsModel documents)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var doc = new PropertyCheckListDocuments();
                UpdateDocumentFromModel(doc, documents);
                return await dataService.DeletePropertyCheckListDocumentAsync(doc);
            }
        }

        public async Task<int> DeletePropertyVendorAsync(int proeprtyVendotId)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var doc = new PropertyCheckListVendor { CheckListVendorId= proeprtyVendotId};
                return await dataService.DeletePropertyCheckListVendorAsync(doc);
            }
        }

        public List<PropertyCheckListVendorModel> GetPropertyCheckListVendors(int id) {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var list = dataService.GetPropertyCheckListVendors(id);
                if (list != null && list.Count > 0) {
                    var vendors = new List<PropertyCheckListVendorModel>();
                    foreach (var obj in list) {
                        vendors.Add(new PropertyCheckListVendorModel
                        {
                            VendorId = obj.VendorId,
                            VendorName = obj.VendorName,
                            PropertyCheckListId = obj.PropertyCheckListId,
                            CheckListVendorId = obj.CheckListVendorId
                        });

                    }
                    return vendors;
                }
                return null;
            }
        }

        static public async Task<PropertyCheckListModel> CreatePropertyCheckListModelAsync(PropertyCheckList source, bool includeAllFields)
        {
            var model = new PropertyCheckListModel()
            {
                PropertyCheckListId = source.PropertyCheckListId,
                PropertyGuid = source.PropertyGuid,
                PropertyName = source.PropertyName,
                TalukId = source.TalukId,
                HobliId = source.HobliId,
                VillageId = source.VillageId,
                DocumentTypeId = source.DocumentTypeId,
                PropertyTypeId = source.PropertyTypeId,
                SurveyNo = source.SurveyNo,
                PropertyGMapLink = source.PropertyGMapLink,
                LandAreaInputAcres = source.LandAreaInputAcres.ToString(),
                LandAreaInputGuntas = source.LandAreaInputGuntas.ToString(),
                LandAreaInputAanas = source.LandAreaInputAanas.ToString(),
                LandAreaInAcres = source.LandAreaInAcres.ToString(),
                LandAreaInGuntas = source.LandAreaInGuntas.ToString(),
                LandAreaInSqMts = source.LandAreaInSqMts.ToString(),
                LandAreaInSqft = source.LandAreaInSqft.ToString(),
                AKarabAreaInputAcres = source.AKarabAreaInputAcres.ToString(),
                AKarabAreaInputGuntas = source.AKarabAreaInputGuntas.ToString(),
                AKarabAreaInputAanas = source.AKarabAreaInputAanas.ToString(),
                AKarabAreaInAcres = source.AKarabAreaInAcres.ToString(),
                AKarabAreaInGuntas = source.AKarabAreaInGuntas.ToString(),
                AKarabAreaInSqMts = source.AKarabAreaInSqMts.ToString(),
                AKarabAreaInSqft = source.AKarabAreaInSqft.ToString(),
                BKarabAreaInputAcres = source.BKarabAreaInputAcres.ToString(),
                BKarabAreaInputGuntas = source.BKarabAreaInputGuntas.ToString(),
                BKarabAreaInputAanas = source.BKarabAreaInputAanas.ToString(),
                BKarabAreaInAcres = source.BKarabAreaInAcres.ToString(),
                BKarabAreaInGuntas = source.BKarabAreaInGuntas.ToString(),
                BKarabAreaInSqMts = source.BKarabAreaInSqMts.ToString(),
                BKarabAreaInSqft = source.BKarabAreaInSqft.ToString(),
                CompanyID = source.CompanyID,
                CompanyName = source.CompanyName,
                VillageName = source.VillageName,
                PropertyDescription =source.PropertyDescription,
                CheckListMaster=source.CheckListMaster,
                Remarks=source.Remarks,
                Status=source.Status
            };

           

            return model;
        }

        static public async Task<PropertyCheckListModel> CreatePropertyCheckListModelWithDocsAsync(PropertyCheckList source)
        {
            var model = new PropertyCheckListModel()
            {
                PropertyCheckListId = source.PropertyCheckListId,
                PropertyGuid = source.PropertyGuid,
                PropertyName = source.PropertyName,
                TalukId = source.TalukId,
                HobliId = source.HobliId,
                VillageId = source.VillageId,
                DocumentTypeId = source.DocumentTypeId,
                PropertyTypeId = source.PropertyTypeId,
                SurveyNo = source.SurveyNo,
                PropertyGMapLink = source.PropertyGMapLink,
                LandAreaInputAcres = source.LandAreaInputAcres.ToString(),
                LandAreaInputGuntas = source.LandAreaInputGuntas.ToString(),
                LandAreaInputAanas = source.LandAreaInputAanas.ToString(),
                LandAreaInAcres = source.LandAreaInAcres.ToString(),
                LandAreaInGuntas = source.LandAreaInGuntas.ToString(),
                LandAreaInSqMts = source.LandAreaInSqMts.ToString(),
                LandAreaInSqft = source.LandAreaInSqft.ToString(),
                AKarabAreaInputAcres = source.AKarabAreaInputAcres.ToString(),
                AKarabAreaInputGuntas = source.AKarabAreaInputGuntas.ToString(),
                AKarabAreaInputAanas = source.AKarabAreaInputAanas.ToString(),
                AKarabAreaInAcres = source.AKarabAreaInAcres.ToString(),
                AKarabAreaInGuntas = source.AKarabAreaInGuntas.ToString(),
                AKarabAreaInSqMts = source.AKarabAreaInSqMts.ToString(),
                AKarabAreaInSqft = source.AKarabAreaInSqft.ToString(),
                BKarabAreaInputAcres = source.BKarabAreaInputAcres.ToString(),
                BKarabAreaInputGuntas = source.BKarabAreaInputGuntas.ToString(),
                BKarabAreaInputAanas = source.BKarabAreaInputAanas.ToString(),
                BKarabAreaInAcres = source.BKarabAreaInAcres.ToString(),
                BKarabAreaInGuntas = source.BKarabAreaInGuntas.ToString(),
                BKarabAreaInSqMts = source.BKarabAreaInSqMts.ToString(),
                BKarabAreaInSqft = source.BKarabAreaInSqft.ToString(),
                CompanyID = source.CompanyID,
                CompanyName=source.CompanyName,
                VillageName=source.VillageName,
                PropertyDescription = source.PropertyDescription,
                CheckListMaster = source.CheckListMaster
            };
            if (source.PropertyCheckListDocuments != null && source.PropertyCheckListDocuments.Count > 0)
            {
                var docList = new ObservableCollection<PropertyCheckListDocumentsModel>();
                foreach (var obj in source.PropertyCheckListDocuments)
                {
                    docList.Add(new PropertyCheckListDocumentsModel
                    {
                        PropertyCheckListBlobId = obj.PropertyCheckListBlobId,
                        guid = obj.PropertyGuid,
                        ImageBytes = obj.FileBlob,
                        FileName = obj.FileName,
                        ContentType = obj.FileType,
                        FileCategoryId = obj.FileCategoryId,
                        UploadTime = obj.UploadTime,
                        DueDate = obj.DueDate,
                        ActualCompletionDate = obj.ActualCompletionDate,
                        Remarks = obj.Remarks
                    });
                }
                model.PropertyCheckListDocuments =docList;
            }

            if (source.CheckListOfProperties != null && source.CheckListOfProperties.Count > 0)
            {
                var checkList = new ObservableCollection<CheckListOfPropertyModel>();
                foreach (var obj in source.CheckListOfProperties)
                {
                    checkList.Add(new CheckListOfPropertyModel
                    {
                        CheckListPropertyId = obj.CheckListPropertyId,
                        PropertyCheckListId = obj.PropertyCheckListId,
                        CheckListId = obj.CheckListId,
                        Mandatory = obj.Mandatory
                    });
                }
                model.CheckListOfProperties = checkList;
            }

            if (source.PropertyCheckListVendors != null && source.PropertyCheckListVendors.Count > 0)
            {
                var vendors = new ObservableCollection<PropertyCheckListVendorModel>();
                foreach (var obj in source.PropertyCheckListVendors)
                {
                    vendors.Add(new PropertyCheckListVendorModel
                    {
                        VendorId = obj.VendorId,
                        VendorName = obj.VendorName,
                        PropertyCheckListId = obj.PropertyCheckListId,
                        CheckListVendorId = obj.CheckListVendorId
                    });
                }
                model.PropertyCheckListVendors = vendors;
            }

            return model;
        }

        

        private void UpdatePropertyCheckListFromModel(PropertyCheckList target, PropertyCheckListModel source)
        {
            target.PropertyCheckListId = source.PropertyCheckListId;
            target.PropertyGuid = source.PropertyGuid;
            target.PropertyName = source.PropertyName;
            target.TalukId = source.TalukId;
            target.HobliId = source.HobliId;
            target.VillageId = source.VillageId;
            target.DocumentTypeId = source.DocumentTypeId;
            target.PropertyTypeId = source.PropertyTypeId;
            target.SurveyNo = source.SurveyNo;
            target.PropertyGMapLink = source.PropertyGMapLink;
            target.LandAreaInputAcres = Convert.ToDecimal(string.IsNullOrEmpty(source.LandAreaInputAcres) ? "0" : source.LandAreaInputAcres);
            target.LandAreaInputGuntas = Convert.ToDecimal(string.IsNullOrEmpty(source.LandAreaInputGuntas) ? "0" : source.LandAreaInputGuntas);
            target.LandAreaInputAanas = Convert.ToDecimal(string.IsNullOrEmpty(source.LandAreaInputAanas) ? "0" : source.LandAreaInputAanas);
            target.LandAreaInAcres = Convert.ToDecimal(string.IsNullOrEmpty(source.LandAreaInAcres) ? "0" : source.LandAreaInAcres);
            target.LandAreaInGuntas = Convert.ToDecimal(string.IsNullOrEmpty(source.LandAreaInGuntas) ? "0" : source.LandAreaInGuntas);
            target.LandAreaInSqMts = Convert.ToDecimal(string.IsNullOrEmpty(source.LandAreaInSqMts) ? "0" : source.LandAreaInSqMts);
            target.LandAreaInSqft = Convert.ToDecimal(string.IsNullOrEmpty(source.LandAreaInSqft) ? "0" : source.LandAreaInSqft);
            target.AKarabAreaInputAcres = Convert.ToDecimal(string.IsNullOrEmpty(source.AKarabAreaInputAcres) ? "0" : source.AKarabAreaInputAcres);
            target.AKarabAreaInputGuntas = Convert.ToDecimal(string.IsNullOrEmpty(source.AKarabAreaInputGuntas) ? "0" : source.AKarabAreaInputGuntas);
            target.AKarabAreaInputAanas = Convert.ToDecimal(string.IsNullOrEmpty(source.AKarabAreaInputAanas) ? "0" : source.AKarabAreaInputAanas);
            target.AKarabAreaInAcres = Convert.ToDecimal(string.IsNullOrEmpty(source.AKarabAreaInAcres) ? "0" : source.AKarabAreaInAcres);
            target.AKarabAreaInGuntas = Convert.ToDecimal(string.IsNullOrEmpty(source.AKarabAreaInGuntas) ? "0" : source.AKarabAreaInGuntas);
            target.AKarabAreaInSqMts = Convert.ToDecimal(string.IsNullOrEmpty(source.AKarabAreaInSqft) ? "0" : source.AKarabAreaInSqft);
            target.AKarabAreaInSqft = Convert.ToDecimal(string.IsNullOrEmpty(source.LandAreaInputAcres) ? "0" : source.LandAreaInputAcres);
            target.BKarabAreaInputAcres = Convert.ToDecimal(string.IsNullOrEmpty(source.BKarabAreaInputAcres) ? "0" : source.BKarabAreaInputAcres);
            target.BKarabAreaInputGuntas = Convert.ToDecimal(string.IsNullOrEmpty(source.BKarabAreaInputGuntas) ? "0" : source.BKarabAreaInputGuntas);
            target.BKarabAreaInputAanas = Convert.ToDecimal(string.IsNullOrEmpty(source.BKarabAreaInputAanas) ? "0" : source.BKarabAreaInputAanas);
            target.BKarabAreaInAcres = Convert.ToDecimal(string.IsNullOrEmpty(source.BKarabAreaInAcres) ? "0" : source.BKarabAreaInAcres);
            target.BKarabAreaInGuntas = Convert.ToDecimal(string.IsNullOrEmpty(source.BKarabAreaInGuntas) ? "0" : source.BKarabAreaInGuntas);
            target.BKarabAreaInSqMts = Convert.ToDecimal(string.IsNullOrEmpty(source.BKarabAreaInSqMts) ? "0" : source.BKarabAreaInSqMts);
            target.BKarabAreaInSqft = Convert.ToDecimal(string.IsNullOrEmpty(source.BKarabAreaInSqft) ? "0" : source.BKarabAreaInSqft);
            target.CompanyID = source.CompanyID;
            target.PropertyDescription = source.PropertyDescription;
            target.CheckListMaster = source.CheckListMaster;

            if (source.PropertyCheckListDocuments != null && source.PropertyCheckListDocuments.Count > 0)
            {
                List<PropertyCheckListDocuments> docList = new List<PropertyCheckListDocuments>();
                foreach (var obj in source.PropertyCheckListDocuments)
                {
                    var doc = new PropertyCheckListDocuments();
                    UpdateDocumentFromModel(doc, obj);
                    docList.Add(doc);
                }
                target.PropertyCheckListDocuments = docList;
            }
            if (source.CheckListOfProperties != null && source.CheckListOfProperties.Count > 0)
            {
                List<CheckListOfProperty> checkList = new List<CheckListOfProperty>();
                foreach (var obj in source.CheckListOfProperties)
                {
                    var doc = new CheckListOfProperty();
                    UpdateCheckListFromModel(doc, obj);
                    checkList.Add(doc);
                }
                target.CheckListOfProperties = checkList;
            }

            if (source.PropertyCheckListVendors != null && source.PropertyCheckListVendors.Count > 0)
            {
                List<PropertyCheckListVendor> vendors = new List<PropertyCheckListVendor>();
                foreach (var obj in source.PropertyCheckListVendors)
                {
                    var doc = new PropertyCheckListVendor();
                    UpdateVendorFromModel(doc, obj);
                    vendors.Add(doc);
                }
                target.PropertyCheckListVendors = vendors;
            }
        }

        private void UpdateDocumentFromModel(PropertyCheckListDocuments target, PropertyCheckListDocumentsModel source)
        {
            target.PropertyCheckListBlobId = source.blobId;
            target.PropertyGuid = source.guid;
            target.FileBlob = source.ImageBytes;
            target.FileName = source.FileName;
            target.FileType = source.ContentType;
            target.FileCategoryId = source.FileCategoryId;
            target.UploadTime = DateTime.Now;
            target.DueDate = source.DueDate.UtcDateTime;
            target.ActualCompletionDate = source.ActualCompletionDate.UtcDateTime;
            target.Remarks = source.Remarks;
        }

        private void UpdateVendorFromModel(PropertyCheckListVendor target, PropertyCheckListVendorModel source)
        {
            target.VendorId = source.VendorId;
            target.VendorName = source.VendorName;
            target.PropertyCheckListId = source.PropertyCheckListId;
            target.CheckListVendorId = source.CheckListVendorId;
        }

        private void UpdateCheckListFromModel(CheckListOfProperty target, CheckListOfPropertyModel source)
        {
            target.CheckListPropertyId = source.CheckListPropertyId;
            target.CheckListId = source.CheckListId;
            target.PropertyCheckListId = source.PropertyCheckListId;
            target.Mandatory = source.Mandatory;
            
        }
    }
}
