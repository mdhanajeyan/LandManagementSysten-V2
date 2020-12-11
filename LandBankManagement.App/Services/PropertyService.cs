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

        public PropertyService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public async Task<PropertyModel> AddPropertyAsync(PropertyModel model, ICollection<ImagePickerResult> docs)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var property = new Property();
                if (property != null)
                {
                    if (docs.Count > 0)
                    {
                        List<PropertyDocuments> docList = new List<PropertyDocuments>();
                        foreach (var obj in docs)
                        {
                            var doc = new PropertyDocuments();
                            UpdateDocumentFromModel(doc, obj);
                            docList.Add(doc);
                        }
                        property.PropertyDocuments = docList;
                    }
                    UpdatePropertyFromModel(property, model);
                    property.PropertyGuid = Guid.NewGuid();
                   var propertyID=  await dataService.AddPropertyAsync(property);
                    model.Merge(await GetPropertyAsync(dataService, propertyID));
                }
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
                    models.Add(await CreatePropertyModelAsync(item, includeAllFields: false));
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

        public async Task<PropertyModel> UpdatePropertyAsync(PropertyModel model, ICollection<ImagePickerResult> docs)
        {
            long id = model.PropertyId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var property = id > 0 ? await dataService.GetPropertyAsync(model.PropertyId) : new Property();
                if (property != null)
                {
                    if (docs != null && docs.Count > 0)
                    {
                        List<PropertyDocuments> docList = new List<PropertyDocuments>();
                        foreach (var obj in docs)
                        {
                            var doc = new PropertyDocuments();
                            UpdateDocumentFromModel(doc, obj);
                            docList.Add(doc);
                        }
                        property.PropertyDocuments = docList;
                    }
                    UpdatePropertyFromModel(property, model);
                    await dataService.UpdatePropertyAsync(property);
                    model.Merge(await GetPropertyAsync(dataService, property.PropertyId));
                }
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
        }


        private async Task<PropertyCostDetailsModel> CreateCostDetailsModel(PropertyCostDetails source) {
            var model = new PropertyCostDetailsModel {
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

        public async Task<int> AddPropPaySchedule(List<PaymentScheduleModel> schedules, decimal Sale1, decimal Sale2) {
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
                return await dataService.AddPropPaySchedule(paySchedule, Sale1, Sale2);
            }
        }

        private void UpdatePropPaySchedule(PropPaySchedule target, PaymentScheduleModel source) {
            target.PropertyId = source.PropertyId;
            target.ScheduleDate = source.ScheduleDate.UtcDateTime;
            target.Description = source.Description;
            target.Amount1 = source.Amount1;
            target.Amount2 = source.Amount2;
        }

        private PaymentScheduleModel CreatePaymentScheduleModel(PropPaySchedule source) {

            var model = new PaymentScheduleModel { 
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
                PartyName = source.PartyName
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
                PartyId = source.PartyId,
                TalukId = source.TalukId,
                HobliId = source.HobliId,
                VillageId = source.VillageId,
                DocumentTypeId = source.DocumentTypeId,
                DateOfExecution = source.DateOfExecution,
                DocumentNo = source.DocumentNo,
                PropertyTypeId = source.PropertyTypeId,
                SurveyNo = source.SurveyNo,
                PropertyGMapLink = source.PropertyGMapLink,
                LandAreaInputAcres = source.LandAreaInputAcres.ToString(),
                LandAreaInputGuntas = source.LandAreaInputGuntas.ToString(),
                LandAreaInAcres = source.LandAreaInAcres.ToString(),
                LandAreaInGuntas = source.LandAreaInGuntas.ToString(),
                LandAreaInSqMts = source.LandAreaInSqMts.ToString(),
                LandAreaInSqft = source.LandAreaInSqft.ToString(),
                AKarabAreaInputAcres = source.AKarabAreaInputAcres.ToString(),
                AKarabAreaInputGuntas = source.AKarabAreaInputGuntas.ToString(),
                AKarabAreaInAcres = source.AKarabAreaInAcres.ToString(),
                AKarabAreaInGuntas = source.AKarabAreaInGuntas.ToString(),
                AKarabAreaInSqMts = source.AKarabAreaInSqMts.ToString(),
                AKarabAreaInSqft = source.AKarabAreaInSqft.ToString(),
                BKarabAreaInputAcres = source.BKarabAreaInputAcres.ToString(),
                BKarabAreaInputGuntas = source.BKarabAreaInputGuntas.ToString(),
                BKarabAreaInAcres = source.BKarabAreaInAcres.ToString(),
                BKarabAreaInGuntas = source.BKarabAreaInGuntas.ToString(),
                BKarabAreaInSqMts = source.BKarabAreaInSqMts.ToString(),
                BKarabAreaInSqft = source.BKarabAreaInSqft.ToString(),
                SaleValue1 = source.SaleValue1,
                SaleValue2 = source.SaleValue2,
                CompanyID=source.CompanyID
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
                PartyId = source.PartyId,
                TalukId = source.TalukId,
                HobliId = source.HobliId,
                VillageId = source.VillageId,
                DocumentTypeId = source.DocumentTypeId,
                DateOfExecution = source.DateOfExecution,
                DocumentNo = source.DocumentNo,
                PropertyTypeId = source.PropertyTypeId,
                SurveyNo = source.SurveyNo,
                PropertyGMapLink = source.PropertyGMapLink,
                LandAreaInputAcres = source.LandAreaInputAcres.ToString(),
                LandAreaInputGuntas = source.LandAreaInputGuntas.ToString(),
                LandAreaInAcres = source.LandAreaInAcres.ToString(),
                LandAreaInGuntas = source.LandAreaInGuntas.ToString(),
                LandAreaInSqMts = source.LandAreaInSqMts.ToString(),
                LandAreaInSqft = source.LandAreaInSqft.ToString(),
                AKarabAreaInputAcres = source.AKarabAreaInputAcres.ToString(),
                AKarabAreaInputGuntas = source.AKarabAreaInputGuntas.ToString(),
                AKarabAreaInAcres = source.AKarabAreaInAcres.ToString(),
                AKarabAreaInGuntas = source.AKarabAreaInGuntas.ToString(),
                AKarabAreaInSqMts = source.AKarabAreaInSqMts.ToString(),
                AKarabAreaInSqft = source.AKarabAreaInSqft.ToString(),
                BKarabAreaInputAcres = source.BKarabAreaInputAcres.ToString(),
                BKarabAreaInputGuntas = source.BKarabAreaInputGuntas.ToString(),
                BKarabAreaInAcres = source.BKarabAreaInAcres.ToString(),
                BKarabAreaInGuntas = source.BKarabAreaInGuntas.ToString(),
                BKarabAreaInSqMts = source.BKarabAreaInSqMts.ToString(),
                BKarabAreaInSqft = source.BKarabAreaInSqft.ToString(),
                SaleValue1 = source.SaleValue1,
                SaleValue2 = source.SaleValue2,
                CompanyID = source.CompanyID
            };
            if (source.PropertyDocuments != null && source.PropertyDocuments.Count > 0)
            {
                ObservableCollection<ImagePickerResult> docs = new ObservableCollection<ImagePickerResult>();
                foreach (var doc in source.PropertyDocuments)
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
                model.PropertyDocuments = docs;
            }

            return model;
        }

        private void UpdatePropertyFromModel(Property target, PropertyModel source)
        {
            target.PropertyId = source.PropertyId;
            target.PropertyGuid = source.PropertyGuid;
            target.PropertyName = source.PropertyName;
            target.PartyId = source.PartyId;
            target.TalukId = source.TalukId;
            target.HobliId = source.HobliId;
            target.VillageId = source.VillageId;
            target.DocumentTypeId = source.DocumentTypeId;
            target.DateOfExecution = source.DateOfExecution.UtcDateTime;
            target.DocumentNo = source.DocumentNo;
            target.PropertyTypeId = source.PropertyTypeId;
            target.SurveyNo = source.SurveyNo;
            target.PropertyGMapLink = source.PropertyGMapLink;
            target.LandAreaInputAcres = Convert.ToDecimal(string.IsNullOrEmpty(source.LandAreaInputAcres) ? "0" : source.LandAreaInputAcres);
            target.LandAreaInputGuntas = Convert.ToDecimal(string.IsNullOrEmpty(source.LandAreaInputGuntas) ? "0" : source.LandAreaInputGuntas);
            target.LandAreaInAcres = Convert.ToDecimal(string.IsNullOrEmpty(source.LandAreaInAcres) ? "0" : source.LandAreaInAcres);
            target.LandAreaInGuntas = Convert.ToDecimal(string.IsNullOrEmpty(source.LandAreaInGuntas) ? "0" : source.LandAreaInGuntas);
            target.LandAreaInSqMts = Convert.ToDecimal(string.IsNullOrEmpty(source.LandAreaInSqMts) ? "0" : source.LandAreaInSqMts);
            target.LandAreaInSqft = Convert.ToDecimal(string.IsNullOrEmpty(source.LandAreaInSqft) ? "0" : source.LandAreaInSqft);
            target.AKarabAreaInputAcres = Convert.ToDecimal(string.IsNullOrEmpty(source.AKarabAreaInputAcres) ? "0" : source.AKarabAreaInputAcres);
            target.AKarabAreaInputGuntas = Convert.ToDecimal(string.IsNullOrEmpty(source.AKarabAreaInputGuntas) ? "0" : source.AKarabAreaInputGuntas);
            target.AKarabAreaInAcres = Convert.ToDecimal(string.IsNullOrEmpty(source.AKarabAreaInAcres) ? "0" : source.AKarabAreaInAcres);
            target.AKarabAreaInGuntas = Convert.ToDecimal(string.IsNullOrEmpty(source.AKarabAreaInGuntas) ? "0" : source.AKarabAreaInGuntas);
            target.AKarabAreaInSqMts = Convert.ToDecimal(string.IsNullOrEmpty(source.AKarabAreaInSqft) ? "0" : source.AKarabAreaInSqft);
            target.AKarabAreaInSqft = Convert.ToDecimal(string.IsNullOrEmpty(source.LandAreaInputAcres) ? "0" : source.LandAreaInputAcres);
            target.BKarabAreaInputAcres = Convert.ToDecimal(string.IsNullOrEmpty(source.BKarabAreaInputAcres) ? "0" : source.BKarabAreaInputAcres);
            target.BKarabAreaInputGuntas = Convert.ToDecimal(string.IsNullOrEmpty(source.BKarabAreaInputGuntas) ? "0" : source.BKarabAreaInputGuntas);
            target.BKarabAreaInAcres = Convert.ToDecimal(string.IsNullOrEmpty(source.BKarabAreaInAcres) ? "0" : source.BKarabAreaInAcres);
            target.BKarabAreaInGuntas = Convert.ToDecimal(string.IsNullOrEmpty(source.BKarabAreaInGuntas) ? "0" : source.BKarabAreaInGuntas);
            target.BKarabAreaInSqMts = Convert.ToDecimal(string.IsNullOrEmpty(source.BKarabAreaInSqMts) ? "0" : source.BKarabAreaInSqMts);
            target.BKarabAreaInSqft = Convert.ToDecimal(string.IsNullOrEmpty(source.BKarabAreaInSqft) ? "0" : source.BKarabAreaInSqft);
            target.SaleValue1 = source.SaleValue1;
            target.SaleValue2 = source.SaleValue2;
            target.CompanyID = source.CompanyID;
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
        }
    }
}
