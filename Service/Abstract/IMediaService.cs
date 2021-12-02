using Entity.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace  Service.Abstract
{
    public interface IMediaService:IGenericService<Media>
    {
        string ConvertWebp(string fileName);
        Media GetByFileName(string fileName);
        string FileUploadDirectory(string project);
        void DeleteMedia(string fileName);
        public string ChangeFileName(string title, IFormFile file);
        bool MediaExits(string FileName);
        void DeleteImage(string fileName, string project, string filePath);
        IQueryable<Media> GetAllByDate();
        string Insert(string filePath, string title, string project, string mediaConfirm, string newFilePath, IFormFile file, string oldFileName, string folderName, int quality, string oldFileType);
        string InsertWithIFormFile(string filePath, string title, string project, IFormFile file, string folderName, int quality, int w, int h);
        string InsertFiles(string filePath, string title, string project, IEnumerable<IFormFile> files, string folderName, int quality, int w, int h);
        string InsertFile(string filePath, string title, string project, IFormFile file);
        void AddEmailAttachments(Email entity, IEnumerable<IFormFile> attachments);
        void InsertProductImages(Product product, IEnumerable<IFormFile> attachments);
    }
}
