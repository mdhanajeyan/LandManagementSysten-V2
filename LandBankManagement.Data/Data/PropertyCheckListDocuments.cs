using System;
using System.ComponentModel.DataAnnotations;

namespace LandBankManagement.Data
{
    public class PropertyCheckListDocuments
    {
        [Key]
        public int PropertyCheckListBlobId { get; set; }
        public Guid PropertyGuid { get; set; }
        public byte[] FileBlob { get; set; }
        public string FileName { get; set; }
        public int FileLenght { get; set; }
        public string FileType { get; set; }
        public int FileCategoryId { get; set; }
        public DateTime UploadTime { get; set; }
        public DateTime DueDate {get;set;}
        public DateTime ActualCompletionDate { get; set; }
        public string Remarks { get; set; }
    }
}
