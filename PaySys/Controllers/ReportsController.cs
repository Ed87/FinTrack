using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PaySys.Helpers;
using PaySys.Models;
using PaySys.Exceptions;
using static PaySys.Helpers.Helpers;
using CrystalDecisions.Shared;

namespace PaySys.Controllers
{
    public class ReportsController : Controller
    {
       // Invoice invoice = new Invoice();
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }


        public static List<SelectListItem> PopulateCurrencies()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            using (SqlConnection conn = new SqlConnection(DatabaseConnect))
            {

                SqlCommand cmd = new SqlCommand("SELECT CurrencyId,CurrencyName FROM Currencies ORDER BY CurrencyId", conn);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    items.Add(new SelectListItem
                    {
                        Text = Convert.ToString(dr["CurrencyName"]),
                        Value = Convert.ToString(dr["CurrencyId"])

                    });
                }

                conn.Close();
            }

            return items;
        }
        public static List<SelectListItem> PopulateVendors()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            using (SqlConnection conn = new SqlConnection(DatabaseConnect))
            {

                SqlCommand cmd = new SqlCommand("SELECT VendorId,VendorName FROM Vendors ORDER BY VendorId", conn);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    items.Add(new SelectListItem
                    {
                        Text = Convert.ToString(dr["VendorName"]),
                        Value = Convert.ToString(dr["VendorId"])

                    });
                }

                conn.Close();
            }

            return items;
        }

        public static List<SelectListItem> PopulateServices()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            using (SqlConnection conn = new SqlConnection(DatabaseConnect))
            using (SqlCommand cmd = new SqlCommand("SELECT AllocationId, AllocationName FROM Allocations ORDER BY AllocationId", conn))
            {

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        items.Add(new SelectListItem
                        {
                            Text = Convert.ToString(dr["AllocationName"]),
                            Value = Convert.ToString(dr["AllocationId"])
                        });
                    }
                }
            }

            return items;
        }

        public static List<SelectListItem> PopulateUnits()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            using (SqlConnection conn = new SqlConnection(DatabaseConnect))
            using (SqlCommand cmd = new SqlCommand("SELECT UnitId, UnitName FROM BusinessUnits ORDER BY UnitId", conn))
            {

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        items.Add(new SelectListItem
                        {
                            Text = Convert.ToString(dr["UnitName"]),
                            Value = Convert.ToString(dr["UnitId"])
                        });
                    }
                }
            }

            return items;
        }

        public static List<SelectListItem> PopulateExpenses()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            using (SqlConnection conn = new SqlConnection(DatabaseConnect))
            using (SqlCommand cmd = new SqlCommand("SELECT ExpId, ExpenseName FROM ExpenseTypes ORDER BY ExpId", conn))
            {

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        items.Add(new SelectListItem
                        {
                            Text = Convert.ToString(dr["ExpenseName"]),
                            Value = Convert.ToString(dr["ExpId"])
                        });
                    }
                }
            }

            return items;
        }

        public static List<SelectListItem> PopulatePaymentTypes()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            using (SqlConnection conn = new SqlConnection(DatabaseConnect))
            using (SqlCommand cmd = new SqlCommand("SELECT PaymentTypeId, PaymentName FROM PaymentTypes ORDER BY PaymentTypeId", conn))
            {

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        items.Add(new SelectListItem
                        {
                            Text = Convert.ToString(dr["PaymentName"]),
                            Value = Convert.ToString(dr["PaymentTypeId"])
                        });
                    }
                }
            }

            return items;
        }

        public static List<SelectListItem> PopulateStatuses()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            using (SqlConnection conn = new SqlConnection(DatabaseConnect))
            using (SqlCommand cmd = new SqlCommand("SELECT Flag, FlagText FROM Lookup", conn))
            {

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        items.Add(new SelectListItem
                        {
                            Text = Convert.ToString(dr["FlagText"]),
                            Value = Convert.ToString(dr["Flag"])
                        });
                    }
                }
            }

            return items;
        }


        public static List<SelectListItem> PopulateExportTypes()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            //string[] exportFormats = Enum.GetNames(typeof(ExportFormatType));
            //SelectList list = new SelectList(exportFormats);
            using (SqlConnection conn = new SqlConnection(Helpers.Helpers.DatabaseConnect))
            using (SqlCommand cmd = new SqlCommand("SELECT FormatId, FormatName FROM FormatTypes ORDER BY FormatId", conn))
            {
                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        items.Add(new SelectListItem
                        {
                            Text = Convert.ToString(dr["FormatName"]),
                            Value = Convert.ToString(dr["FormatId"])
                        });
                    }
                }
            }

            return items;
            //items = exportFormats.Select((day, index) =>
            //{
            //    return new SelectListItem
            //    {
            //        Text = index.ToString(),
            //        Value = day
                    
            //    };
            //}).ToList();
          
        }

        public static List<SelectListItem> PopulateYears()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            using (SqlConnection conn = new SqlConnection(DatabaseConnect))
            using (SqlCommand cmd = new SqlCommand("SELECT YearId, Year FROM Years ORDER BY YearId", conn))
            {

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        items.Add(new SelectListItem
                        {
                            Text = Convert.ToString(dr["Year"]),
                            Value = Convert.ToString(dr["YearId"])
                        });
                    }
                }
            }

            return items;
        }
        public static List<SelectListItem> PopulateMonths()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            using (SqlConnection conn = new SqlConnection(DatabaseConnect))
            using (SqlCommand cmd = new SqlCommand("SELECT MonthId, Month FROM Months", conn))
            {

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        items.Add(new SelectListItem
                        {
                            Text = Convert.ToString(dr["Month"]),
                            Value = Convert.ToString(dr["MonthId"])
                        });
                    }
                }
            }

            return items;
        }

        public static List<SelectListItem> PopulateQuarters()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            using (SqlConnection conn = new SqlConnection(DatabaseConnect))
            using (SqlCommand cmd = new SqlCommand("SELECT QuarterId, Quarter FROM Quarters", conn))
            {

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        items.Add(new SelectListItem
                        {
                            Text = Convert.ToString(dr["Quarter"]),
                            Value = Convert.ToString(dr["QuarterId"])
                        });
                    }
                }
            }

            return items;
        }

        public ActionResult PurchasesByMonth()
        {
            Invoice invoice = new Invoice();
            invoice.Years = PopulateYears();
            invoice.Months = PopulateMonths();
            invoice.FileFormats = PopulateExportTypes();
            return View(invoice);
        }

        [HttpPost]
        public ActionResult PurchasesByMonth(FormCollection formcollection)
        {
            Invoice invoice = new Invoice();
            try
            {
                invoice.Years = PopulateYears();
                invoice.Months = PopulateMonths();
                invoice.FileFormats = PopulateExportTypes();
                invoice.YearId = Convert.ToInt32(formcollection["YearId"]);
                invoice.MonthId = Convert.ToInt32(formcollection["MonthId"]);
                invoice.FormatId = Convert.ToInt32(formcollection["FormatId"]);

                var YearName = formcollection["YearName"];
                var MonthName = formcollection["MonthName"];

                if (Convert.ToString(formcollection["YearId"]) != null && Convert.ToString(formcollection["MonthId"]) != null && Convert.ToString(formcollection["FormatId"]) != null)
                {
                   
                    //initialize session variables
                    Session["MonthId"] = invoice.MonthId;
                    Session["YearId"] = invoice.YearId;
                    var DropChoice = invoice.FormatId;
                    if (DropChoice == 4)
                    {
                        return RedirectToAction("PurchasesReportByMonthWord");
                    }
                    else if (DropChoice == 3)
                    {
                        return RedirectToAction("PurchasesReportByMonthExcel");
                    }
                    else
                    {
                        return RedirectToAction("PurchasesReportByMonth");
                    }
                   
                }
                else
                {
                    ViewBag.ErrorMessage = "Missing selection.Please select all parameters!";

                    return View();
                }
            }

            catch (Exception ex)
            {
                invoice.Years = PopulateYears();
                invoice.Months = PopulateMonths();
                invoice.FileFormats = PopulateExportTypes();
                ExceptionLogging.SendErrorToText(ex);
                ViewBag.ErrorMessage = "An error occurred during request processing";
                return View(invoice);
            }


        }

        public ActionResult ReportError()
        {
            
            return View();
        }


        public ActionResult PurchasesByQuarter()
        {
            Invoice invoice = new Invoice();
            invoice.Years = PopulateYears();
            invoice.AnnualQuarters = PopulateQuarters();
            invoice.FileFormats = PopulateExportTypes();
            return View(invoice);
        }

        [HttpPost]
        public ActionResult PurchasesByQuarter(FormCollection formcollection)
        {
            Invoice invoice = new Invoice();
            try

            {
                invoice.Years = PopulateYears();
                invoice.AnnualQuarters = PopulateQuarters();
                invoice.FileFormats = PopulateExportTypes();
                invoice.YearId = Convert.ToInt32(formcollection["YearId"]);
                invoice.QuarterId = Convert.ToInt32(formcollection["QuarterId"]);
                invoice.FormatId = Convert.ToInt32(formcollection["FormatId"]);

                //var YearName = formcollection["YearName"];
                //var QuarterName = formcollection["QuarterName"];

                if (Convert.ToString(formcollection["YearId"]) != null && Convert.ToString(formcollection["QuarterId"]) != null && Convert.ToString(formcollection["FormatId"]) != null)
                {
                    var selectedYear = invoice.AnnualQuarters.Find(p => p.Value == invoice.QuarterId.ToString());
                    var selectedQuarter = invoice.Years.Find(p => p.Value == invoice.YearId.ToString());
                    var DropChoice = invoice.FormatId;
                    //if (selectedYear != null && selectedQuarter != null)
                    //{
                    //    selectedYear.Selected = true;
                    //    selectedQuarter.Selected = true;
                    //    ViewBag.Message = selectedYear.Text;
                    //    ViewBag.Message = selectedQuarter.Text;
                    //}
                    //initialize session variables
                    Session["QuarterId"] = invoice.QuarterId;
                    Session["YearId"] = invoice.YearId;
                   
                        if (DropChoice == 4)
                        {
                            return RedirectToAction("PurchasesReportByQuarterWord");
                        }
                        else if (DropChoice == 3)
                        {
                            return RedirectToAction("PurchasesReportByQuarterExcel");
                        }
                        else
                        {
                        return RedirectToAction("PurchasesReportByQuarterPDF");
                        }

                    //}
                    // return RedirectToAction("PurchasesReportByQuarter");
                }
                else
                {
                    ViewBag.ErrorMessage = "Missing selection.Please select all parameters!";

                    return View();
                }
            }

            catch (Exception ex)
            {
                invoice.Years = PopulateYears();
                invoice.AnnualQuarters = PopulateQuarters();
                invoice.FileFormats = PopulateExportTypes();
                ExceptionLogging.SendErrorToText(ex);              
                return View(invoice);
            }
            

        }


        public ActionResult PurchasesByDate()
        {
            Invoice invoice = new Invoice();
            invoice.FileFormats = PopulateExportTypes();

            return View(invoice);
        }

        [HttpPost]
        public ActionResult PurchasesByDate(FormCollection formcollection)
        {
            Invoice invoice = new Invoice();
            invoice.FileFormats = PopulateExportTypes();

            try

            {
                
                if (Convert.ToString(formcollection["StartDate"]) != null && Convert.ToString(formcollection["EndDate"]) != null && Convert.ToString(formcollection["FormatId"]) != null)
                {

                    invoice.StartDate = Convert.ToDateTime(formcollection["StartDate"]);
                    invoice.EndDate = Convert.ToDateTime(formcollection["EndDate"]);
                    invoice.FormatId= Convert.ToInt32(formcollection["FormatId"]);
                    if (invoice.StartDate > invoice.EndDate)
                    {
                        ViewBag.ErrorMessage = Messages.INVALID_LEAVE_DATES_ENTRY;
                        return View(invoice);
                    }
                    //initialize session variables
                    var DropChoice = invoice.FormatId;
                    Session["StartDate"] = invoice.StartDate;
                    Session["EndDate"] = invoice.EndDate;

                    if (DropChoice == 4)
                    {
                        return RedirectToAction("PurchasesMasterWord");
                    }
                    else if (DropChoice == 3)
                    {
                        return RedirectToAction("PurchasesMasterExcel");
                    }
                    else
                    {
                        return RedirectToAction("PurchasesMasterByDate");
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Missing selection.Please select all parameters!";

                    return View();
                }

            }
            catch (Exception ex)
            {
                invoice.FileFormats = PopulateExportTypes();
                ExceptionLogging.SendErrorToText(ex);

                ViewBag.ErrorMessage = ex.Message;
                return View(invoice);
            }
           
        }


        [HttpGet]
        public ActionResult PurchasesReportByCriteria()
        {
            Invoice invoice = new Invoice();
            //string[] exportFormats = Enum.GetNames(typeof(ExportFormatType));
            //SelectList list = new SelectList(exportFormats);
            //ViewBag.mylist = list;

            invoice.Expenses = PopulateExpenses();
            invoice.Units = PopulateUnits();
            invoice.Currencies = PopulateCurrencies();
            invoice.Services = PopulateServices();
            invoice.Vendors = PopulateVendors();
           invoice.FileFormats = PopulateExportTypes();
            return View(invoice);
        }

        [HttpPost]
        public ActionResult PurchasesReportByCriteria(FormCollection formcollection)
        {
            
            Invoice invoice = new Invoice();
           // string[] exportFormats = Enum.GetNames(typeof(ExportFormatType));
           // SelectList list = new SelectList(exportFormats);
           // ViewBag.mylist = list;
            invoice.Expenses = PopulateExpenses();
            invoice.Units = PopulateUnits();
            invoice.Currencies = PopulateCurrencies();
            invoice.Services = PopulateServices();
            invoice.Vendors = PopulateVendors();
            invoice.FileFormats = PopulateExportTypes();
           // var selectedText = list.Where(x => x.Selected).FirstOrDefault().Text;

            try
            {              
                if ( Convert.ToString(formcollection["CurrencyId"]) != null && Convert.ToString(formcollection["VendorId"]) != null && Convert.ToString(formcollection["AllocationId"]) != null && Convert.ToString(formcollection["UnitId"]) != null && Convert.ToString(formcollection["ExpId"]) != null && Convert.ToString(formcollection["FormatId"]) != null)
                {

                    invoice.AllocationId = Convert.ToInt32(formcollection["AllocationId"]);
                    invoice.UnitId = Convert.ToInt32(formcollection["UnitId"]);
                    invoice.VendorId = Convert.ToInt32(formcollection["VendorId"]);
                    invoice.CurrencyId = Convert.ToInt32(formcollection["CurrencyId"]);                   
                    invoice.ExpId = Convert.ToInt32(formcollection["ExpId"]);
                    invoice.FormatId = Convert.ToInt32(formcollection["FormatId"]);

                    //var selectedFormat = list.SelectedValue.ToString();
                    // var selectedFormat = list.s.ToString();
                    //if (!String.IsNullOrEmpty(selectedFormat))
                    //{
                    //    var fileformat = selectedFormat;
                    //    invoice.format = fileformat;
                    //    Session["FormatId"] = fileformat;
                    //}
                    var DropChoice = invoice.FormatId;
                    //initialize session variables
                    Session["UnitId"] = invoice.UnitId;
                    Session["AllocationId"] = invoice.AllocationId;
                    Session["VendorId"] = invoice.VendorId;
                    Session["CurrencyId"] = invoice.CurrencyId;
                    Session["ExpId"] = invoice.ExpId;

                    if (DropChoice == 4)
                    {
                        return RedirectToAction("PurchasesByCriteriaWord");
                    }
                    else if (DropChoice == 3)
                    {
                        return RedirectToAction("PurchasesByCriteriaExcel");
                    }
                    else
                    {
                        return RedirectToAction("PurchasesByCriteria");
                    }

                   // return RedirectToAction("PurchasesByCriteria");
                   
                }
                else
                {
                    ViewBag.ErrorMessage = "Missing selection.Please select all parameters!";
                   
                    return View(invoice);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
               
                invoice.Currencies = PopulateCurrencies();
                invoice.Services = PopulateServices();
                invoice.Units = PopulateUnits();
                invoice.Vendors = PopulateVendors();
                invoice.Expenses = PopulateExpenses();
                invoice.FileFormats = PopulateExportTypes();
                // string[] exportFormatts = Enum.GetNames(typeof(ExportFormatType));
                // SelectList listt = new SelectList(exportFormatts);

                return View(invoice);
            }

        }

        [HttpGet]
        public ActionResult TasksByCriteria()
        {
            Invoice invoice = new Invoice();
            invoice.Statuses = PopulateStatuses();
            return View(invoice);
        }

        [HttpPost]
        public ActionResult TasksByCriteria(FormCollection formcollection)
        {
            
            Invoice invoice = new Invoice();
            invoice.Statuses = PopulateStatuses();
            try
            {              
                if (Convert.ToString(formcollection["Flag"]) != null )
                {
                    invoice.StatusId = Convert.ToInt32(formcollection["Flag"]);                
                    
                    //initialize session variables                  
                    Session["Flag"] = invoice.StatusId;                  
                    return RedirectToAction("TaskReport");

                }
                else
                {
                    ViewBag.ErrorMessage = "Missing selection.Please select all parameters!";
                    return View(invoice);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                invoice.Statuses = PopulateStatuses();            
                return View(invoice);
            }
        }


        // GET: Reports/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        //protected void Export(object sender, EventArgs e)
        //{
        //    ExportFormatType formatType = ExportFormatType.NoFormat;
        //    switch (rbFormat.SelectedItem.Value)
        //    {
        //        case "Word":
        //            formatType = ExportFormatType.WordForWindows;
        //            break;
        //        case "PDF":
        //            formatType = ExportFormatType.PortableDocFormat;
        //            break;
        //        case "Excel":
        //            formatType = ExportFormatType.Excel;
        //            break;
        //        case "CSV":
        //            formatType = ExportFormatType.CharacterSeparatedValues;
        //            break;
        //    }

        //    crystalReport.ExportToHttpResponse(formatType, Response, true, "Crystal");
        //    Response.End();
        //}
        // GET: Reports/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult TaskReport()
        {

            SqlConnection con = new SqlConnection(DatabaseConnect);
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT CompletedOn,AssignedTo,CreatedOn,FinishBy,CreatedBy,Reference,Priority,FlagText FROM vw_Tasks", con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                ViewBag.ErrorMessage = Messages.GENERAL_ERROR;
                return View();

            }
            var today = DateTime.Now.ToShortDateString();
            var TaskReport = "DailyTasks";
             string OutputFileName = TaskReport.ToString() + "_" + today + ".pdf";

            ReportClass rptH = new ReportClass();
            rptH.FileName = Server.MapPath(("~/Reports/") + "DailyTasks.rpt");

            rptH.Load();
            rptH.SetDataSource(dt);

         
             string filePath = Server.MapPath("~/TaskReports/");
            string destPath = Path.Combine(filePath, ToSafeFileName(OutputFileName));
            // if (formatId == parameters.Excel)

             rptH.ExportToDisk(ExportFormatType.PortableDocFormat, destPath);
            //  Stream stream = rptH.ExportToStream(ExportFormatType.ExcelWorkbook);
            //return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            // else

            // rptH.ExportToDisk(ExportFormatType.PortableDocFormat, destPath);
            Stream stream = rptH.ExportToStream(ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }
        [HttpGet]
        public ActionResult GenerateSingleMemo()
        {
            Invoice invoice = new Invoice();                
            return View(invoice);
        }

        [HttpPost]
        public ActionResult GenerateSingleMemo(FormCollection formcollection)
        {
            Invoice invoice = new Invoice();       
            try
            {     
                    invoice.Reference = Convert.ToString(formcollection["Reference"]);
                    return RedirectToAction("GenerateMemo");
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);

                return View(invoice);
            }

        }

        public ActionResult GenerateMemo()
        {

            SqlConnection con = new SqlConnection(DatabaseConnect);
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT Description,CurrencySymbol,ActualAmount,CurrencyAbbr FROM vw_PurchasesByCriteria WHERE Reference='" + Convert.ToString(Session["Reference"]) + "'", con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);

             
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                ViewBag.ErrorMessage = Messages.GENERAL_ERROR;
                return RedirectToAction("ReportError");

            }
            var today = DateTime.Now.ToShortDateString();
            var TaskReport = "Memo";
            string OutputFileName = TaskReport.ToString() + "_" + today + ".pdf";

            ReportClass rptH = new ReportClass();
            rptH.FileName = Server.MapPath(("~/Reports/") + "PurchaseMemo.rpt");

            rptH.Load();
            rptH.SetDataSource(dt);


            string filePath = Server.MapPath("~/TaskReports/");
            string destPath = Path.Combine(filePath, ToSafeFileName(OutputFileName));
           
            rptH.ExportToDisk(ExportFormatType.PortableDocFormat, destPath);          
            Stream stream = rptH.ExportToStream(ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }

        public ActionResult GenerateWordMemo()
        {

            SqlConnection con = new SqlConnection(DatabaseConnect);
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT Description,CurrencySymbol,ActualAmount,CurrencyAbbr FROM vw_PurchasesByCriteria WHERE Reference='" + Convert.ToString(Session["Reference"]) + "'", con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                ViewBag.ErrorMessage = Messages.GENERAL_ERROR;
                return RedirectToAction("ReportError");
            }
            var today = DateTime.Now.ToShortDateString();
            var TaskReport = "Memo";
            string OutputFileName = TaskReport.ToString() + "_" + today + ".pdf";

            ReportClass rptH = new ReportClass();
            rptH.FileName = Server.MapPath(("~/Reports/") + "PurchaseMemo.rpt");

            rptH.Load();
            rptH.SetDataSource(dt);


            string filePath = Server.MapPath("~/TaskReports/");
            string destPath = Path.Combine(filePath, ToSafeFileName(OutputFileName));
            
            rptH.ExportToDisk(ExportFormatType.PortableDocFormat, destPath);
          
            Stream stream = rptH.ExportToStream(ExportFormatType.WordForWindows);
            return File(stream, " application/msword");
        }

        public ActionResult PurchasesByCriteria()
        {
           
            SqlConnection con = new SqlConnection(DatabaseConnect);
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT AllocationName,ExpenseName,UnitName,VendorName,ActualAmount,Reference,CreatedOn,CurrencyAbbr,Discount FROM vw_PurchasesByCriteria WHERE AllocationId='" + Convert.ToInt32(Session["AllocationId"]) + "' AND CurrencyId='" + Convert.ToInt32(Session["CurrencyId"]) + "' AND VendorId='" + Convert.ToInt32(Session["VendorId"]) + "' AND ExpId='" + Convert.ToInt32(Session["ExpId"]) + "' AND UnitId='" + Convert.ToInt32(Session["UnitId"]) + "'", con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                ViewBag.ErrorMessage = Messages.GENERAL_ERROR;
                return RedirectToAction("ReportError");
            }

            var BatchId = "PurchaseByCriteria";
            var today = DateTime.Now.ToShortDateString();
            string ExcelFile = BatchId.ToString() + "_" + today + ".xlsx";
            string ExcelRecordFile = BatchId.ToString() + "_" + today + ".xls";
            string ExcelHtml32File = BatchId.ToString() + "_" + today + ".html";
            string ExcelHtml40File = BatchId.ToString() + "_" + today + ".html";
            string ExcelNoFile = BatchId.ToString() + "_" + today + ".xlsx";
            string ExcelWordFile = BatchId.ToString() + "_" + today + ".docx";
            string ExcelRichTextFile = BatchId.ToString() + "_" + today + ".rtf";
            string ExcelPdfFile = BatchId.ToString() + "_" + today + ".pdf";
            string filePath = Server.MapPath("~/BatchOut/");
           // string destPath = Path.Combine(filePath, ToSafeFileName(OutputFileName));

            ReportClass rptH = new ReportClass();
            rptH.FileName = Server.MapPath(("~/Reports/") + "PurchasesByCriteria.rpt");

            rptH.Load();
            rptH.SetDataSource(dt);

            //ExportFormatType efileType = (ExportFormatType)Enum.Parse(typeof(ExportFormatType), Convert.ToString(Session["FormatId"]));
            //switch (efileType)
            //{
            //    case ExportFormatType.Excel:
            //        rptH.ExportToDisk(efileType, Path.Combine(filePath, ToSafeFileName(ExcelFile)));
            //        break;
            //    case ExportFormatType.ExcelRecord:
            //        rptH.ExportToDisk(efileType, Path.Combine(filePath, ToSafeFileName(ExcelRecordFile)));
            //        break;
            //    case ExportFormatType.HTML32:
            //        rptH.ExportToDisk(efileType, Path.Combine(filePath, ToSafeFileName(ExcelHtml32File)));
            //        break;
            //    case ExportFormatType.HTML40:
            //        rptH.ExportToDisk(efileType, Path.Combine(filePath, ToSafeFileName(ExcelHtml40File)));
            //        break;
            //    case ExportFormatType.NoFormat:
            //        rptH.ExportToDisk(efileType, Path.Combine(filePath, ToSafeFileName(ExcelNoFile)));
            //        break;
            //    case ExportFormatType.PortableDocFormat:
            //        rptH.ExportToDisk(efileType, Path.Combine(filePath, ToSafeFileName(ExcelPdfFile)));
            //        break;
            //    case ExportFormatType.RichText:
            //        rptH.ExportToDisk(efileType, Path.Combine(filePath, ToSafeFileName(ExcelRichTextFile)));
            //        break;
            //    case ExportFormatType.WordForWindows:
            //        rptH.ExportToDisk(efileType, Path.Combine(filePath, ToSafeFileName(ExcelWordFile)));
            //        break;
            //}
       
               Stream stream = rptH.ExportToStream(ExportFormatType.PortableDocFormat);
               return File(stream, "application/pdf");
     
           
        }
        public ActionResult PurchasesByCriteriaWord()
        {

            SqlConnection con = new SqlConnection(DatabaseConnect);
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT AllocationName,ExpenseName,UnitName,VendorName,ActualAmount,Reference,CreatedOn,CurrencyAbbr,Discount FROM vw_PurchasesByCriteria WHERE AllocationId='" + Convert.ToInt32(Session["AllocationId"]) + "' AND CurrencyId='" + Convert.ToInt32(Session["CurrencyId"]) + "' AND VendorId='" + Convert.ToInt32(Session["VendorId"]) + "' AND ExpId='" + Convert.ToInt32(Session["ExpId"]) + "' AND UnitId='" + Convert.ToInt32(Session["UnitId"]) + "'", con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                ViewBag.ErrorMessage = Messages.GENERAL_ERROR;
                return RedirectToAction("ReportError");
            }

            var BatchId = "PurchaseByCriteria";
            var today = DateTime.Now.ToShortDateString();
            string ExcelFile = BatchId.ToString() + "_" + today + ".xlsx";
            string ExcelRecordFile = BatchId.ToString() + "_" + today + ".xls";
            string ExcelHtml32File = BatchId.ToString() + "_" + today + ".html";
            string ExcelHtml40File = BatchId.ToString() + "_" + today + ".html";
            string ExcelNoFile = BatchId.ToString() + "_" + today + ".xlsx";
            string ExcelWordFile = BatchId.ToString() + "_" + today + ".docx";
            string ExcelRichTextFile = BatchId.ToString() + "_" + today + ".rtf";
            string ExcelPdfFile = BatchId.ToString() + "_" + today + ".pdf";
            string filePath = Server.MapPath("~/BatchOut/");
            // string destPath = Path.Combine(filePath, ToSafeFileName(OutputFileName));

            ReportClass rptH = new ReportClass();
            rptH.FileName = Server.MapPath(("~/Reports/") + "PurchasesByCriteria.rpt");

            rptH.Load();
            rptH.SetDataSource(dt);

            Stream stream = rptH.ExportToStream(ExportFormatType.WordForWindows);
            return File(stream, "application/msword");


        }
        public ActionResult PurchasesByCriteriaExcel()
        {

            SqlConnection con = new SqlConnection(DatabaseConnect);
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT AllocationName,ExpenseName,UnitName,VendorName,ActualAmount,Reference,CreatedOn,CurrencyAbbr,Discount FROM vw_PurchasesByCriteria WHERE AllocationId='" + Convert.ToInt32(Session["AllocationId"]) + "' AND CurrencyId='" + Convert.ToInt32(Session["CurrencyId"]) + "' AND VendorId='" + Convert.ToInt32(Session["VendorId"]) + "' AND ExpId='" + Convert.ToInt32(Session["ExpId"]) + "' AND UnitId='" + Convert.ToInt32(Session["UnitId"]) + "'", con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                ViewBag.ErrorMessage = Messages.GENERAL_ERROR;
                return RedirectToAction("ReportError");
            }

            var BatchId = "PurchaseByCriteria";
            var today = DateTime.Now.ToShortDateString();
            string ExcelFile = BatchId.ToString() + "_" + today + ".xlsx";
            string ExcelRecordFile = BatchId.ToString() + "_" + today + ".xls";
            string ExcelHtml32File = BatchId.ToString() + "_" + today + ".html";
            string ExcelHtml40File = BatchId.ToString() + "_" + today + ".html";
            string ExcelNoFile = BatchId.ToString() + "_" + today + ".xlsx";
            string ExcelWordFile = BatchId.ToString() + "_" + today + ".docx";
            string ExcelRichTextFile = BatchId.ToString() + "_" + today + ".rtf";
            string ExcelPdfFile = BatchId.ToString() + "_" + today + ".pdf";
            string filePath = Server.MapPath("~/BatchOut/");
            // string destPath = Path.Combine(filePath, ToSafeFileName(OutputFileName));

            ReportClass rptH = new ReportClass();
            rptH.FileName = Server.MapPath(("~/Reports/") + "PurchasesByCriteria.rpt");

            rptH.Load();
            rptH.SetDataSource(dt);

            Stream stream = rptH.ExportToStream(ExportFormatType.ExcelWorkbook);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");


        }
        public ActionResult PurchasesReportByQuarterPDF()
        {

            SqlConnection con = new SqlConnection(DatabaseConnect);
            DataTable dt = new DataTable();
            try
            {
              
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT Year,Quarter,AllocationName,ExpenseName,UnitName,VendorName,ActualAmount,Reference,CreatedOn,CurrencyAbbr,Discount FROM vw_PaymentsQuarterlyData WHERE YearId='" + Convert.ToInt32(Session["YearId"]) + "' AND QuarterId='" + Convert.ToInt32(Session["QuarterId"]) + "'", con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                ViewBag.ErrorMessage = Messages.GENERAL_ERROR;
                return RedirectToAction("ReportError");
               // return View();
            }

         
            var today = DateTime.Now.ToShortDateString();
           
           
            ReportClass rptH = new ReportClass();
            rptH.FileName = Server.MapPath(("~/Reports/") + "PurchasesByQuarter.rpt");

            rptH.Load();
            rptH.SetDataSource(dt);

            Stream stream = rptH.ExportToStream(ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }

        public ActionResult PurchasesReportByQuarterExcel()
        {

            SqlConnection con = new SqlConnection(DatabaseConnect);
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT Year,Quarter,AllocationName,ExpenseName,UnitName,VendorName,ActualAmount,Reference,CreatedOn,CurrencyAbbr,Discount FROM vw_PaymentsQuarterlyData WHERE YearId='" + Convert.ToInt32(Session["YearId"]) + "' AND QuarterId='" + Convert.ToInt32(Session["QuarterId"]) + "'", con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                ViewBag.ErrorMessage = Messages.GENERAL_ERROR;
                return RedirectToAction("ReportError");
                
            }
            var today = DateTime.Now.ToShortDateString();
            ReportClass rptH = new ReportClass();
            rptH.FileName = Server.MapPath(("~/Reports/") + "PurchasesByQuarter.rpt");

            rptH.Load();
            rptH.SetDataSource(dt);

            Stream stream = rptH.ExportToStream(ExportFormatType.ExcelWorkbook);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public ActionResult PurchasesReportByQuarterWord()
        {

            SqlConnection con = new SqlConnection(DatabaseConnect);
            DataTable dt = new DataTable();
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT Year,Quarter,AllocationName,ExpenseName,UnitName,VendorName,ActualAmount,Reference,CreatedOn,CurrencyAbbr,Discount FROM vw_PaymentsQuarterlyData WHERE YearId='" + Convert.ToInt32(Session["YearId"]) + "' AND QuarterId='" + Convert.ToInt32(Session["QuarterId"]) + "'", con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                ViewBag.ErrorMessage = Messages.GENERAL_ERROR;
                 return RedirectToAction("ReportError");

            }


            var today = DateTime.Now.ToShortDateString();


            ReportClass rptH = new ReportClass();
            rptH.FileName = Server.MapPath(("~/Reports/") + "PurchasesByQuarter.rpt");

            rptH.Load();
            rptH.SetDataSource(dt);

            Stream stream = rptH.ExportToStream(ExportFormatType.WordForWindows);
            //.docx application/ vnd.openxmlformats - officedocument.wordprocessingml.document
            return File(stream, "application/msword");
        }
        public ActionResult PurchasesReportByMonth()
        {

            SqlConnection con = new SqlConnection(DatabaseConnect);
            DataTable dt = new DataTable();
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT AllocationName,ExpenseName,UnitName,VendorName,ActualAmount,Reference,CreatedOn,CurrencyAbbr,Discount FROM vw_PaymentsQuarterlyData WHERE YearId='" + Convert.ToInt32(Session["YearId"]) + "' AND MonthId='" + Convert.ToInt32(Session["MonthId"]) + "'", con);

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                ViewBag.ErrorMessage = Messages.GENERAL_ERROR;
                return RedirectToAction("ReportError");
            }

            var BatchId = "PurchaseByCriteria";
            var today = DateTime.Now.ToShortDateString();
            ReportClass rptH = new ReportClass();
            rptH.FileName = Server.MapPath(("~/Reports/") + "PurchasesByMonth.rpt");

            rptH.Load();
            rptH.SetDataSource(dt);

            Stream stream = rptH.ExportToStream(ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }
        public ActionResult PurchasesReportByMonthWord()
        {

            SqlConnection con = new SqlConnection(DatabaseConnect);
            DataTable dt = new DataTable();
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT AllocationName,ExpenseName,UnitName,VendorName,ActualAmount,Reference,CreatedOn,CurrencyAbbr,Discount FROM vw_PaymentsQuarterlyData WHERE YearId='" + Convert.ToInt32(Session["YearId"]) + "' AND MonthId='" + Convert.ToInt32(Session["MonthId"]) + "'", con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                ViewBag.ErrorMessage = Messages.GENERAL_ERROR;
                return RedirectToAction("ReportError");
            }

            var BatchId = "PurchaseByCriteria";
            var today = DateTime.Now.ToShortDateString();
            ReportClass rptH = new ReportClass();
            rptH.FileName = Server.MapPath(("~/Reports/") + "PurchasesByMonth.rpt");

            rptH.Load();
            rptH.SetDataSource(dt);

            Stream stream = rptH.ExportToStream(ExportFormatType.WordForWindows);
            return File(stream, "application/msword");
        }
        public ActionResult PurchasesReportByMonthExcel()
        {

            SqlConnection con = new SqlConnection(DatabaseConnect);
            DataTable dt = new DataTable();
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT AllocationName,ExpenseName,UnitName,VendorName,ActualAmount,Reference,CreatedOn,CurrencyAbbr,Discount FROM vw_PaymentsQuarterlyData WHERE YearId='" + Convert.ToInt32(Session["YearId"]) + "' AND MonthId='" + Convert.ToInt32(Session["MonthId"]) + "'", con);

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                ViewBag.ErrorMessage = Messages.GENERAL_ERROR;
                return RedirectToAction("ReportError");
            }

            var BatchId = "PurchaseByCriteria";
            var today = DateTime.Now.ToShortDateString();
            ReportClass rptH = new ReportClass();
            rptH.FileName = Server.MapPath(("~/Reports/") + "PurchasesByMonth.rpt");

            rptH.Load();
            rptH.SetDataSource(dt);

            Stream stream = rptH.ExportToStream(ExportFormatType.ExcelWorkbook);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
        public ActionResult PurchasesMaster()
        {
           
            SqlConnection con = new SqlConnection(DatabaseConnect);
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT AllocationName,ExpenseName,UnitName,VendorName,ActualAmount,Reference,CreatedOn,CurrencyAbbr,Discount FROM vw_PurchasesByCriteria ", con);
               // SqlCommand cmd = new SqlCommand("SELECT AllocationName,ExpenseName,UnitName,VendorName,ActualAmount,Reference,CreatedOn,CurrencyAbbr,Discount FROM vw_PurchasesByCriteria where CreatedOn BETWEEN Convert(DateTime,'" + Session["StartDate"] + "',101) AND Convert(DateTime,'" + Session["EndDate"] + "',101) ", con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                ViewBag.ErrorMessage = Messages.GENERAL_ERROR;
                return RedirectToAction("ReportError");
            }

            ReportClass rptH = new ReportClass();
            rptH.FileName = Server.MapPath(("~/Reports/") + "PurchasesMaster.rpt");

            rptH.Load();
            rptH.SetDataSource(dt);
           
            Stream stream = rptH.ExportToStream(ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }

        public ActionResult PurchasesMasterByDate()
        {

            SqlConnection con = new SqlConnection(DatabaseConnect);
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                //SqlCommand cmd = new SqlCommand("SELECT AllocationName,ExpenseName,UnitName,VendorName,ActualAmount,Reference,CreatedOn,CurrencyAbbr,Discount FROM vw_PurchasesByCriteria ", con);
                 SqlCommand cmd = new SqlCommand("SELECT AllocationName,ExpenseName,UnitName,VendorName,ActualAmount,Reference,CreatedOn,CurrencyAbbr,Discount FROM vw_PurchasesByCriteria where CreatedOn BETWEEN Convert(DateTime,'" + Session["StartDate"] + "',101) AND Convert(DateTime,'" + Session["EndDate"] + "',101) ", con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                ViewBag.ErrorMessage = Messages.GENERAL_ERROR;
                return RedirectToAction("ReportError");
            }

            ReportClass rptH = new ReportClass();
            rptH.FileName = Server.MapPath(("~/Reports/") + "PurchasesMaster.rpt");

            rptH.Load();
            rptH.SetDataSource(dt);

            Stream stream = rptH.ExportToStream(ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }

        public ActionResult PurchasesMasterWord()
        {

            SqlConnection con = new SqlConnection(DatabaseConnect);
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                // SqlCommand cmd = new SqlCommand("SELECT AllocationName,ExpenseName,UnitName,VendorName,ActualAmount,Reference,CreatedOn,CurrencyAbbr,Discount FROM vw_PurchasesByCriteria ", con);
                SqlCommand cmd = new SqlCommand("SELECT AllocationName,ExpenseName,UnitName,VendorName,ActualAmount,Reference,CreatedOn,CurrencyAbbr,Discount FROM vw_PurchasesByCriteria where CreatedOn BETWEEN Convert(DateTime,'" + Session["StartDate"] + "',101) AND Convert(DateTime,'" + Session["EndDate"] + "',101) ", con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                ViewBag.ErrorMessage = Messages.GENERAL_ERROR;
                return RedirectToAction("ReportError");
            }

            ReportClass rptH = new ReportClass();
            rptH.FileName = Server.MapPath(("~/Reports/") + "PurchasesMaster.rpt");

            rptH.Load();
            rptH.SetDataSource(dt);

            Stream stream = rptH.ExportToStream(ExportFormatType.WordForWindows);
            return File(stream, "application/msword");
        }


        public ActionResult PurchasesMasterExcel()
        {

            SqlConnection con = new SqlConnection(DatabaseConnect);
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                // SqlCommand cmd = new SqlCommand("SELECT AllocationName,ExpenseName,UnitName,VendorName,ActualAmount,Reference,CreatedOn,CurrencyAbbr,Discount FROM vw_PurchasesByCriteria ", con);
                SqlCommand cmd = new SqlCommand("SELECT AllocationName,ExpenseName,UnitName,VendorName,ActualAmount,Reference,CreatedOn,CurrencyAbbr,Discount FROM vw_PurchasesByCriteria where CreatedOn BETWEEN Convert(DateTime,'" + Session["StartDate"] + "',101) AND Convert(DateTime,'" + Session["EndDate"] + "',101) ", con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                ViewBag.ErrorMessage = Messages.GENERAL_ERROR;
                return RedirectToAction("ReportError");
            }

            ReportClass rptH = new ReportClass();
            rptH.FileName = Server.MapPath(("~/Reports/") + "PurchasesMaster.rpt");

            rptH.Load();
            rptH.SetDataSource(dt);

            Stream stream = rptH.ExportToStream(ExportFormatType.WordForWindows);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
        // GET: Reports/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Reports/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Reports/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Reports/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
