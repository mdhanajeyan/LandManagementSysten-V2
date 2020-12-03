#region copyright
// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Pickers;

namespace LandBankManagement.Services
{
    public class FilePickerService : IFilePickerService
    {
        public async Task<List<ImagePickerResult>> OpenImagePickerAsync()
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".pdf");
            picker.FileTypeFilter.Add(".docx");

            // picker.FileTypeFilter.Add(".bmp");
            //picker.FileTypeFilter.Add(".gif");
            List<ImagePickerResult> docList = new List<ImagePickerResult>();
            var files = await picker.PickMultipleFilesAsync();
            foreach(var file in files)
            {
                Dictionary<byte[], int> bytes = await GetImageBytesAsync(file);
                docList.Add( new ImagePickerResult
                {
                    FileName = file.Name,
                    ContentType = file.ContentType,
                    ImageBytes = bytes.ElementAt(0).Key,
                    Size = bytes.ElementAt(0).Value// Convert.ToInt32(file.OpenReadAsync().GetResults().Size)
                    // ImageSource = await BitmapTools.LoadBitmapAsync(bytes)
                });
            }
            return docList;
        }

        static private async Task<Dictionary<byte[],int>> GetImageBytesAsync(StorageFile file)
        {
            using (var randomStream = await file.OpenReadAsync())
            { var size = randomStream.Size;

                using (var stream = randomStream.AsStream())
                {
                    byte[] buffer = new byte[randomStream.Size];
                    await stream.ReadAsync(buffer, 0, buffer.Length);
                  
                    return new Dictionary<byte[], int>() { { buffer, Convert.ToInt32(size) } };
                }
            }
        }

        public async Task<bool> DownloadFile(string fileName,byte[] buffer) {
            try
            {
                //var downloadFolder = await DownloadsFolder.CreateFolderAsync("LmsFiles",CreationCollisionOption.GenerateUniqueName);
                //var srcFile = await downloadFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                var srcFile = await DownloadsFolder.CreateFileAsync(fileName, CreationCollisionOption.GenerateUniqueName);
                Stream stream = await srcFile.OpenStreamForWriteAsync();
                stream.Write(buffer, 0, buffer.Length);
                return true;
            }
            catch (Exception ex) {
                return false;
            }
           
        }

    }
}
