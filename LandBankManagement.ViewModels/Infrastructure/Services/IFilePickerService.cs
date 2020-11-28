﻿using System;
using System.Threading.Tasks;

namespace LandBankManagement.Services
{
    public class ImagePickerResult
    {
        public int blobId { get; set; }
        public Guid guid { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] ImageBytes { get; set; }
        //public object ImageSource { get; set; }
        public int Size { get; set; }
        public int FileCategoryId { get; set; }
    }

    public interface IFilePickerService
    {
        Task<ImagePickerResult> OpenImagePickerAsync();
    }
}
