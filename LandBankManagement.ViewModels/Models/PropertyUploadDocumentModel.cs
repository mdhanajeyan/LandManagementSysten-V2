using System;
using System.Collections.Generic;
using System.Text;

namespace LandBankManagement.Models
{
   public class PropertyUploadDocumentModel : ObservableObject
    {
        public int blobId { get; set; }
        public Guid guid { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] ImageBytes { get; set; }
        //public object ImageSource { get; set; }
        public int Size { get; set; }
        public int FileCategoryId { get; set; }
        public int Identity { get; set; }
        public int DocumentType { get; set; }
    }

}
