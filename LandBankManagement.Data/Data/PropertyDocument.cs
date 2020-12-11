using System;
using System.Collections.Generic;
using System.Text;

namespace LandBankManagement.Data.Data
{
    public class PropertyDocument
    {

        public int PropertyBlobId { get; set; }
        public Guid PropertyGuid { get; set; }
        public byte[] FileBlob { get; set; }
        public byte[] FileName { get; set; }
        public int FileLenght { get; set; }
        public string FileType { get; set; }
        public int FileCategoryId { get; set; }
        public DateTime UploadTime { get; set; }

    }
}
