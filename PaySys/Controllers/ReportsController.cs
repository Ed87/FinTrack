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

            using (SqlConnection conn = new SqlConnection(Helpers.Helpers.DatabaseConnect))
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

            using (SqlConnection conn = new SqlConnection(Helpers.Helpers.DatabaseConnect))
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

            using (SqlConnection conn = new SqlConnection(Helpers.Helpers.DatabaseConnect))
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

            using (SqlConnection conn = new SqlConnection(Helpers.Helpers.DatabaseConnect))
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

            using (SqlConnection conn = new SqlConnection(Helpers.Helpers.DatabaseConnect))
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

            using (SqlConnection conn = new SqlConnection(Helpers.Helpers.DatabaseConnect))
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

            using (SqlConnection conn = new SqlConnection(Helpers.Helpers.DatabaseConnect))
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
            string[] exportFormats = Enum.GetNames(typeof(CrystalDecisions.Shared.ExportFormatType));
            //using (SqlConnection conn = new SqlConnection(Helpers.Helpers.DatabaseConnect))
            //using (SqlCommand cmd = new SqlCommand("SELECT FormatId, FormatName FROM FormatTypes ORDER BY FormatId", conn))
            //{

            //    conn.Open();

            //    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            //    {
            //        while (dr.Read())
            //        {
            //            items.Add(new SelectListItem
            //            {
            //                Text = Convert.ToString(dr["FormatName"]),
            //                Value = Convert.ToString(dr["FormatId"])
            //            });
            //        }
            //    }
            //}

            //return items;
            items = exportFormats.Select((day, index) =>
            {
                return new SelectListItem
                {
                    Text = index.ToString(),
                    Value = day
                    
                };
            }).ToList();
           //ViewBag.FormatTypes = new SelectList(items, "Text", "Value");
            return items;
        }
        [HttpGet]
        public ActionResult PurchasesReportByCriteria()
        {
            Invoice invoice = new Invoice();

            string[] exportFormats = Enum.GetNames(typeof(CrystalDecisions.Shared.ExportFormatType));
            SelectList list = new SelectList(exportFormats);
            ViewBag.mylist = list;

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
            string[] exportFormats = Enum.GetNames(typeof(CrystalDecisions.Shared.ExportFormatType));
            SelectList list = new SelectList(exportFormats);
            ViewBag.mylist = list;
            Invoice invoice = new Invoice();
            invoice.Expenses = PopulateExpenses();
            invoice.Units = PopulateUnits();
            invoice.Currencies = PopulateCurrencies();
            invoice.Services = PopulateServices();
            invoice.Vendors = PopulateVendors();
            invoice.FileFormats = PopulateExportTypes();
            try
            {              
                if ( Convert.ToString(formcollection["CurrencyId"]) != null && Convert.ToString(formcollection["VendorId"]) != null && Convert.ToString(formcollection["AllocationId"]) != null && Convert.ToString(formcollection["UnitId"]) != null && Convert.ToString(formcollection["ExpId"]) != null)
                {

                    invoice.AllocationId = Convert.ToInt32(formcollection["AllocationId"]);
                    invoice.UnitId = Convert.ToInt32(formcollection["UnitId"]);
                    invoice.VendorId = Convert.ToInt32(formcollection["VendorId"]);
                    invoice.CurrencyId = Convert.ToInt32(formcollection["CurrencyId"]);                   
                    invoice.ExpId = Convert.ToInt32(formcollection["ExpId"]);                 
                    string selectedText = list.Where(x => x.Selected).FirstOrDefault().Text;

                    //initialize session variables
                    Session["UnitId"] = invoice.UnitId;
                    Session["AllocationId"] = invoice.AllocationId;
                    Session["VendorId"] = invoice.VendorId;
                    Session["CurrencyId"] = invoice.CurrencyId;
                    Session["ExpId"] = invoice.ExpId;
                    Session["FormatId"] = selectedText;
                 
                    // ViewBag.SuccessMessage = Messages.OPERATION_COMPLETED_SUCCESSFULLY;
                    return RedirectToAction("PurchasesByCriteria");
                   
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
               // invoice.PaymentTypes = PopulatePaymentTypes();
                invoice.FileFormats = PopulateExportTypes();
                string[] exportFormatts = Enum.GetNames(typeof(CrystalDecisions.Shared.ExportFormatType));
                //SelectList list = new SelectList(exportFormats);
                ViewBag.mylist = list;

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

        // GET: Reports/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult TaskReport()
        {

            SqlConnection con = new SqlConnection(Helpers.Helpers.DatabaseConnect);
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
            string destPath = Path.Combine(filePath, Helpers.Helpers.ToSafeFileName(OutputFileName));
            // if (formatId == parameters.Excel)

             rptH.ExportToDisk(ExportFormatType.PortableDocFormat, destPath);
            //  Stream stream = rptH.ExportToStream(ExportFormatType.ExcelWorkbook);
            //return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            // else

            // rptH.ExportToDisk(ExportFormatType.PortableDocFormat, destPath);
            Stream stream = rptH.ExportToStream(ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }

        public ActionResult GenerateMemo()
        {

            SqlConnection con = new SqlConnection(Helpers.Helpers.DatabaseConnect);
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
                return View();

            }
            var today = DateTime.Now.ToShortDateString();
            var TaskReport = "Memo";
            string OutputFileName = TaskReport.ToString() + "_" + today + ".pdf";

            ReportClass rptH = new ReportClass();
            rptH.FileName = Server.MapPath(("~/Reports/") + "PurchaseMemo.rpt");

            rptH.Load();
            rptH.SetDataSource(dt);


            string filePath = Server.MapPath("~/TaskReports/");
            string destPath = Path.Combine(filePath, Helpers.Helpers.ToSafeFileName(OutputFileName));
            // if (formatId == parameters.Excel)

            rptH.ExportToDisk(ExportFormatType.PortableDocFormat, destPath);
            //  Stream stream = rptH.ExportToStream(ExportFormatType.ExcelWorkbook);
            //return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            // else

            // rptH.ExportToDisk(ExportFormatType.PortableDocFormat, destPath);
            Stream stream = rptH.ExportToStream(ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }


        public ActionResult PurchasesByCriteria()
        {
          
            // var unitId = System.Web.HttpContext.Current.Session["ReportName"].ToString();    // Setting ReportName
            //        string strFromDate = System.Web.HttpContext.Current.Session["rptFromDate"].ToString();     // Setting FromDate 
            //        string strToDate = System.Web.HttpContext.Current.Session["rptToDate"].ToString();         // Setting ToDate    
            //        var rptSource = System.Web.HttpContext.Current.Session["rptSource"];

            //string filetype = Convert.ToString(Session["FormatId"]); ;

            //Rename Processed Batch 
            // string newPath = AddSuffix(sourceFile, String.Format("({0})", parameters.IsProcessed));
            //FileInfo fi = new FileInfo(sourceFile);
            //if (fi.Exists)
            //{

            //    fi.MoveTo(newPath);
            //}

            SqlConnection con = new SqlConnection(Helpers.Helpers.DatabaseConnect);
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
                return View();
               
            }

           
           // string OutputFileName = BatchId + "_" + monthend + ".xlsx";

            ReportClass rptH = new ReportClass();
            rptH.FileName = Server.MapPath(("~/Reports/") + "PurchasesByCriteria.rpt");

            rptH.Load();
            rptH.SetDataSource(dt);

            ExportFormatType efileType = (CrystalDecisions.Shared.ExportFormatType)Enum.Parse(typeof(CrystalDecisions.Shared.ExportFormatType), Convert.ToString(Session["FormatId"]));
            switch (efileType)
            {
                case CrystalDecisions.Shared.ExportFormatType.Excel:
                    rptH.ExportToDisk(efileType, "reportExcel.xls");
                    break;
                case CrystalDecisions.Shared.ExportFormatType.ExcelRecord:
                    rptH.ExportToDisk(efileType, "reportExcel.xls");
                    break;
                case CrystalDecisions.Shared.ExportFormatType.HTML32:
                    rptH.ExportToDisk(efileType, "reporthtml.html");
                    break;
                case CrystalDecisions.Shared.ExportFormatType.HTML40:
                    rptH.ExportToDisk(efileType, "reporthtml.html");
                    break;
                case CrystalDecisions.Shared.ExportFormatType.NoFormat:
                    rptH.ExportToDisk(efileType, "reportExcel.xls");
                    break;
                case CrystalDecisions.Shared.ExportFormatType.PortableDocFormat:
                    rptH.ExportToDisk(efileType, "reportpdf.pdf");
                    break;
                case CrystalDecisions.Shared.ExportFormatType.RichText:
                    rptH.ExportToDisk(efileType, "reportrtf.rtf");
                    break;
                case CrystalDecisions.Shared.ExportFormatType.WordForWindows:
                    rptH.ExportToDisk(efileType, "reportdoc.doc");
                    break;
            }
            // string filePath = Server.MapPath("~/BatchOut/");
            // string destPath = Path.Combine(filePath, ToSafeFileName(OutputFileName));
            // if (formatId == parameters.Excel)

            // rptH.ExportToDisk(ExportFormatType.ExcelWorkbook, destPath);
            //  Stream stream = rptH.ExportToStream(ExportFormatType.ExcelWorkbook);
            //return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            // else

            // rptH.ExportToDisk(ExportFormatType.PortableDocFormat, destPath);
               Stream stream = rptH.ExportToStream(ExportFormatType.PortableDocFormat);
               return File(stream, "application/pdf");
            
            //else if (formatId == parameters.Xml)
            //{
            //    Stream stream = rptH.ExportToStream(ExportFormatType.Xml);
            //    return File(stream, "application/xml");
            //}

            //else if (formatId == parameters.Html)
            //{
            //    Stream streamm = rptH.ExportToStream(ExportFormatType.HTML40);
            //    return File(streamm, "application/html");
            //}
            //else 
            //    Stream stream = rptH.ExportToStream(ExportFormatType.CharacterSeparatedValues);
            //    return File(stream, "application/csv");
  
            
           
        }

        public ActionResult PurchasesMaster()
        {
           
            // var unitId = System.Web.HttpContext.Current.Session["ReportName"].ToString();    // Setting ReportName
            //        string strFromDate = System.Web.HttpContext.Current.Session["rptFromDate"].ToString();     // Setting FromDate 
            //        string strToDate = System.Web.HttpContext.Current.Session["rptToDate"].ToString();         // Setting ToDate    
            //        var rptSource = System.Web.HttpContext.Current.Session["rptSource"];

            //Rename Processed Batch 
            // string newPath = AddSuffix(sourceFile, String.Format("({0})", parameters.IsProcessed));


            //FileInfo fi = new FileInfo(sourceFile);
            //if (fi.Exists)
            //{

            //    fi.MoveTo(newPath);
            //}

            SqlConnection con = new SqlConnection(Helpers.Helpers.DatabaseConnect);
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT AllocationName,ExpenseName,UnitName,VendorName,ActualAmount,Reference,CreatedOn,CurrencyAbbr,Discount FROM vw_PurchasesByCriteria ", con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                ViewBag.ErrorMessage = Messages.GENERAL_ERROR;
                return View();
                // ex.Message.ToString();
            }


            // string OutputFileName = BatchId + "_" + monthend + ".xlsx";

            ReportClass rptH = new ReportClass();
            rptH.FileName = Server.MapPath(("~/Reports/") + "PurchasesByCriteria.rpt");

            rptH.Load();
            rptH.SetDataSource(dt);
            // string filePath = Server.MapPath("~/BatchOut/");
            // string destPath = Path.Combine(filePath, ToSafeFileName(OutputFileName));
            // if (formatId == parameters.Excel)

            // rptH.ExportToDisk(ExportFormatType.ExcelWorkbook, destPath);
            //  Stream stream = rptH.ExportToStream(ExportFormatType.ExcelWorkbook);
            //return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            // else

            // rptH.ExportToDisk(ExportFormatType.PortableDocFormat, destPath);
            Stream stream = rptH.ExportToStream(ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");


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
