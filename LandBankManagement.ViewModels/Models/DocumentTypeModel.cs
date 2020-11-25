using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandBankManagement.Models
{
    public class DocumentTypeModel : ObservableObject
    {
        static public DocumentTypeModel CreateEmpty() => new DocumentTypeModel { DocumentTypeId = -1, IsEmpty = true };

        public int DocumentTypeId { get; set; }
        public Guid DocumentTypeGuid { get; set; }
        public string DocumentTypeName { get; set; }
        public bool IsDocumentTypeActive { get; set; }
        
        public bool IsNew => DocumentTypeId <= 0;

        public override void Merge(ObservableObject source)
        {
            if (source is DocumentTypeModel model)
            {
                Merge(model);
            }
        }

        public void Merge(DocumentTypeModel source)
        {
            if (source != null)
            {
                DocumentTypeId = source.DocumentTypeId;
                DocumentTypeGuid = source.DocumentTypeGuid;
                DocumentTypeName = source.DocumentTypeName;
                IsDocumentTypeActive = source.IsDocumentTypeActive;
            }
        }
    }
}
