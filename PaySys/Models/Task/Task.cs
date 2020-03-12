using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaySys.Models.Task
{
    public class Task
    {
        public int TaskId { get; set; }
        public int Active { get; set; }
        public string Action { get; set; }
        public string Username { get; set; }
        public string Entity { get; set; }
        public int PaymentType { get; set; }

        [DataType(DataType.MultilineText)]
        public String Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:n}")]
        public Double ActualAmount { get; set; }


        [DisplayFormat(DataFormatString = "{0:n}")]
        public Double QuotedAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:n}")]
        public Double Discount { get; set; }

        public String Reference { get; set; }
        public String OpReference { get; set; }

        public String DocumentUrl { get; set; }

        //[Required]
        //[Display(Name = "I ")]
        public int Status { get; set; }

        public String DocumentUrl2 { get; set; }

        public String Comment { get; set; }

        public String ExpType { get; set; }

        public String BusUnit { get; set; }

        public int StatusId { get; set; }

        public String Priority { get; set; }

        public String Name { get; set; }

        public int Quantity { get; set; }

        public string TaskName { get; set; }


        [Required]
        [Display(Name = "Job Title ")]
        public int PriorityId { get; set; }
        public List<SelectListItem> TaskPriorities { get; set; }


        [Required]
        [Display(Name = "Job Title ")]
        public int DepartmentId { get; set; }
        public List<SelectListItem> Departments { get; set; }

      
        [Required]
        [Display(Name = "Job Title ")]
        public int CurrencyId { get; set; }
        public List<SelectListItem> Currencies { get; set; }


        public String RequestedBy { get; set; }

        public String CompletedBy { get; set; }

        public String DeferredBy { get; set; }

        public DateTime DeferredTo { get; set; }

        public String taskStatus { get; set; }

        public String AssignedTo { get; set; }

        public String Commment { get; set; }

        [Required]
        [Display(Name = "Employee Name ")]
        public String EmployeeName { get; set; }
        public String EmployeeNumber { get; set; }
        public List<SelectListItem> Employees { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]      
        public DateTime CreatedOn { get; set; }


        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]      
        public DateTime FinishBy { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime CompletedOn { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Joining Date ")]
        public DateTime ModifiedOn { get; set; }


        [DataType(DataType.Upload)]
        [Display(Name = "File Attachment")]
        public String PostedFile { get; set; }
    }
}