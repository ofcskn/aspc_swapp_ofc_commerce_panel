using Entity.Context;
using Entity.Models;
using ImageProcessor;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.Formats;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Service.Abstract;
using Service.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Service.Concrete.EntityFramework
{
    public class EfMediaService : EfGenericService<Media>, IMediaService
    {
        IHostingEnvironment _webHostEnvironment;
        public EfMediaService(SwappDbContext _context, IHostingEnvironment webHostEnvironment) : base(_context)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }
        public string ConvertWebp(string fileName)
        {
            string newFileName = Path.GetFileNameWithoutExtension(fileName) + ".webp";
            return newFileName;
        }
        public IQueryable<Media> GetAllByDate()
        {
            return _db.Media.OrderByDescending(p => p.Date);
        }
        public string FileUploadDirectory(string project)
        {
            int lastIndexOf = _webHostEnvironment.ContentRootPath.LastIndexOf(_webHostEnvironment.ApplicationName);
            string path = _webHostEnvironment.ContentRootPath.Substring(0, lastIndexOf);
            if (Directory.Exists(path + @"\www"))
            {
                path = path + @"\" + project;
            }
            return path + @"\wwwroot";
        }

        public void DeleteMedia(string fileName)
        {
            var media = _db.Media.FirstOrDefault(p => p.FileNames == fileName);
            try
            {
                _db.Media.Remove(media);
                _db.SaveChanges();
            }
            catch
            {
            }
        }
        public string ChangeFileName(string title, IFormFile file)
        {
            if (title == null) { title = "default"; }
            string fileName = Helpers.ToSlug(title);
            string extension = Path.GetExtension(file.FileName);
            return ChangeLastChar(fileName, extension);
        }
        public bool MediaExits(string fileName)
        {
            if (_db.Media.Any(p => p.FileNames == fileName))//Burada klasörü de kontrol edecek bir sistem yazılamlı.
            {
                bool a = true;
                return a;
            }
            return false;
        }
        public string ChangeLastChar(string fileName, string extension)
        {
            if (MediaExits(fileName + extension) == true)
            {
                string lastChar = fileName.Substring(fileName.Length - 1);

                if (lastChar.All(char.IsDigit))
                {
                    int lastIndexOf = fileName.LastIndexOf(lastChar);
                    int count = lastChar.Length;

                    string fileNameWithoutLastChar = fileName.Remove(lastIndexOf, count);

                    fileName = fileNameWithoutLastChar + (Convert.ToInt32(lastChar) + 1).ToString();
                    return ChangeLastChar(fileName, extension);
                }
                else
                {
                    fileName = fileName + "-1";
                    return ChangeLastChar(fileName, extension);
                }
            }
            return fileName + extension;
        }
        public Media GetByFileName(string fileName)
        {
            Media media = _db.Media.FirstOrDefault(p => p.FileNames == fileName);
            return media;
        }
        public void DeleteImage(string fileName, string project, string filePath)
        {
            if (filePath == null)
            {
                filePath = GetByFileName(fileName).Path;
            }
            try
            {
                DeleteMedia(fileName);
            }
            catch { }

            try
            {
                string fullPath = FileUploadDirectory(project) + filePath + fileName;
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            catch { }

            try
            {
                string fullPath = FileUploadDirectory(project) + filePath + Path.GetFileNameWithoutExtension(fileName) + ".webp";
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            catch { }
        }

        public string Insert(string filePath, string title, string project, string mediaConfirm, string newFilePath, IFormFile file, string oldFileName, string folderName, int quality, string oldFileType)
        {
            filePath = Helpers.ChangeFilePath(filePath);
            string fileName;
            string[] allowedFileTypes = new string[] { "image/jpeg", "image/png", "image/jpg", "image/webp" };

            if (!allowedFileTypes.Contains(oldFileType)) return null;
            if (title != null)
            {
                fileName = ChangeFileName(title, file);
            }
            else
            {
                fileName = "default.jpg";
            }

            string extension = "." + file.ContentType.Replace("image/", "");

            if (mediaConfirm == "add")
            {

                Add(new Media()
                {
                    FileNames = fileName + extension,
                    Date = DateTime.Now,
                    Path = filePath,
                    Title = title,
                    Enabled = true,
                    Folder = folderName,
                    Project = project
                });
            }
            else
            {
                Add(new Media()
                {
                    FileNames = fileName,
                    Date = DateTime.Now,
                    Path = filePath,
                    Title = title,
                    Enabled = true,
                    Folder = folderName,
                    Project = project
                });
            }

            string webPFilePath;
            string fileUploadDirectory;

            fileUploadDirectory = FileUploadDirectory(project) + filePath + fileName + extension;
            webPFilePath = FileUploadDirectory(project) + filePath + fileName + ".webp";

            if (file.ContentType == "image/png")
            {
                //convert jpg with package
                using (FileStream fs = System.IO.File.Create(fileUploadDirectory))
                {
                    ISupportedImageFormat format = new PngFormat { Quality = 70 };
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                    {
                        imageFactory.Load(file.OpenReadStream()).Format(format).Save(fs);
                    }
                }
            }
            else
            {
                //convert jpg with package
                using (FileStream fs = System.IO.File.Create(fileUploadDirectory))
                {
                    ISupportedImageFormat jpegFormat = new JpegFormat { Quality = quality };
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                    {
                        imageFactory.Load(file.OpenReadStream()).Format(jpegFormat).Save(fs);
                    }
                }
            }

            try
            {
                //convert webp with package
                using (FileStream fs = System.IO.File.Create(webPFilePath))
                {
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                    {
                        imageFactory.Load(file.OpenReadStream()).Format(new WebPFormat()).Quality(quality).Save(fs);
                    }
                }
            }
            catch { }

            return fileName;
        }

        public string InsertWithIFormFile(string filePath, string title, string project, IFormFile file, string folderName, int quality, int w, int h)
        {
            filePath = Helpers.ChangeFilePath(filePath);
            string fileName;

            if (title != null)
            {
                fileName = ChangeFileName(title, file);
            }
            else
            {
                fileName = file.FileName;
            }
            Add(new Media()
            {
                FileNames = fileName,
                Date = DateTime.Now,
                Path = filePath,
                Title = title,
                Enabled = true,
                Folder = folderName,
                Project = project
            });
            string webPFilePath;
            string fileUploadDirectory;
            fileUploadDirectory = FileUploadDirectory(project) + filePath + fileName;
            webPFilePath = FileUploadDirectory(project) + filePath + fileName.Substring(0, fileName.IndexOf(".")) + ".webp";
            try
            {
                if (w == 0 || h == 0)
                {
                    if (file.ContentType == "image/png")
                    {
                        //convert jpg with package
                        using (FileStream fs = System.IO.File.Create(fileUploadDirectory))
                        {
                            ISupportedImageFormat format = new PngFormat { Quality = 70 };
                            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                            {
                                imageFactory.Load(file.OpenReadStream()).Format(format).Save(fs);
                            }
                        }
                    }
                    else
                    {
                        using (FileStream fs = System.IO.File.Create(fileUploadDirectory))
                        {
                            ISupportedImageFormat jpegFormat = new JpegFormat { Quality = quality };
                            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                            {
                                imageFactory.Load(file.OpenReadStream()).Format(jpegFormat).Save(fs);
                            }
                        }
                    }
                }

                else
                {
                    if (file.ContentType == "image/png")
                    {
                        Size size = new Size(w, h);
                        ResizeLayer resizeLayer = new ResizeLayer(size, ResizeMode.Pad);
                        using (FileStream fs = System.IO.File.Create(fileUploadDirectory))
                        {
                            ISupportedImageFormat format = new PngFormat { Quality = 70 };
                            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                            {
                                imageFactory.Load(file.OpenReadStream()).Resize(resizeLayer).Format(format).Save(fs);
                            }
                        }
                    }
                    else
                    {
                        Size size = new Size(w, h);
                        ResizeLayer resizeLayer = new ResizeLayer(size, ResizeMode.Crop);
                        using (FileStream fs = System.IO.File.Create(fileUploadDirectory))
                        {
                            ISupportedImageFormat jpegFormat = new JpegFormat { Quality = quality };
                            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                            {
                                imageFactory.Load(file.OpenReadStream()).Format(jpegFormat).Resize(resizeLayer).Save(fs);
                            }
                        }
                    }
                }
            }
            catch { }
            try
            {
                if (w == 0 || h == 0)
                {
                    using (FileStream fs = System.IO.File.Create(webPFilePath))
                    {
                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                        {
                            imageFactory.Load(file.OpenReadStream()).Format(new WebPFormat()).Quality(quality).Save(fs);
                        }
                    }

                }
                else
                {
                    Size size = new Size(w, h);
                    ResizeLayer resizeLayer = new ResizeLayer(size, ResizeMode.Crop);
                    using (FileStream fs = System.IO.File.Create(webPFilePath))
                    {
                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                        {
                            imageFactory.Load(file.OpenReadStream()).Format(new WebPFormat()).Resize(resizeLayer).Quality(quality).Save(fs);
                        }
                    }
                }
                //convert webp with package

            }
            catch { }
            return fileName;
        }

        public string InsertFiles(string filePath, string title, string project, IEnumerable<IFormFile> files, string folderName, int quality, int w, int h)
        {
            filePath = Helpers.ChangeFilePath(filePath);
            string fileName;

            foreach (var file in files)
            {
                if (title != null)
                {
                    fileName = ChangeFileName(title, file);
                }
                else
                {
                    fileName = file.FileName;
                }

                string webPFilePath;
                string fileUploadDirectory;
                fileUploadDirectory = FileUploadDirectory(project) + filePath + fileName;

                if (file.ContentType.Contains("image"))
                {
                    webPFilePath = FileUploadDirectory(project) + filePath + fileName.Substring(0, fileName.IndexOf(".")) + ".webp";
                    try
                    {
                        if (w == 0 || h == 0)
                        {
                            if (file.ContentType == "image/png")
                            {
                                //convert jpg with package
                                using (FileStream fs = System.IO.File.Create(fileUploadDirectory))
                                {
                                    ISupportedImageFormat format = new PngFormat { Quality = 70 };
                                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                                    {
                                        imageFactory.Load(file.OpenReadStream()).Format(format).Save(fs);
                                    }
                                }
                            }
                            else
                            {
                                using (FileStream fs = System.IO.File.Create(fileUploadDirectory))
                                {
                                    ISupportedImageFormat jpegFormat = new JpegFormat { Quality = quality };
                                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                                    {
                                        imageFactory.Load(file.OpenReadStream()).Format(jpegFormat).Save(fs);
                                    }
                                }
                            }
                        }

                        else
                        {
                            if (file.ContentType == "image/png")
                            {
                                Size size = new Size(w, h);
                                ResizeLayer resizeLayer = new ResizeLayer(size, ResizeMode.Pad);
                                using (FileStream fs = System.IO.File.Create(fileUploadDirectory))
                                {
                                    ISupportedImageFormat format = new PngFormat { Quality = 70 };
                                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                                    {
                                        imageFactory.Load(file.OpenReadStream()).Resize(resizeLayer).Format(format).Save(fs);
                                    }
                                }
                            }
                            else
                            {
                                Size size = new Size(w, h);
                                ResizeLayer resizeLayer = new ResizeLayer(size, ResizeMode.Crop);
                                using (FileStream fs = System.IO.File.Create(fileUploadDirectory))
                                {
                                    ISupportedImageFormat jpegFormat = new JpegFormat { Quality = quality };
                                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                                    {
                                        imageFactory.Load(file.OpenReadStream()).Format(jpegFormat).Resize(resizeLayer).Save(fs);
                                    }
                                }
                            }
                        }
                    }
                    catch { }
                    try
                    {
                        if (w == 0 || h == 0)
                        {
                            using (FileStream fs = System.IO.File.Create(webPFilePath))
                            {
                                using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                                {
                                    imageFactory.Load(file.OpenReadStream()).Format(new WebPFormat()).Quality(quality).Save(fs);
                                }
                            }

                        }
                        else
                        {
                            Size size = new Size(w, h);
                            ResizeLayer resizeLayer = new ResizeLayer(size, ResizeMode.Crop);
                            using (FileStream fs = System.IO.File.Create(webPFilePath))
                            {
                                using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                                {
                                    imageFactory.Load(file.OpenReadStream()).Format(new WebPFormat()).Resize(resizeLayer).Quality(quality).Save(fs);
                                }
                            }
                        }
                        //convert webp with package

                    }
                    catch { }
                }
                else
                {
                    using (FileStream fs = System.IO.File.Create(fileUploadDirectory))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                }
            }
            return "asdasd";

        }
        public string InsertFile(string filePath, string title, string project, IFormFile file)
        {
            filePath = Helpers.ChangeFilePath(filePath);
            string fileName;

            if (title != null)
            {
                fileName = ChangeFileName(title, file);
            }
            else
            {
                fileName = file.FileName;
            }

            string fileUploadDirectory;
            fileUploadDirectory = FileUploadDirectory(project) + filePath + fileName;

            using (FileStream fs = System.IO.File.Create(fileUploadDirectory))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
            return fileName;
        }
        public void AddEmailAttachments(Email entity, IEnumerable<IFormFile> attachments)
        {
            try
            {
                foreach (var attachment in attachments)
                {
                    if (attachment != null)
                    {
                        if (attachment.Length < 10485760)
                        {
                            var newFileName = InsertFile("/uploads/email/", Path.GetFileNameWithoutExtension(attachment.FileName), "admin", attachment);

                            EmailAttachments ea = new EmailAttachments
                            {
                                Filename = newFileName,
                                SendDate = DateTime.Now,
                                ContentType = attachment.ContentType,
                                Size = attachment.Length.ToString(),
                                MailId = entity.Id
                            };

                            _db.EmailAttachments.Add(ea);

                        }
                        else
                        {

                        }
                    }
                }
                _db.SaveChanges();
            }
            catch (Exception)
            {
            }
        }
        public void InsertProductImages(Product entity, IEnumerable<IFormFile> attachments)
        {
            try
            {
                foreach (var attachment in attachments)
                {
                    if (attachment != null)
                    {
                        if (attachment.Length < 10485760)
                        {
                            var newFileName = InsertFile("/img/product/", entity.Name, "admin", attachment);

                            ProductImage pi = new ProductImage
                            {
                                Image = newFileName,
                                ProductId = entity.Id
                            };

                            Media media = new Media
                            {
                                Enabled = true,
                                Date = DateTime.Now,
                                FileNames = newFileName,
                                Project = "admin",
                                Folder = "/img/product",
                                Title = newFileName
                            };

                            _db.Media.Add(media);
                            _db.ProductImage.Add(pi);
                        }
                    }
                }
                _db.SaveChanges();
            }
            catch (Exception)
            {
            }
        }
    }
}
