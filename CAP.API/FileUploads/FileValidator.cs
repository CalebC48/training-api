

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace CAP.API.FileUploads
{
    public class ValidType
    {
        public byte[] ValidTypesBytes { get; set; }
        public string MimeType { get; set; }
        public List<string> Extension { get; set; }
    }
    public class FileValidator
    {

        private static ValidType CreateValidationType(string mimType, byte[] validTypes, List<string> fileExtension)
        {
            return new ValidType
            {
                ValidTypesBytes = validTypes,
                MimeType = mimType,
                Extension = fileExtension
            };
        }

        private static bool FileSizeOkay(IFormFile file)
        {
            const decimal MAX_FILE_SIZE = 5; // 5MB, this is in MB
            decimal fileSize = file.Length;
            if (fileSize > MAX_FILE_SIZE * 1048576)
            {
                return false;
            }
            return true;
        }
        
        public static bool IsValidFile(Stream imageStream, IFormFile file)
        {

            // Check content type of the file
            var contentType = file.ContentType;
            var fileName = file.Name;

            if (imageStream.Length == 0 || !FileSizeOkay(file))
            {
                return false;
            }

            byte[] header = new byte[20];
            var bmp = CreateValidationType("image/bmp", Encoding.ASCII.GetBytes("BM"), new List<string> { "bmp" });     // BMP
            var gif = CreateValidationType("image/gif", Encoding.ASCII.GetBytes("GIF"), new List<string> { "gif" });    // GIF
            var png = CreateValidationType("image/png", new byte[] { 0x89, 0x50, 0x4E, 0x47 }, new List<string> { "png" });// PNG
            var tiff = CreateValidationType("image/tiff", new byte[] { 0x49, 0x49, 0x2A }, new List<string> { "tiff", "tif" });         // TIFF
            var tiff2 = CreateValidationType("image/tiff", new byte[] { 0x4D, 0x4D, 0x2A }, new List<string> { "tiff", "tif" });         // TIFF
            var jpeg = CreateValidationType("image/jpeg", new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 }, new List<string> { "jpg", "jpeg" }); // jpeg cannon
            var jpeg2 = CreateValidationType("image/jpeg", new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 }, new List<string> { "jpg", "jpeg" }); // jpeg
            var jpeg3 = CreateValidationType("image/jpeg", new byte[] { 0xFF, 0xD8, 0xFF, 0xDB }, new List<string> { "jpg", "jpeg" }); // jpeg) 
                                                                                                                                       // Docx File

            var docx = CreateValidationType("application/vnd.openxmlformats-officedocument.wordprocessingml.document", new byte[] { 0x50, 0x4B, 0x03, 0x04 }, new List<string> { "docx" });
            var pdf = CreateValidationType("application/pdf", new byte[] { 0x25, 0x50, 0x44, 0x46 }, new List<string> { "pdf" });

            // mp4
            var mp4 = CreateValidationType("video/mp4", new byte[] { 0x00, 0x00, 0x00, 0x18, 0x66, 0x74, 0x79, 0x70, 0x6D, 0x70, 0x34, 0x32 }, new List<string> { "mp4" });
            // avi
            var avi = CreateValidationType("video/x-msvideo", new byte[] { 0x52, 0x49, 0x46, 0x46 }, new List<string> { "avi" });
            // mpeg4
            var mpeg4 = CreateValidationType("video/mp4", new byte[] { 0x00, 0x00, 0x01, 0xB0 }, new List<string> { "mp4" });
            // mov
            var mov = CreateValidationType("video/quicktime", new byte[] { 0x00, 0x00, 0x00, 0x14, 0x66, 0x74, 0x79, 0x70, 0x4D, 0x34, 0x56 }, new List<string> { "mov" });
            //*/
            //255 to hex

            var types = new List<ValidType> { bmp, gif, png, tiff, tiff2, jpeg, jpeg2, jpeg3, docx, pdf, mp4, avi, mpeg4, mov };

            // Loop through the image bytes and check against the types
            for (int i = 0; i < types.Count; i++)
            {
                imageStream.Seek(0, SeekOrigin.Begin);
                imageStream.Read(header, 0, header.Length);
                if (header.Take(types[i].ValidTypesBytes.Count()).SequenceEqual(types[i].ValidTypesBytes) && types[i].MimeType == contentType && types[i].Extension.Contains(Path.GetExtension(fileName).Replace(".", "")))
                {
                    imageStream.Seek(0, SeekOrigin.Begin); //reset stream back to beginning before returning
                    return true;
                }
            }

            return false;

        }
    }
}
