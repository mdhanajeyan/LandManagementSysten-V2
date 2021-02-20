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
        public PropertyCheckListContainer CurrentPropertyCheckList { get; set; }
        public PropertyCheckListService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }
        public void StoreItems(PropertyCheckListContainer data)
        {
            CurrentPropertyCheckList = data;
        }

        public PropertyCheckListContainer GetStoredItems()
        {
            return CurrentPropertyCheckList;
        }

        public void AddVendor(PropertyCheckListVendorModel propertyVendorModel)
        {
            if (CurrentPropertyCheckList.VendorList == null)
                CurrentPropertyCheckList.VendorList = new ObservableCollection<PropertyCheckListVendorModel>();
            CurrentPropertyCheckList.VendorList.Add(propertyVendorModel);
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
        public async Task<int> DeleteCheckListOfPropertyAsync(int checkListPropertyID)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteCheckListOfPropertyAsync(checkListPropertyID);
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
                            CheckListVendorId = obj.CheckListVendorId,
                            IsPrimaryVendor=obj.IsPrimaryVendor
                        });

                    }
                    return vendors;
                }
                return null;
            }
        }

        public async Task<int> SaveDocuments(ICollection<PropertyCheckListDocumentsModel> docs, Guid propertyGuid)
        {
            if (docs != null && docs.Count > 0)
            {
                using (var dataService = DataServiceFactory.CreateDataService())
                {
                    List<PropertyCheckListDocuments> docList = new List<PropertyCheckListDocuments>();
                    foreach (var obj in docs)
                    {
                        var doc = new PropertyCheckListDocuments();
                        UpdateDocumentFromModel(doc, obj);
                        doc.PropertyGuid = propertyGuid;
                        docList.Add(doc);
                    }
                    return await dataService.UploadPropertyCheckListDocumentsAsync(docList);
                }
            }
            return 0;
        }

        public async Task<ObservableCollection<PropertyCheckListDocumentsModel>> GetDocuments(Guid propertyGuid)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                ObservableCollection<PropertyCheckListDocumentsModel> docList = new ObservableCollection<PropertyCheckListDocumentsModel>();
                var items = await dataService.GetPropertyCheckListDocumentsAsync(propertyGuid);
                foreach (var obj in items)
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

                return docList;
            }
        }

        public async Task<ObservableCollection<CheckListOfPropertyModel>> GetCheckListOfProperty(int id) {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                ObservableCollection<CheckListOfPropertyModel> checkList = new ObservableCollection<CheckListOfPropertyModel>();
                var items = await dataService.GetCheckListOfProperty(id);
                foreach (var obj in items)
                {
                    checkList.Add(new CheckListOfPropertyModel
                    {
                        CheckListPropertyId=obj.CheckListPropertyId,
                        PropertyCheckListId=obj.PropertyCheckListId,
                        CheckListId=obj.CheckListId,
                        Mandatory=obj.Mandatory,
                        Name=obj.Name
                    });
                }

                return checkList;
            }
        }


        static public async Task<PropertyCheckListModel> CreatePropertyCheckListModelAsync(PropertyCheckList source, bool includeAllFields)
        {
            var model = new PropertyCheckListModel()
            {
                PropertyCheckListId = source.PropertyCheckListId,
                PropertyGuid = source.PropertyGuid,
                PropertyName = source.PropertyName,
                TalukId = source.TalukId.ToString(),
                HobliId = source.HobliId.ToString(),
                VillageId = source.VillageId.ToString(),
                DocumentTypeId = source.DocumentTypeId.ToString(),
                PropertyTypeId = source.PropertyTypeId.ToString(),
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
                CompanyID = source.CompanyID.ToString(),
                CompanyName = source.CompanyName,
                VillageName = source.VillageName,
                PropertyDescription =source.PropertyDescription,
                CheckListMaster=source.CheckListMaster,
                Remarks=source.Remarks,
                Status=source.Status,
                TotalArea=source.TotalArea
            };
            var area = model.TotalArea.Split('-');
            var calculatedArea = LandBankManagement.AreaConvertor.ConvertArea(Convert.ToDecimal(area[0]), Convert.ToDecimal(area[1]), Convert.ToDecimal(area[2]));
            model.TotalArea = calculatedArea.Acres + " - " + calculatedArea.Guntas + " - " + calculatedArea.Anas;


            return model;
        }

        static public async Task<PropertyCheckListModel> CreatePropertyCheckListModelWithDocsAsync(PropertyCheckList source)
        {
            var model = new PropertyCheckListModel()
            {
                PropertyCheckListId = source.PropertyCheckListId,
                PropertyGuid = source.PropertyGuid,
                PropertyName = source.PropertyName,
                TalukId = source.TalukId.ToString(),
                HobliId = source.HobliId.ToString(),
                VillageId = source.VillageId.ToString(),
                DocumentTypeId = source.DocumentTypeId.ToString(),
                PropertyTypeId = source.PropertyTypeId.ToString(),
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
                CompanyID = source.CompanyID.ToString(),
                CompanyName=source.CompanyName,
                VillageName=source.VillageName,
                PropertyDescription = source.PropertyDescription,
                CheckListMaster = source.CheckListMaster,
                TotalArea=source.TotalArea
            };
            //if (source.PropertyCheckListDocuments != null && source.PropertyCheckListDocuments.Count > 0)
            //{
            //    var docList = new ObservableCollection<PropertyCheckListDocumentsModel>();
            //    foreach (var obj in source.PropertyCheckListDocuments)
            //    {
            //        docList.Add(new PropertyCheckListDocumentsModel
            //        {
            //            PropertyCheckListBlobId = obj.PropertyCheckListBlobId,
            //            guid = obj.PropertyGuid,
            //            ImageBytes = obj.FileBlob,
            //            FileName = obj.FileName,
            //            ContentType = obj.FileType,
            //            FileCategoryId = obj.FileCategoryId,
            //            UploadTime = obj.UploadTime,
            //            DueDate = obj.DueDate,
            //            ActualCompletionDate = obj.ActualCompletionDate,
            //            Remarks = obj.Remarks
            //        });
            //    }
            //    model.PropertyCheckListDocuments =docList;
            //}

            if (source.CheckListOfProperties != null && source.CheckListOfProperties.Count > 0)
            {
                var checkList = new ObservableCollection<CheckListOfPropertyModel>();
                foreach (var obj in source.CheckListOfProperties)
                {
                    var checkItem = new CheckListOfPropertyModel
                    {
                        CheckListPropertyId = obj.CheckListPropertyId,
                        PropertyCheckListId = obj.PropertyCheckListId,
                        CheckListId = obj.CheckListId,
                        Mandatory = obj.Mandatory,
                        Name=obj.Name
                    };
                    var docList = new ObservableCollection<PropertyCheckListDocumentsModel>();
                    foreach (var doc in obj.Documents)
                    {
                        docList.Add(new PropertyCheckListDocumentsModel
                        {
                            PropertyCheckListBlobId = doc.PropertyCheckListBlobId,
                            CheckListPropertyId = doc.CheckListPropertyId,
                            guid = doc.PropertyGuid,
                            ImageBytes = doc.FileBlob,
                            FileName = doc.FileName,
                            ContentType = doc.FileType,
                            FileCategoryId = doc.FileCategoryId,
                            UploadTime = doc.UploadTime,
                            DueDate = doc.DueDate,
                            ActualCompletionDate = doc.ActualCompletionDate,
                            Remarks = doc.Remarks
                        });
                    }
                    checkItem.Documents = docList;

                    checkList.Add(checkItem);
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
                        CheckListVendorId = obj.CheckListVendorId,
                        IsGroup=obj.IsGroup,
                        IsPrimaryVendor=obj.IsPrimaryVendor
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
            target.TalukId =Convert.ToInt32( source.TalukId);
            target.HobliId = Convert.ToInt32(source.HobliId);
            target.VillageId = Convert.ToInt32(source.VillageId);
            target.DocumentTypeId = Convert.ToInt32(source.DocumentTypeId);
            target.PropertyTypeId = Convert.ToInt32(source.PropertyTypeId);
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
            target.CompanyID = Convert.ToInt32(source.CompanyID);
            target.PropertyDescription = source.PropertyDescription;
            target.CheckListMaster = source.CheckListMaster;

            //if (source.PropertyCheckListDocuments != null && source.PropertyCheckListDocuments.Count > 0)
            //{
            //    List<PropertyCheckListDocuments> docList = new List<PropertyCheckListDocuments>();
            //    foreach (var obj in source.PropertyCheckListDocuments)
            //    {
            //        var doc = new PropertyCheckListDocuments();
            //        UpdateDocumentFromModel(doc, obj);
            //        docList.Add(doc);
            //    }
            //    target.PropertyCheckListDocuments = docList;
            //}
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
            target.PropertyCheckListBlobId = source.PropertyCheckListBlobId;
            target.CheckListPropertyId = source.CheckListPropertyId;
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
            target.IsPrimaryVendor = source.IsPrimaryVendor;
            target.IsGroup = source.IsGroup;
        }

        private void UpdateCheckListFromModel(CheckListOfProperty target, CheckListOfPropertyModel source)
        {
            target.CheckListPropertyId = source.CheckListPropertyId;
            target.CheckListId = source.CheckListId;
            target.PropertyCheckListId = source.PropertyCheckListId;
            target.Mandatory = source.Mandatory;
            target.Delete = source.Delete;

            if (source.Documents == null)
                return;

            List<PropertyCheckListDocuments> docList = new List<PropertyCheckListDocuments>();
            foreach (var obj in source.Documents)
            {
                var doc = new PropertyCheckListDocuments();
                UpdateDocumentFromModel(doc, obj);
                docList.Add(doc);
            }
            target.Documents =docList;
        }
    }
}
