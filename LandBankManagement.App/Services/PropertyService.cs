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
    public class PropertyService : IPropertyService
    {

        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }
        public PropertyContainer CurrentProperty { get; set; }
       
        public PropertyService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public void StoreItems(PropertyContainer data) {
            CurrentProperty = data;
        }

        public PropertyContainer GetStoredItems()
        {
            return CurrentProperty;
        }
        public void AddParty(PropertyPartyModel propertyPartyModel) {
            if (CurrentProperty.PartyList == null)
                CurrentProperty.PartyList = new ObservableCollection<PropertyPartyModel>();
            CurrentProperty.PartyList.Add(propertyPartyModel);
        }

        public async Task<PropertyModel> AddPropertyAsync(PropertyModel model,ICollection<PropertyDocumentTypeModel> propDocType, ICollection<ImagePickerResult> docs)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var property = new Property();
               
                    List<PropertyDocumentType> propertyDocumentTypeList = new List<PropertyDocumentType>();
                var propertyDoc = propDocType.First();
                    var propertyDocumentType = new PropertyDocumentType();
                    UpdatePropertyDocumentTypeFromModel(propertyDocumentType, propertyDoc);
                    if (propertyDoc.PropertyDocuments != null && propertyDoc.PropertyDocuments.Count > 0)
                    {
                        List<PropertyDocuments> docList = new List<PropertyDocuments>();
                        foreach (var obj in propertyDoc.PropertyDocuments)
                        {
                            var doc = new PropertyDocuments();
                            UpdateDocumentFromModel(doc, obj);
                            docList.Add(doc);
                        }
                    propertyDocumentType.PropertyDocuments = docList;                   
                    }
                propertyDocumentTypeList.Add(propertyDocumentType);

                property.PropertyDocumentType = propertyDocumentTypeList;

                    UpdatePropertyFromModel(property, model);
                    property.PropertyGuid = Guid.NewGuid();
                    property.GroupGuid = (model.GroupGuid==null || model.GroupGuid == Guid.Empty) ? Guid.NewGuid() : model.GroupGuid;// For Grouping property
                   var propertyID=  await dataService.AddPropertyAsync(property);
                    model.Merge(await GetPropertyAsync(dataService, propertyID));
                
                return model;
            }
        }


        static private async Task<PropertyModel> GetPropertyAsync(IDataService dataService, long id)
        {
            var item = await dataService.GetPropertyAsync(id);
            if (item != null)
            {
                //return await CreatePropertyModelAsync(item, includeAllFields: true);
                return await CreatePropertyModelWithDocsAsync(item);
                
            }
            return null;
        }
        public async Task<int> SaveDocuments(ICollection<ImagePickerResult> docs,Guid propertyGuid,int propertyDocumentTypeId) {
            if (docs != null && docs.Count > 0)
            {
                using (var dataService = DataServiceFactory.CreateDataService())
                {
                    List<PropertyDocuments> docList = new List<PropertyDocuments>();
                    foreach (var obj in docs)
                    {
                        var doc = new PropertyDocuments();
                        UpdateDocumentFromModel(doc, obj);
                        doc.PropertyGuid = propertyGuid;
                        doc.PropertyDocumentTypeId = propertyDocumentTypeId;
                        docList.Add(doc);
                    }
                    return await dataService.UploadPropertyDocumentsAsync(docList);
                }
            }
            return 0;
        }

        public async Task<ObservableCollection<ImagePickerResult>> GetProeprtyDocuments(int propertyDocumentTypeId) {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                ObservableCollection<ImagePickerResult> docs = new ObservableCollection<ImagePickerResult>();
                var items = await dataService.GetPropertyDocumentsAsync(propertyDocumentTypeId);
                foreach (var doc in items)
                {
                    docs.Add(new ImagePickerResult
                    {
                        blobId = doc.PropertyBlobId,
                        guid = doc.PropertyGuid,
                        FileName = doc.FileName,
                        ImageBytes = doc.FileBlob,
                        ContentType = doc.FileType,
                        Size = doc.FileLenght,
                        FileCategoryId = doc.FileCategoryId
                    });
                }
               
                return docs;
            }
        }

        public async Task<PropertyModel> GetPropertyAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetPropertyAsync(dataService, id);
            }
        }

        public async Task<IList<PropertyModel>> GetPropertiesAsync(DataRequest<Property> request)
        {
            var collection = new PropertyCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<PropertyModel>> GetPropertiesAsync(int skip, int take, DataRequest<Property> request)
        {
            var models = new List<PropertyModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetPropertiesAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(await CreatePropertyModelWithDocsAsync(item));
                }
                return models;
            }
        }

        public async Task<ObservableCollection<PropertyModel>> GetPropertyByGroupGuidAsync(Guid guid) {
            var models = new ObservableCollection<PropertyModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetPropertyByGroupGuidAsync(guid);
                foreach (var item in items)
                {
                    models.Add(await CreatePropertyModelWithDocsAsync(item));
                }
                return models;
            }
        }

        public async Task<int> GetPropertiesCountAsync(DataRequest<Property> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetPropertiesCountAsync(request);
            }
        }

        public async Task<PropertyModel> UpdatePropertyAsync(PropertyModel model, ICollection<PropertyDocumentTypeModel> propDocType, ICollection<ImagePickerResult> docs)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var property = new Property();
                List<PropertyDocumentType> propertyDocumentTypeList = new List<PropertyDocumentType>();
                foreach (var propertyDoc in propDocType) {
                    var propertyDocumentType = new PropertyDocumentType();
                    UpdatePropertyDocumentTypeFromModel(propertyDocumentType, propertyDoc);
                    if (propertyDoc.PropertyDocuments != null && propertyDoc.PropertyDocuments.Count > 0)
                    {
                        List<PropertyDocuments> docList = new List<PropertyDocuments>();
                        foreach (var obj in propertyDoc.PropertyDocuments)
                        {
                            var doc = new PropertyDocuments();
                            UpdateDocumentFromModel(doc, obj);
                            docList.Add(doc);
                        }
                        propertyDocumentType.PropertyDocuments = docList;                       
                    }

                    propertyDocumentTypeList.Add(propertyDocumentType);
                }
                property.PropertyDocumentType = propertyDocumentTypeList;
                UpdatePropertyFromModel(property, model);
                await dataService.UpdatePropertyAsync(property);
                model.Merge(await GetPropertyAsync(dataService, property.PropertyId));

                return model;
            }
        }

        public async Task<int> DeletePropertyAsync(PropertyModel model)
        {
            var property = new Property { PropertyId = model.PropertyId };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeletePropertyAsync(property);
            }
        }

        public async Task<int> AddPropertyPartyAsync(List<PropertyPartyModel> propertyParties)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                List<PropertyParty> list = new List<PropertyParty>();
                foreach(var model in propertyParties)
                {
                    PropertyParty party = new PropertyParty();
                    UpdatePropertyPartyFromModel(party, model);
                    list.Add(party);
                }
                return await dataService.AddPropertyParty(list);
            }
        }
        public async Task<ObservableCollection<PropertyPartyModel>> GetPartiesOfProperty(int propertyId) {
            var models = new ObservableCollection<PropertyPartyModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetPartiesOfProperty(propertyId);
                foreach (var item in items)
                {
                    models.Add(await CreatePropertyPartyModelAsync(item));
                }
                return models;
            }
        }

        public async Task<int> DeletePropertyPartyAsync(PropertyPartyModel model)
        {
            var property = new PropertyParty { PropertyPartyId = model.PropertyPartyId };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeletePropertyPartyAsync(property);
            }
        }


        public async Task<int> DeletePropertyDocumentAsync(ImagePickerResult documents)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var doc = new PropertyDocuments();
                UpdateDocumentFromModel(doc, documents);
                return await dataService.DeletePropertyDocumentAsync(doc);
            }
        }

        public async Task<PropertyCostDetailsModel> GetPropertyCostDetails(int propertyId) {
            using (var dataService = DataServiceFactory.CreateDataService()) {
                var items = await dataService.GetPropertyCostDetails(propertyId);
                return await CreateCostDetailsModel(items);
            }
        }


        private void UpdatePropertyPartyFromModel(PropertyParty target, PropertyPartyModel source) {
            target.PropertyPartyId = source.PropertyPartyId;
            target.PropertyGuid = source.PropertyGuid;
            target.PartyId = source.PartyId;
            target.PropertyId = source.PropertyId;
            target.IsGroup = source.IsGroup;
            target.IsPrimaryParty = source.IsPrimaryParty;
        }


        private async Task<PropertyCostDetailsModel> CreateCostDetailsModel(PropertyCostDetails source) {
            var model = new PropertyCostDetailsModel {
                PropertyDocumentTypeId=source.PropertyDocumentTypeId,
                PropertyId = source.PropertyId,
                ComapnyName = source.ComapnyName,
                PropertyName = source.PropertyName,
                PropertyType = source.PropertyType,
                DocumentType = source.DocumentType,
                Taluk = source.Taluk,
                Hobli = source.Hobli,
                Village = source.Village,
                SurveyNo = source.SurveyNo,
                SaleValue1 = source.SaleValue1.ToString(),
                SaleValue2 = source.SaleValue2.ToString(),
                Total = (source.SaleValue1 + source.SaleValue2).ToString()
            };

            var models = new ObservableCollection<PropertyPartyModel>();
            foreach (var item in source.PropertyParties)
            {
                models.Add(await CreatePropertyPartyModelAsync(item));
            }
            model.Parties = models.ToList();

            var schedules = new List<PaymentScheduleModel>();
            foreach (var item in source.PropPaySchedules)
            {
                schedules.Add( CreatePaymentScheduleModel(item));
            }
            model.PropPaySchedules = schedules;
            return model;

        }

        public async Task<int> AddPropPaySchedule(int propDocTypeId,List<PaymentScheduleModel> schedules, decimal Sale1, decimal Sale2) {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var paySchedule = new List<PropPaySchedule>();
                foreach (var model in schedules) {
                    if (model.ScheduleId != 0)
                        continue;
                    var pay = new PropPaySchedule();
                    UpdatePropPaySchedule(pay, model);
                    paySchedule.Add(pay);
                }

                return await dataService.AddPropPaySchedule(propDocTypeId, paySchedule, Sale1, Sale2);
            }
        }

        private void UpdatePropPaySchedule(PropPaySchedule target, PaymentScheduleModel source) {
            target.PropertyId = source.PropertyId;
            target.PropertyDocumentTypeId = source.PropertyDocumentTypeId;
            target.ScheduleDate = source.ScheduleDate.UtcDateTime;
            target.Description = source.Description;
            target.Amount1 = source.Amount1;
            target.Amount2 = source.Amount2;
        }

        private PaymentScheduleModel CreatePaymentScheduleModel(PropPaySchedule source) {

            var model = new PaymentScheduleModel { 
                PropertyDocumentTypeId=source.PropertyDocumentTypeId,
            ScheduleId=source.PropPayScheduleId,
            ScheduleDate=source.ScheduleDate,
            Description=source.Description,
            Amount1=source.Amount1,
            Amount2=source.Amount2
            };

            return model;
        }


        static public async Task<PropertyPartyModel> CreatePropertyPartyModelAsync(PropertyParty source) {
            var model = new PropertyPartyModel()
            {
                PropertyPartyId = source.PropertyPartyId,
                PropertyGuid = source.PropertyGuid,
                PartyId = source.PartyId,
                PropertyId = source.PropertyId,
                PartyName = source.PartyName,
                IsGroup=source.IsGroup,
                IsPrimaryParty=source.IsPrimaryParty
            };
            return model;
        }

        static public async Task<PropertyModel> CreatePropertyModelAsync(Property source, bool includeAllFields)
        {
            var model = new PropertyModel()
            {
                PropertyId = source.PropertyId,
                PropertyGuid = source.PropertyGuid,
                PropertyName = source.PropertyName,
                GroupGuid=source.GroupGuid,
                PartyId = source.PartyId,
                TalukId = source.TalukId.ToString(),
                HobliId = source.HobliId.ToString(),
                VillageId = source.VillageId.ToString(),
                DocumentTypeId = source.DocumentTypeId.ToString(),
                DateOfExecution = source.DateOfExecution,
                DocumentNo = source.DocumentNo,
                PropertyTypeId = source.PropertyTypeId.ToString(),
                SurveyNo = source.SurveyNo,
                PropertyGMapLink = source.PropertyGMapLink,
                //LandAreaInputAcres = source.LandAreaInputAcres.ToString(),
                //LandAreaInputGuntas = source.LandAreaInputGuntas.ToString(),
                //LandAreaInputAanas= source.LandAreaInputAanas.ToString(),
                //LandAreaInAcres = source.LandAreaInAcres.ToString(),
                //LandAreaInGuntas = source.LandAreaInGuntas.ToString(),
                //LandAreaInSqMts = source.LandAreaInSqMts.ToString(),
                //LandAreaInSqft = source.LandAreaInSqft.ToString(),
                //AKarabAreaInputAcres = source.AKarabAreaInputAcres.ToString(),
                //AKarabAreaInputGuntas = source.AKarabAreaInputGuntas.ToString(),
                //AKarabAreaInputAanas = source.AKarabAreaInputAanas.ToString(),
                //AKarabAreaInAcres = source.AKarabAreaInAcres.ToString(),
                //AKarabAreaInGuntas = source.AKarabAreaInGuntas.ToString(),
                //AKarabAreaInSqMts = source.AKarabAreaInSqMts.ToString(),
                //AKarabAreaInSqft = source.AKarabAreaInSqft.ToString(),
                //BKarabAreaInputAcres = source.BKarabAreaInputAcres.ToString(),
                //BKarabAreaInputGuntas = source.BKarabAreaInputGuntas.ToString(),
                //BKarabAreaInputAanas = source.BKarabAreaInputAanas.ToString(),
                //BKarabAreaInAcres = source.BKarabAreaInAcres.ToString(),
                //BKarabAreaInGuntas = source.BKarabAreaInGuntas.ToString(),
                //BKarabAreaInSqMts = source.BKarabAreaInSqMts.ToString(),
                //BKarabAreaInSqft = source.BKarabAreaInSqft.ToString(),
                //SaleValue1 = source.SaleValue1,
                //SaleValue2 = source.SaleValue2,
                CompanyID=source.CompanyID.ToString(),
                IsSold=source.IsSold
            };

            return model;
        }

       static public async Task<PropertyModel> CreatePropertyModelWithDocsAsync(Property source)
        {
            var model = new PropertyModel()
            {
                PropertyId = source.PropertyId,
                PropertyGuid = source.PropertyGuid,
                PropertyName = source.PropertyName,
                GroupGuid=source.GroupGuid,
                PartyId = source.PartyId,
                TalukId = source.TalukId.ToString(),
                HobliId = source.HobliId.ToString(),
                VillageId = source.VillageId.ToString(),
                DocumentTypeId = source.DocumentTypeId.ToString(),
                DateOfExecution = source.DateOfExecution,
                DocumentNo = source.DocumentNo,
                PropertyTypeId = source.PropertyTypeId.ToString(),
                SurveyNo = source.SurveyNo,
                PropertyGMapLink = source.PropertyGMapLink,
                //LandAreaInputAcres = source.LandAreaInputAcres.ToString(),
                //LandAreaInputGuntas = source.LandAreaInputGuntas.ToString(),
                //LandAreaInputAanas = source.LandAreaInputAanas.ToString(),
                //LandAreaInAcres = source.LandAreaInAcres.ToString(),
                //LandAreaInGuntas = source.LandAreaInGuntas.ToString(),
                //LandAreaInSqMts = source.LandAreaInSqMts.ToString(),
                //LandAreaInSqft = source.LandAreaInSqft.ToString(),
                //AKarabAreaInputAcres = source.AKarabAreaInputAcres.ToString(),
                //AKarabAreaInputGuntas = source.AKarabAreaInputGuntas.ToString(),
                //AKarabAreaInputAanas = source.AKarabAreaInputAanas.ToString(),
                //AKarabAreaInAcres = source.AKarabAreaInAcres.ToString(),
                //AKarabAreaInGuntas = source.AKarabAreaInGuntas.ToString(),
                //AKarabAreaInSqMts = source.AKarabAreaInSqMts.ToString(),
                //AKarabAreaInSqft = source.AKarabAreaInSqft.ToString(),
                //BKarabAreaInputAcres = source.BKarabAreaInputAcres.ToString(),
                //BKarabAreaInputGuntas = source.BKarabAreaInputGuntas.ToString(),
                //BKarabAreaInputAanas = source.BKarabAreaInputAanas.ToString(),
                //BKarabAreaInAcres = source.BKarabAreaInAcres.ToString(),
                //BKarabAreaInGuntas = source.BKarabAreaInGuntas.ToString(),
                //BKarabAreaInSqMts = source.BKarabAreaInSqMts.ToString(),
                //BKarabAreaInSqft = source.BKarabAreaInSqft.ToString(),
                //SaleValue1 = source.SaleValue1,
                //SaleValue2 = source.SaleValue2,
                CompanyID = source.CompanyID.ToString(),
                IsSold = source.IsSold
            };


            if (source.PropertyDocumentType != null && source.PropertyDocumentType.Count > 0)
            {
                ObservableCollection<PropertyDocumentTypeModel> propDocTypeList = new ObservableCollection<PropertyDocumentTypeModel>();
                foreach (var propDoc in source.PropertyDocumentType) {
                    var propertyDocumentTypeModel = new PropertyDocumentTypeModel();
                    UpDatePropertyDocumentTypeModel(propertyDocumentTypeModel, propDoc);

                    if (propDoc.PropertyDocuments != null)
                    {
                        ObservableCollection<ImagePickerResult> docs = new ObservableCollection<ImagePickerResult>();
                        foreach (var doc in propDoc.PropertyDocuments)
                        {
                            docs.Add(new ImagePickerResult
                            {
                                blobId = doc.PropertyBlobId,
                                guid = doc.PropertyGuid,
                                FileName = doc.FileName,
                                ImageBytes = doc.FileBlob,
                                ContentType = doc.FileType,
                                Size = doc.FileLenght,
                                FileCategoryId = doc.FileCategoryId,
                                DocumentType = doc.PropertyDocumentTypeId
                            });
                        }
                        propertyDocumentTypeModel.PropertyDocuments = docs;
                    }
                    propDocTypeList.Add(propertyDocumentTypeModel);
                }
                model.PropertyDocumentType = propDocTypeList;
            }

            return model;
        }

       static private void UpDatePropertyDocumentTypeModel(PropertyDocumentTypeModel target, PropertyDocumentType source) {
            target.PropertyDocumentTypeId = source.PropertyDocumentTypeId;
            target.PropertyId = source.PropertyId;
            target.DocumentType = source.DocumentTypeName;
            target.DocumentTypeId = source.DocumentTypeId;
            target.LandAreaInputAcres = source.LandAreaInputAcres.ToString();
            target.LandAreaInputGuntas =source.LandAreaInputGuntas.ToString();
            target.LandAreaInputAanas =  source.LandAreaInputAanas.ToString();
            target.LandAreaInAcres =source.LandAreaInAcres.ToString();
            target.LandAreaInGuntas = source.LandAreaInGuntas.ToString();
            target.LandAreaInSqMts =  source.LandAreaInSqMts.ToString();
            target.LandAreaInSqft =  source.LandAreaInSqft.ToString();
            target.AKarabAreaInputAcres = source.AKarabAreaInputAcres.ToString();
            target.AKarabAreaInputGuntas =  source.AKarabAreaInputGuntas.ToString();
            target.AKarabAreaInputAanas = source.AKarabAreaInputAanas.ToString();
            target.AKarabAreaInAcres =  source.AKarabAreaInAcres.ToString();
            target.AKarabAreaInGuntas =  source.AKarabAreaInGuntas.ToString();
            target.AKarabAreaInSqMts =  source.AKarabAreaInSqMts.ToString();
            target.AKarabAreaInSqft = source.AKarabAreaInSqft.ToString();
            target.BKarabAreaInputAcres =source.BKarabAreaInputAcres.ToString();
            target.BKarabAreaInputGuntas = source.BKarabAreaInputGuntas.ToString();
            target.BKarabAreaInputAanas = source.BKarabAreaInputAanas.ToString();
            target.BKarabAreaInAcres = source.BKarabAreaInAcres.ToString();
            target.BKarabAreaInGuntas =source.BKarabAreaInGuntas.ToString();
            target.BKarabAreaInSqMts = source.BKarabAreaInSqMts.ToString();
            target.BKarabAreaInSqft = source.BKarabAreaInSqft.ToString();
            target.SaleValue1 = source.SaleValue1;
            target.SaleValue2 = source.SaleValue2;
            if (source.LandArea != null && source.LandArea != "")
            {
                var area = source.LandArea.Split('-');
                var calculatedArea = LandBankManagement.AreaConvertor.ConvertArea(Convert.ToDecimal(area[0]), Convert.ToDecimal(area[1]), Convert.ToDecimal(area[2]));
                target.LandArea = calculatedArea.Acres + " - " + calculatedArea.Guntas + " - " + calculatedArea.Anas;
            }
           
        }

        private void UpdatePropertyFromModel(Property target, PropertyModel source)
        {
            target.PropertyId = source.PropertyId;
            target.PropertyGuid = source.PropertyGuid;
            target.PropertyName = source.PropertyName;
            target.GroupGuid = source.GroupGuid;
            target.PartyId = source.PartyId;
            target.TalukId = Convert.ToInt32( source.TalukId);
            target.HobliId = Convert.ToInt32(source.HobliId);
            target.VillageId = Convert.ToInt32(source.VillageId);
            target.DocumentTypeId = Convert.ToInt32(source.DocumentTypeId);
            target.DateOfExecution = source.DateOfExecution.UtcDateTime;
            target.DocumentNo = source.DocumentNo;
            target.PropertyTypeId = Convert.ToInt32(source.PropertyTypeId);
            target.SurveyNo = source.SurveyNo;
            target.PropertyGMapLink = source.PropertyGMapLink;
            //target.LandAreaInputAcres = Convert.ToDecimal(string.IsNullOrEmpty(source.LandAreaInputAcres) ? "0" : source.LandAreaInputAcres);
            //target.LandAreaInputGuntas = Convert.ToDecimal(string.IsNullOrEmpty(source.LandAreaInputGuntas) ? "0" : source.LandAreaInputGuntas);
            //target.LandAreaInputAanas = Convert.ToDecimal(string.IsNullOrEmpty(source.LandAreaInputAanas) ? "0" : source.LandAreaInputAanas);
            //target.LandAreaInAcres = Convert.ToDecimal(string.IsNullOrEmpty(source.LandAreaInAcres) ? "0" : source.LandAreaInAcres);
            //target.LandAreaInGuntas = Convert.ToDecimal(string.IsNullOrEmpty(source.LandAreaInGuntas) ? "0" : source.LandAreaInGuntas);
            //target.LandAreaInSqMts = Convert.ToDecimal(string.IsNullOrEmpty(source.LandAreaInSqMts) ? "0" : source.LandAreaInSqMts);
            //target.LandAreaInSqft = Convert.ToDecimal(string.IsNullOrEmpty(source.LandAreaInSqft) ? "0" : source.LandAreaInSqft);
            //target.AKarabAreaInputAcres = Convert.ToDecimal(string.IsNullOrEmpty(source.AKarabAreaInputAcres) ? "0" : source.AKarabAreaInputAcres);
            //target.AKarabAreaInputGuntas = Convert.ToDecimal(string.IsNullOrEmpty(source.AKarabAreaInputGuntas) ? "0" : source.AKarabAreaInputGuntas);
            //target.AKarabAreaInputAanas = Convert.ToDecimal(string.IsNullOrEmpty(source.AKarabAreaInputAanas) ? "0" : source.AKarabAreaInputAanas);
            //target.AKarabAreaInAcres = Convert.ToDecimal(string.IsNullOrEmpty(source.AKarabAreaInAcres) ? "0" : source.AKarabAreaInAcres);
            //target.AKarabAreaInGuntas = Convert.ToDecimal(string.IsNullOrEmpty(source.AKarabAreaInGuntas) ? "0" : source.AKarabAreaInGuntas);
            //target.AKarabAreaInSqMts = Convert.ToDecimal(string.IsNullOrEmpty(source.AKarabAreaInSqft) ? "0" : source.AKarabAreaInSqft);
            //target.AKarabAreaInSqft = Convert.ToDecimal(string.IsNullOrEmpty(source.LandAreaInputAcres) ? "0" : source.LandAreaInputAcres);
            //target.BKarabAreaInputAcres = Convert.ToDecimal(string.IsNullOrEmpty(source.BKarabAreaInputAcres) ? "0" : source.BKarabAreaInputAcres);
            //target.BKarabAreaInputGuntas = Convert.ToDecimal(string.IsNullOrEmpty(source.BKarabAreaInputGuntas) ? "0" : source.BKarabAreaInputGuntas);
            //target.BKarabAreaInputAanas = Convert.ToDecimal(string.IsNullOrEmpty(source.BKarabAreaInputAanas) ? "0" : source.BKarabAreaInputAanas);
            //target.BKarabAreaInAcres = Convert.ToDecimal(string.IsNullOrEmpty(source.BKarabAreaInAcres) ? "0" : source.BKarabAreaInAcres);
            //target.BKarabAreaInGuntas = Convert.ToDecimal(string.IsNullOrEmpty(source.BKarabAreaInGuntas) ? "0" : source.BKarabAreaInGuntas);
            //target.BKarabAreaInSqMts = Convert.ToDecimal(string.IsNullOrEmpty(source.BKarabAreaInSqMts) ? "0" : source.BKarabAreaInSqMts);
            //target.BKarabAreaInSqft = Convert.ToDecimal(string.IsNullOrEmpty(source.BKarabAreaInSqft) ? "0" : source.BKarabAreaInSqft);
            //target.SaleValue1 = source.SaleValue1;
            //target.SaleValue2 = source.SaleValue2;
            target.CompanyID = Convert.ToInt32(source.CompanyID);
        }

        private void UpdatePropertyDocumentTypeFromModel(PropertyDocumentType target, PropertyDocumentTypeModel source)
        {
            target.PropertyDocumentTypeId = source.PropertyDocumentTypeId;
            target.PropertyId = source.PropertyId;
            target.DocumentTypeId = source.DocumentTypeId;
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
            target.AKarabAreaInSqMts = Convert.ToDecimal(string.IsNullOrEmpty(source.AKarabAreaInSqMts) ? "0" : source.AKarabAreaInSqMts);
            target.AKarabAreaInSqft = Convert.ToDecimal(string.IsNullOrEmpty(source.AKarabAreaInSqft) ? "0" : source.AKarabAreaInSqft);
            target.BKarabAreaInputAcres = Convert.ToDecimal(string.IsNullOrEmpty(source.BKarabAreaInputAcres) ? "0" : source.BKarabAreaInputAcres);
            target.BKarabAreaInputGuntas = Convert.ToDecimal(string.IsNullOrEmpty(source.BKarabAreaInputGuntas) ? "0" : source.BKarabAreaInputGuntas);
            target.BKarabAreaInputAanas = Convert.ToDecimal(string.IsNullOrEmpty(source.BKarabAreaInputAanas) ? "0" : source.BKarabAreaInputAanas);
            target.BKarabAreaInAcres = Convert.ToDecimal(string.IsNullOrEmpty(source.BKarabAreaInAcres) ? "0" : source.BKarabAreaInAcres);
            target.BKarabAreaInGuntas = Convert.ToDecimal(string.IsNullOrEmpty(source.BKarabAreaInGuntas) ? "0" : source.BKarabAreaInGuntas);
            target.BKarabAreaInSqMts = Convert.ToDecimal(string.IsNullOrEmpty(source.BKarabAreaInSqMts) ? "0" : source.BKarabAreaInSqMts);
            target.BKarabAreaInSqft = Convert.ToDecimal(string.IsNullOrEmpty(source.BKarabAreaInSqft) ? "0" : source.BKarabAreaInSqft);
            target.SaleValue1 = source.SaleValue1;
            target.SaleValue2 = source.SaleValue2;
        }

        private void UpdateDocumentFromModel(PropertyDocuments target, ImagePickerResult source)
        {
            target.PropertyBlobId = source.blobId;
            target.PropertyGuid = source.guid;
            target.FileBlob = source.ImageBytes;
            target.FileName = source.FileName;
            target.FileType = source.ContentType;
            target.FileCategoryId = source.FileCategoryId;
            target.UploadTime = DateTime.Now;
            target.PropertyDocumentTypeId = source.DocumentType;
        }
    }
}
