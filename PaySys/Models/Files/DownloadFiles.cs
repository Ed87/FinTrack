using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace PaySys.Models.Files
{
    public class DownloadFiles
    {
        public List<DownLoadFileInformation> GetFiles()
        {
            List<DownLoadFileInformation> lstFiles = new List<DownLoadFileInformation>();

            DirectoryInfo dirInfo = new DirectoryInfo(HostingEnvironment.MapPath("~/TaskReports"));

            int i = 0;
            foreach (var item in dirInfo.GetFiles())
            {

                lstFiles.Add(new DownLoadFileInformation()
                {

                    FileId = i + 1,
                    FileName = item.Name,
                    FileSize = item.Length,
                    FilePath = dirInfo.FullName + @"\" + item.Name

                });
                i = i + 1;
            }
            return lstFiles;
        }

        public List<DownLoadFileInformation> GetIncomingBatches()
        {
            List<DownLoadFileInformation> lstFiles = new List<DownLoadFileInformation>();

            //  DirectoryInfo dirInfo = new DirectoryInfo(HostingEnvironment.MapPath("~/BatchIn"));
            DirectoryInfo dirInfo = new DirectoryInfo(@"\\10.170.8.56\Export2");

            int i = 0;
            foreach (var item in dirInfo.GetFiles())
            {

                lstFiles.Add(new DownLoadFileInformation()
                {

                    FileId = i + 1,
                    FileName = item.Name,
                    FileSize = item.Length,
                    FilePath = dirInfo.FullName + @"\" + item.Name

                });
                i = i + 1;
            }
            return lstFiles;
        }
    }
}