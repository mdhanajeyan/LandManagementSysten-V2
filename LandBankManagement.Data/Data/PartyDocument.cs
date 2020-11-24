using System;
using System.ComponentModel.DataAnnotations;

namespace LandBankManagement.Data { 
    public class PartyDocument
    {
        [Key]
        public int PartyBlobId { get; set; }
        public Guid PartyGuid { get; set; }
        public byte[] FileBlob { get; set; }
        public string FileName { get; set; }
        public int FileLength { get; set; }
        public string FileType { get; set; }
        public int FileCategoryId { get; set; }
        public DateTime UploadTime { get; set; }
      
    }
}
