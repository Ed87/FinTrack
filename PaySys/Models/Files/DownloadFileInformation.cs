using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PaySys.Models.Files
{
    public class DownLoadFileInformation
    {
        public int FileId { get; set; }

        [Display(Name = "Filename ")]
        public string FileName { get; set; }


        [Display(Name = "Filename ")]
        public string BatchName { get; set; }

        [Display(Name = "Location ")]
        public string FilePath { get; set; }

        [Display(Name = "Export Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }


        [Display(Name = "Size ")]
        public long FileSize { get; set; }
    }
}