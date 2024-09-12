using System.IO;
using System.Threading.Tasks;
using CAP.API.Exceptions;
using Microsoft.AspNetCore.Http;

namespace CAP.API.FileUploads
{
    public static class FileUploader
    {
        /// <summary>
        ///     Joins paths and file names together, and returns the full path.
        ///     Initializes the directory if it doesn't exist.
        ///     Always begins at 
        /// <code>Directory.GetCurrentDirectory()/uploads</code>
        /// </summary>
        /// <param name="paths"></param>
        /// <param name="fileName"></param>
        public static (string fileURL, string uploadPath) CreatePath(string fileName, params string[] paths)
        {
            // Convert List of strings to Path.Combine
            var fileUrl = Path.Combine(Path.Combine(paths), fileName);
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", fileUrl);
            return (fileUrl, uploadPath);
        }

        /// <summary>
        ///     Deletes a file given a path.
        /// </summary>
        /// <param name="path">Path to File to be removed, strips leading / or \ and assumes file is relative to uploads</param>
        /// <returns>True if succesful in removing file</returns>
        public static bool RemoveFile(string path)
        {
            if (path.StartsWith("/") || path.StartsWith("\\"))
            {
                path = path.Remove(0, 1);
            }

            var targetPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", path);
            if (!File.Exists(targetPath)) return false;
            File.Delete(targetPath);
            return true;
        }

        /// <summary>
        ///     Removes a Directory given a path
        /// </summary>
        /// <param name="path">Path of folder to remove, method will strip leading slash and assume deletion is relative to uploads</param>
        /// <returns>True if successful in removing</returns>
        public static bool RemoveDirectory(string path)
        {
            if (path.StartsWith("/") || path.StartsWith("\\"))
            {
                path = path.Remove(0, 1);
            }

            var targetPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", path);
            if (!Directory.Exists(targetPath)) return false;
            Directory.Delete(targetPath, true);
            return true;
        }

        /// <summary>
        ///     Uploads a file to the server.
        ///     Initializes the directory if it doesn't exist.
        ///     Always begins at <code>Directory.GetCurrentDirectory()/uploads</code>
        ///     Validates file type before upload 
        /// </summary>
        /// <param name="file">IFormFile to be uploaded</param>
        /// <param name="filePath">Full FilePath the file should be uploaded to</param>
        /// <param name="clearFolder">If true, will clear the folder</param>
        /// <returns></returns>
        public static async Task Upload(IFormFile file, string filePath, bool? clearFolder = false)
        {
            // Open Read Stream
            var fileStream = file.OpenReadStream();
            // Check first 20 bytes of file for image type
            var isValid = FileValidator.IsValidFile(fileStream, file);

            // Throw an error if there is no valid image
            if (!isValid)
            {
                throw new InvalidFileTypeException();
            }

            // Get path without file name
            var path = Path.GetDirectoryName(filePath);

            if (clearFolder == true && Directory.Exists(path))
            {
                // Delete the directory if it already exists (prevent multiple images on one resource)
                Directory.Delete(path, true);
            }

            // Create Directory if it doesn't exist
            if (!Directory.Exists(path) && path is not null)
            {
                Directory.CreateDirectory(path);
            }

            // Save it to the file path
            await using (var locationStream = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(locationStream);
            }

            // Close the stream
            fileStream.Close();
        }
    }
}