using PaySys.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaySys.Models
{
    public class Invoice
    {
        public string Action { get; set; }
        public string Username { get; set; }
        public string Entity { get; set; }
        public int InvoiceId { get; set; }

        public int Active { get; set; }

        public Boolean Act { get; set; }

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

        public String Dpt { get; set; }

        [Required]
        [Display(Name = "I ")]
        public int Approve { get; set; }

        public String DocumentUrl2 { get; set; }


        public String ExpType { get; set; }

        public String BusUnit { get; set; }

        public int StatusId { get; set; }

        public String Vendor { get; set; }

        public String Allocation { get; set; }

        public int Quantity { get; set; }

        public int Format { get; set; }


        [Required]
        [Display(Name = "Job Title ")]
        public int FormatId { get; set; }
        public List<SelectListItem> FileFormats { get; set; }

        [Required]
        [Display(Name = "Job Title ")]
        public int StatusesId { get; set; }
        public List<SelectListItem> Statuses { get; set; }


        [Required]
        [Display(Name = "Job Title ")]
        public int DepartmentId { get; set; }
        public List<SelectListItem> Departments { get; set; }

        [Required]
        [Display(Name = "Job Title ")]
        public SelectList Formats { get; set; }
        public string SelectedFormat { get; set; }




        [Required]
        [Display(Name = "Job Title ")]
        public int AllocationId { get; set; }
        public List<SelectListItem> Services { get; set; }


        [Required]
        [Display(Name = "Job Title ")]
        public int PaymentTypeId { get; set; }
        public List<SelectListItem> PaymentTypes { get; set; }


        [Required]
        [Display(Name = "Job Title ")]
        public int VendorId { get; set; }
        public List<SelectListItem> Vendors { get; set; }


        [Required]
        [Display(Name = "Job Title ")]
        public int UnitId { get; set; }
        public List<SelectListItem> Units { get; set; }



        [Required]
        [Display(Name = "Job Title ")]
        public int ExpId { get; set; }
        public List<SelectListItem> Expenses { get; set; }


        [Required]
        [Display(Name = "Job Title ")]
        public int CurrencyId { get; set; }
        public List<SelectListItem> Currencies { get; set; }


        public String ModifiedBy { get; set; }
        public String RequestedBy { get; set; }

        public String Status { get; set; }

        public String VendorName { get; set; }

        public String AllocationName { get; set; }


        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Joining Date ")]
        public DateTime CreatedOn { get; set; }


        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Joining Date ")]
        public DateTime SettledOn { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Joining Date ")]
        public DateTime ModifiedOn { get; set; }


        [DataType(DataType.Upload)]
        [Display(Name = "File Attachment")]
        public String PostedFile { get; set; }

        public static Invoice find(int id)
        {
            var Invoice = new Invoice();
            SqlConnection _connection = new SqlConnection(Helpers.Helpers.DatabaseConnect);

            try
            {
                using (_connection)
                {
                    var query = String.Format("SELECT * FROM Invoices WHERE InvoiceId={0}", id);

                    SqlCommand cmd = new SqlCommand(query, _connection);

                    _connection.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    if (dataReader.Read())
                    {

                        Invoice = allocateDetails(dataReader);
                    }
                    _connection.Close();
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
            return Invoice;
        }

        public static Invoice allocateDetails(SqlDataReader dataReader)
        {
            var Invoice = new Invoice();
            try
            {
               
                Invoice.InvoiceId = Convert.ToInt32(dataReader["InvoiceId"]);
                Invoice.DocumentUrl = dataReader["DocumentUrl"].ToString();
            }

            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }
               
            return Invoice;

        }
      
        

    }
}