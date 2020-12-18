using LandBankManagement.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace LandBankManagement.Models
{
    public class PropertyCheckListDocumentsModel : ImagePickerResult
    {       
        public int PropertyCheckListBlobId { get; set; }
        public DateTime UploadTime { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public DateTimeOffset ActualCompletionDate { get; set; }
        public string Remarks { get; set; }
    }
}
