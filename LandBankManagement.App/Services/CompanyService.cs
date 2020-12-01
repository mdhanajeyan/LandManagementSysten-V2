using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using IronSnappy;
using LandBankManagement.Data;
using LandBankManagement.Data.Services;
using LandBankManagement.Models;

namespace LandBankManagement.Services
{
    public class CompanyService : ICompanyService
    {
        public CompanyService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }


        public async Task<int> AddCompanyAsync(CompanyModel model, ICollection<ImagePickerResult> docs)
        {
            long id = model.CompanyID;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var company = new Company();
                if (company != null)
                {
                    if (docs.Count > 0)
                    {
                        List<CompanyDocuments> docList = new List<CompanyDocuments>();
                        foreach (var obj in docs)
                        {
                            var doc = new CompanyDocuments();
                            UpdateDocumentFromModel(doc, obj);
                            docList.Add(doc);
                        }
                        company.CompanyDocuments = docList;
                    }

                    UpdateCompanyFromModel(company, model);
                    company.CompanyGuid = Guid.NewGuid();
                    await dataService.UpdateCompanyAsync(company);
                    model.Merge(await GetCompanyAsync(dataService, company.CompanyID));
                }
                return 0;
            }
        }

        public async Task<CompanyModel> GetCompanyAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetCompanyAsync(dataService, id);
            }
        }
        static private async Task<CompanyModel> GetCompanyAsync(IDataService dataService, long id)
        {
            var item = await dataService.GetCompanyAsync(id);
            if (item != null)
            {
                return  CreateCompanyModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public async Task<IList<CompanyModel>> GetCompaniesAsync(DataRequest<Company> request)
        {
            var collection = new CompanyCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<CompanyModel>> GetCompaniesAsync(int skip, int take, DataRequest<Company> request)
        {
            var models = new List<CompanyModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetCompaniesAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add( CreateCompanyModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }

        public async Task<int> GetCompaniesCountAsync(DataRequest<Company> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetCompaniesCountAsync(request);
            }
        }

        public async Task<int> UpdateCompanyAsync(CompanyModel model, ICollection<ImagePickerResult> docs)
        {
            long id = model.CompanyID;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                //  var company = id > 0 ? await dataService.GetCompanyAsync(model.CompanyID) : new Company();
                var company = new Company();

                if (docs != null && docs.Count > 0)
                {
                    List<CompanyDocuments> docList = new List<CompanyDocuments>();
                    foreach (var obj in docs)
                    {
                        var doc = new CompanyDocuments();
                        UpdateDocumentFromModel(doc, obj);
                        docList.Add(doc);
                    }
                    company.CompanyDocuments = docList;
                }
                UpdateCompanyFromModel(company, model);
                await dataService.UpdateCompanyAsync(company);
                model.Merge(await GetCompanyAsync(dataService, company.CompanyID));
                return 0;
            }
        }

        public async Task<int> DeleteCompanyAsync(CompanyModel model)
        {
            var Company = new Company { CompanyID = model.CompanyID };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteCompanyAsync(Company);
            }
        }

        public async Task<int> DeleteCompanyRangeAsync(int index, int length, DataRequest<Company> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetCompanyKeysAsync(index, length, request);
                return await dataService.DeleteCompanyAsync(items.ToArray());
            }
        }
        public async Task<int> UploadCompanyDocumentsAsync(List<ImagePickerResult> models) {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                List<CompanyDocuments> docList = new List<CompanyDocuments>();
                foreach (var model in models)
                { var doc = new CompanyDocuments();
                    
                    UpdateDocumentFromModel(doc, model);
                    docList.Add(doc);
                }
                return await dataService.UploadCompanyDocumentsAsync(docList);
            }
        }

        public async Task<int> DeleteCompanyDocumentAsync(ImagePickerResult documents) {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var doc = new CompanyDocuments();
                UpdateDocumentFromModel(doc, documents);               
                return await dataService.DeleteCompanyDocumentAsync(doc);
            }
        }

        static public CompanyModel CreateCompanyModelAsync(Company source, bool includeAllFields)
        {
            var model = new CompanyModel()
            {
                CompanyID = source.CompanyID,
                CompanyGuid = source.CompanyGuid,
                Name = source.Name,
                PhoneNoIsdCode = source.PhoneNoIsdCode,
                PhoneNo = source.PhoneNo,
                Email = source.Email,
                PAN = source.PAN,
                GSTIN = source.GSTIN,
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                City = source.City,
                IsActive = source.IsActive,
                Pincode = source.Pincode
            };

            if (source.CompanyDocuments!=null && source.CompanyDocuments.Count > 0) {
                ObservableCollection<ImagePickerResult> docs = new ObservableCollection<ImagePickerResult>();
                foreach (var doc in source.CompanyDocuments) {
                    docs.Add(new ImagePickerResult
                    {blobId=doc.CompanyBlobId,
                    guid=doc.CompanyGuid,
                    FileName=doc.FileName,
                    ImageBytes= Snappy.Decode(doc.FileBlob),
                    ContentType=doc.FileType,
                    Size=doc.FileLength,
                    FileCategoryId=doc.FileCategoryId
                    });
                }
                model.CompanyDocuments = docs;
            }

            return model;
        }

        private void UpdateCompanyFromModel(Company target, CompanyModel source)
        {
            target.CompanyID = source.CompanyID;
            target.CompanyGuid = source.CompanyGuid;
            target.Name = source.Name;
            target.PhoneNoIsdCode = source.PhoneNoIsdCode;
            target.PhoneNo = source.PhoneNo;
            target.Email = source.Email;
            target.PAN = source.PAN;
            target.GSTIN = source.GSTIN;
            target.AddressLine1 = source.AddressLine1;
            target.AddressLine2 = source.AddressLine2;
            target.City = source.City;
            target.IsActive = source.IsActive;
            target.Pincode = source.Pincode;
        }

        private void UpdateDocumentFromModel(CompanyDocuments target, ImagePickerResult source) {
            target.CompanyBlobId = source.blobId;
            target.CompanyGuid = source.guid;
            target.FileBlob = Snappy.Encode(source.ImageBytes);
            target.FileName = source.FileName;
            target.FileType = source.ContentType;
            target.FileCategoryId = source.FileCategoryId;
            target.UploadTime = DateTime.Now;
        }

        public async Task<IList<CompanyModel>> GetCompaniesAsync()
        {
            var models = new List<CompanyModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetCompaniesAsync();
                foreach (var item in items)
                {
                    models.Add(CreateCompanyModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }
    }
}
