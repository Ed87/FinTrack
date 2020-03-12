using PaySys.Models.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaySys.Controllers
{
    public class FileProcessController : Controller
    {
        // GET: FileProcess
        DownloadFiles obj;
        public FileProcessController()
        {
            obj = new DownloadFiles();
        }

        // GET: FileProcess
        public ActionResult Index()
        {
            var filesCollection = obj.GetFiles();
            return View(filesCollection);
        }
        public FileResult Download(string id)
        {

            int fid = Convert.ToInt32(id);
            var files = obj.GetFiles();
            string filename = (from f in files
                               where f.FileId == fid
                               select f.FilePath).First();
            string contentType = string.Empty;
            if (filename.Contains(".xlsx"))
            {
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }


            return File(filename, contentType, filename);
        }

        public ActionResult IncomingBatches()
        {
            var filesCollection = obj.GetIncomingBatches();
            return View(filesCollection);
        }
        public FileResult DownloadIncomingBatch(string id)
        {

            int fid = Convert.ToInt32(id);
            var files = obj.GetIncomingBatches();
            string filename = (from f in files
                               where f.FileId == fid
                               select f.FilePath).First();
            string contentType = string.Empty;
            if (filename.Contains(".xml"))
            {
                contentType = "application/xml";
            }
            return File(filename, contentType, filename);
        }
    }
}
