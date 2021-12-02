using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Service.Abstract;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Service.Utilities;
using Entity.Models;
using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using System.Linq;
using admin.Models;
using ImageProcessor.Imaging.Formats;

namespace admin.Controllers
{
    [Authorize]
    public class MediaController : Controller
    {

        private readonly IUnitOfWork _uow;
        public MediaController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [Authorize(Policy = "User")]
        [HttpGet]
        public IActionResult Manage()
        {
            return View();
        }
        [Authorize(Policy = "User")]
        [HttpGet]
        public async Task<IActionResult> List(int pageNumber = 1)
        {
            return View(await PaginatedList<Media>.CreateAsync(_uow.Media.GetAllByDate(), pageNumber, 20));
        }
        [Authorize(Policy = "Admin")]
        public JsonResult Insert(string filePath, string title, string project, string mediaConfirm, string newFilePath, string folderName, int quality, string oldFileType, IFormFile mediaFile)
        {
            var files = Request.Form.Files;
            if (files.Count > 1)
            {
                foreach (var file in files)
                {
                    var oldFileName = Request.Form.Files[0].FileName;
                    var newFileName = _uow.Media.Insert(filePath, title, project, mediaConfirm, newFilePath, file, oldFileName, folderName, quality, oldFileType);
                    if (newFileName != null)
                    {
                        var newData = new
                        {
                            newFileName = newFileName,
                            oldFileName = oldFileName,
                        };
                        return Json(newData);
                    }
                    else
                    {
                        return Json("not-valid-type");
                    }
                }
                return Json("multiple-success");
            }
            else
            {
                IFormFile file;
                string oldFileName;
                if (files != null)
                {
                    file = files[0];
                    oldFileName = Request.Form.Files[0].FileName;
                }
                else
                {
                    file = mediaFile;
                    oldFileName = mediaFile.FileName;
                }
                var newFileName = _uow.Media.Insert(filePath, title, project, mediaConfirm, newFilePath, file, oldFileName, folderName, quality, oldFileType);
                if (newFileName != null)
                {
                    var newData = new
                    {
                        newFileName = newFileName,
                        oldFileName = oldFileName,
                    };
                    return Json(newData);
                }
                else
                {
                    return Json("not-valid-type");
                }
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public JsonResult Delete(string fileName, string project, string filePath)
        {
            _uow.Media.DeleteImage(fileName, project, filePath);
            return Json(fileName);
        }
    }
}
