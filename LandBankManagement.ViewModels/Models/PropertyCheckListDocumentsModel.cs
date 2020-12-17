using System;
using System.Collections.Generic;
using System.Text;

namespace LandBankManagement.Models
{
    public class PropertyCheckListDocumentsModel
    {
        public int PropertyCheckListBlobId { get; set; }
        public Guid PropertyGuid { get; set; }
        public byte[] FileBlob { get; set; }
        public string FileName { get; set; }
        public int FileLenght { get; set; }
        public string FileType { get; set; }
        public int FileCategoryId { get; set; }
        public DateTime UploadTime { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ActualCompletionDate { get; set; }
        public string Remarks { get; set; }
    }
}
