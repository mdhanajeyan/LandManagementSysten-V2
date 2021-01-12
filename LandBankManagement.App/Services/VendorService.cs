using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using LandBankManagement.Data;
using LandBankManagement.Data.Services;
using LandBankManagement.Models;
namespace LandBankManagement.Services
{
    public class VendorService:IVendorService
    {
        public VendorService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public IDataServiceFactory DataServiceFactory { get; }
        public ILogService LogService { get; }

        public async Task<VendorModel> GetVendorAsync(long id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await GetVendorAsync(dataService, id);
            }
        }

        static private async Task<VendorModel> GetVendorAsync(IDataService dataService, long id)
        {
            var item = await dataService.GetVendorAsync(id);
            if (item != null)
            {
                return await CreateVendorModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public async Task<IList<VendorModel>> GetVendorsAsync(DataRequest<Vendor> request)
        {
            var collection = new VendorCollection(this, LogService);
            await collection.LoadAsync(request);
            return collection;
        }

        public async Task<IList<VendorModel>> GetVendorsAsync(int skip, int take, DataRequest<Vendor> request)
        {
            var models = new List<VendorModel>();
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var items = await dataService.GetVendorsAsync(skip, take, request);
                foreach (var item in items)
                {
                    models.Add(await CreateVendorModelAsync(item, includeAllFields: false));
                }
                return models;
            }
        }

        public async Task<int> GetVendorsCountAsync(DataRequest<Vendor> request)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.GetVendorsCountAsync(request);
            }
        }

        public async Task<VendorModel> AddVendorAsync(VendorModel model, ICollection<ImagePickerResult> docs)
        {
            long id = model.VendorId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var vendor =  new Vendor();
                if (vendor != null)
                {
                    if (docs != null && docs.Count > 0)
                    {
                        List<VendorDocuments> docList = new List<VendorDocuments>();
                        foreach (var obj in docs)
                        {
                            var doc = new VendorDocuments();
                            UpdateDocumentFromModel(doc, obj);
                            docList.Add(doc);
                        }
                        vendor.VendorDocuments = docList;
                    }
                    UpdateVendorFromModel(vendor, model);
                    vendor.VendorGuid = Guid.NewGuid();
                    await dataService.UpdateVendorAsync(vendor);
                    model.Merge(await GetVendorAsync(dataService, vendor.VendorId));
                }
                return model;
            }
        }

        public async Task<VendorModel> UpdateVendorAsync(VendorModel model, ICollection<ImagePickerResult> docs)
        {
            long id = model.VendorId;
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var vendor = id > 0 ? await dataService.GetVendorAsync(model.VendorId) : new Vendor();
                if (vendor != null)
                {
                    if (docs != null && docs.Count > 0)
                    {
                        List<VendorDocuments> docList = new List<VendorDocuments>();
                        foreach (var obj in docs)
                        {
                            var doc = new VendorDocuments();
                            UpdateDocumentFromModel(doc, obj);
                            docList.Add(doc);
                        }
                        vendor.VendorDocuments = docList;
                    }
                    UpdateVendorFromModel(vendor, model);
                    await dataService.UpdateVendorAsync(vendor);
                    model.Merge(await GetVendorAsync(dataService, vendor.VendorId));
                }
                return model;
            }
        }

        public async Task<int> UploadVendorDocumentsAsync(List<ImagePickerResult> models, Guid guid)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                List<VendorDocuments> docList = new List<VendorDocuments>();
                foreach (var model in models)
                {
                    var doc = new VendorDocuments();

                    UpdateDocumentFromModel(doc, model);
                    doc.VendorGuid = guid;
                    docList.Add(doc);
                }
                return await dataService.UploadVendorDocumentsAsync(docList);
            }
        }

        public async Task<ObservableCollection<ImagePickerResult>> GetDocuments(Guid guid)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                ObservableCollection<ImagePickerResult> docs = new ObservableCollection<ImagePickerResult>();
                var items = await dataService.GetVendorDocumentsAsync(guid);
                foreach (var doc in items)
                {
                    docs.Add(new ImagePickerResult
                    {
                        blobId = doc.VendorBlobId,
                        guid = doc.VendorGuid,
                        FileName = doc.FileName,
                        ImageBytes = doc.FileBlob,
                        ContentType = doc.FileType,
                        Size = doc.FileLength,
                        FileCategoryId = doc.FileCategoryId
                    });
                }

                return docs;
            }
        }
        public async Task<int> DeleteVendorAsync(VendorModel model)
        {
            var vendor = new Vendor { VendorId = model.VendorId };
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.DeleteVendorAsync(vendor);
            }
        }

        public async Task<int> DeleteVendorDocumentAsync(ImagePickerResult documents)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var doc = new VendorDocuments();
                UpdateDocumentFromModel(doc, documents);
                return await dataService.DeleteVendorDocumentAsync(doc);
            }
        }
        //public async Task<int> DeleteVendorRangeAsync(int index, int length, DataRequest<Vendor> request)
        //{
        //    using (var dataService = DataServiceFactory.CreateDataService())
        //    {
        //        var items = await dataService.GetVendorKeysAsync(index, length, request);
        //        return await dataService.DeleteCompanyAsync(items.ToArray());
        //    }
        //}

        static public async Task<VendorModel> CreateVendorModelAsync(Vendor source, bool includeAllFields)
        {
            var model = new VendorModel()
            {
                VendorId = source.VendorId,
                VendorGuid = source.VendorGuid,
                VendorSalutation = source.VendorSalutation,
                VendorLastName = source.VendorLastName,
                VendorName = source.VendorName,
                VendorAlias = source.VendorAlias,
                RelativeName = source.RelativeName,
                RelativeLastName = source.RelativeLastName,
                RelativeSalutation = source.RelativeSalutation,
                ContactPerson = source.ContactPerson,
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                City = source.City,
                PinCode = source.PinCode,
                PhoneNoIsdCode = source.PhoneNoIsdCode,
                PhoneNo = source.PhoneNo,
                email = source.email,
                PAN = source.PAN,
                AadharNo = source.AadharNo,
                GSTIN = source.GSTIN,
                IsVendorActive = source.IsVendorActive
            };
            if (source.VendorDocuments != null && source.VendorDocuments.Count > 0)
            {
                ObservableCollection<ImagePickerResult> docs = new ObservableCollection<ImagePickerResult>();
                foreach (var doc in source.VendorDocuments)
                {
                    docs.Add(new ImagePickerResult
                    {
                        blobId = doc.VendorBlobId,
                        guid = doc.VendorGuid,
                        FileName = doc.FileName,
                        ImageBytes = doc.FileBlob,
                        ContentType = doc.FileType,
                        Size = doc.FileLength,
                        FileCategoryId = doc.FileCategoryId
                    });
                }
                model.VendorDocuments = docs;
            }
            return model;
        }

        private void UpdateVendorFromModel(Vendor target, VendorModel source)
        {
            target.VendorId = source.VendorId;
            target.VendorGuid = source.VendorGuid;
            target.VendorSalutation = source.VendorSalutation;
            target.VendorLastName = source.VendorLastName;
            target.VendorName = source.VendorName;
            target.VendorAlias = source.VendorAlias;
            target.RelativeName = source.RelativeName;
            target.RelativeLastName = source.RelativeLastName;
            target.RelativeSalutation = source.RelativeSalutation;
            target.ContactPerson = source.ContactPerson;
            target.AddressLine1 = source.AddressLine1;
            target.AddressLine2 = source.AddressLine2;
            target.City = source.City;
            target.PinCode = source.PinCode;
            target.PhoneNoIsdCode = source.PhoneNoIsdCode;
            target.PhoneNo = source.PhoneNo;
            target.email = source.email;
            target.PAN = source.PAN;
            target.AadharNo = source.AadharNo;
            target.GSTIN = source.GSTIN;
            target.IsVendorActive = source.IsVendorActive;

        }

        private void UpdateDocumentFromModel(VendorDocuments target, ImagePickerResult source)
        {
            target.VendorBlobId = source.blobId;
            target.VendorGuid = source.guid;
            target.FileBlob = source.ImageBytes;
            target.FileName = source.FileName;
            target.FileType = source.ContentType;
            target.FileCategoryId = source.FileCategoryId;
            target.UploadTime = DateTime.Now;
        }
    }
}
