using PaySys.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PaySys.Helpers;
using System.IO;
using static PaySys.Helpers.Helpers;
using PaySys.Exceptions;

namespace PaySys.Controllers
{
    public class InvoicingController : Controller
    {
        // GET: Invoicing
        public ActionResult Index()
        {
            List<Invoice> parameters = new List<Invoice>();
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConnect))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Invoices ORDER BY InvoiceId", conn);

                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Invoice parameter = new Invoice();
                        parameter.InvoiceId= Convert.ToInt32(dr["InvoiceId"]);
                        parameter.AllocationId= Convert.ToInt32(dr["AllocationId"]);
                        parameter.Reference = Convert.ToString(dr["Reference"]);
                        parameter.QuotedAmount= Convert.ToDouble(dr["QuotedAmount"]);
                        parameter.CreatedOn= Convert.ToDateTime(dr["CreatedOn"]);
                        parameter.StatusId= Convert.ToInt32(dr["StatusId"]);

                        using (SqlConnection conn1 = new SqlConnection(DatabaseConnect))
                        {

                            SqlCommand cmd1 = new SqlCommand("SELECT FlagText FROM LookUp WHERE Flag=" + Convert.ToInt32(parameter.StatusId) + "", conn1);

                            conn1.Open();
                            SqlDataReader dr1 = cmd1.ExecuteReader();
                            if (dr1.Read() == true)
                            {
                              parameter.Status = Convert.ToString(dr1["FlagText"]);
                            }

                            conn1.Close();
                        }
                        using (SqlConnection conn1 = new SqlConnection(DatabaseConnect))
                        {

                            SqlCommand cmd1 = new SqlCommand("SELECT AllocationName FROM Allocations WHERE AllocationId= '" + Convert.ToInt32(parameter.AllocationId) + "'", conn1);
                            conn1.Open();
                            SqlDataReader dr1 = cmd1.ExecuteReader();
                            if (dr1.Read())
                            {
                                parameter.Allocation = Convert.ToString(dr1["AllocationName"]);
                            }

                            conn1.Close();
                        }

                        parameters.Add(parameter);
                    }
                }
            }
            catch (Exception ex)
            {

                ExceptionLogging.SendErrorToText(ex);
                ViewBag.ErrorMessage = Messages.GENERAL_ERROR;
                return View();
            }
            return View(parameters);
        }



        public ActionResult AddVendor()
        {
            Invoice invoice = new Invoice();

            return View(invoice);
        }

        [HttpPost]
        public ActionResult AddVendor(Models.Invoice invoice)
        {
            //Invoice invoice = new Invoice();
            // invoice.Active = Convert.ToInt32(formcollection["Active"]);
            // invoice.VendorName = Convert.ToString(formcollection["VendorName"]);
            //if (!ModelState.IsValid)
            //{
            //    //bank.Banks = PopulateBank();
            //    //bank.SortCodes = PopulateSortCode();
            //    //bank.AccountTypes = PopulateAccountType();
            //    return View(invoice);
            //}
            try
            {
                string usernme = "Edtshuma";
                invoice.RequestedBy = usernme.ToString();
                invoice.CreatedOn = DateTime.Now;
               
                using (SqlConnection conn = new SqlConnection(DatabaseConnect))
                {

                    SqlCommand cmd = new SqlCommand("CreateVendor", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CreatedBy", invoice.RequestedBy);
                    cmd.Parameters.AddWithValue("@CreatedOn", invoice.CreatedOn);                  
                    cmd.Parameters.AddWithValue("@Vendor", invoice.VendorName);
                    cmd.Parameters.AddWithValue("@Active", invoice.Active);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
               
                ViewBag.SuccessMessage = Messages.OPERATION_COMPLETED_SUCCESSFULLY;
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View(invoice);
            }
           
        }










        [HttpGet]
        public ActionResult CreateService()
        {
            Invoice invoice = new Invoice();

            return View(invoice);
        }

        [HttpPost]
        public ActionResult CreateService(FormCollection formcollection)
        {
            Invoice invoice = new Invoice();

            try
            {
                string usernme = "Edtshuma";
                invoice.RequestedBy = usernme.ToString();
                invoice.CreatedOn = DateTime.Now;
                invoice.Active = Convert.ToInt32(formcollection["Active"]);
                invoice.AllocationName = Convert.ToString(formcollection["AllocationName"]);

                using (SqlConnection conn = new SqlConnection(DatabaseConnect))
                {

                    SqlCommand cmd = new SqlCommand("CreateAllocation", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CreatedBy", invoice.RequestedBy);
                    cmd.Parameters.AddWithValue("@CreatedOn", invoice.CreatedOn);
                  //  cmd.Parameters.AddWithValue("@FinishBy", invoice.FinishBy);
                    cmd.Parameters.AddWithValue("@AllocationName", invoice.AllocationName);
                    cmd.Parameters.AddWithValue("@Active", invoice.Active);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                ViewBag.SuccessMessage = Messages.OPERATION_COMPLETED_SUCCESSFULLY;
                return RedirectToAction("Index");


            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);

                return View(invoice);
            }
        }



        public ActionResult PurchaseDetails(int id)
        {
            List<Invoice> parameters = new List<Invoice>();
            Invoice invoice = new Invoice();
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConnect))
                {

                    SqlCommand cmd = new SqlCommand("SELECT * FROM Invoices WHERE InvoiceId=" + Convert.ToInt32(id) + "", conn);

                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        invoice.AllocationId = Convert.ToInt32(dr["AllocationId"]);
                        invoice.ExpId = Convert.ToInt32(dr["ExpId"]);
                        invoice.InvoiceId = Convert.ToInt32(dr["InvoiceId"]);
                        invoice.VendorId = Convert.ToInt32(dr["VendorId"]);
                        invoice.UnitId = Convert.ToInt32(dr["UnitId"]);
                        invoice.CurrencyId = Convert.ToInt32(dr["CurrencyId"]);
                        invoice.StatusId = Convert.ToInt32(dr["StatusId"]);
                        invoice.DocumentUrl= Convert.ToString(dr["DocumentUrl"]);
                        invoice.Reference = Convert.ToString(dr["Reference"]);
                        Session["Reference"] = invoice.Reference;
                        invoice.Description = Convert.ToString(dr["Description"]);
                        invoice.Quantity = Convert.ToInt32(dr["Quantity"]);
                        invoice.CreatedOn = Convert.ToDateTime(dr["CreatedOn"]);
                        invoice.QuotedAmount = Convert.ToDouble(dr["QuotedAmount"]);
                        invoice.ActualAmount = Convert.ToDouble(dr["ActualAmount"]);
                        invoice.Discount = Convert.ToDouble(dr["Discount"]);
                        

                        using (SqlConnection conn1 = new SqlConnection(DatabaseConnect))
                        {
                            SqlCommand cmd1 = new SqlCommand("SELECT FlagText FROM LookUp WHERE Flag=" + Convert.ToInt32(invoice.StatusId) + "", conn1);

                            conn1.Open();
                            SqlDataReader dr1 = cmd1.ExecuteReader();
                            if (dr1.Read() == true)
                            {
                                invoice.Status = Convert.ToString(dr1["FlagText"]);
                            }

                            conn1.Close();
                        }
                        using (SqlConnection conn1 = new SqlConnection(DatabaseConnect))
                        {
                            SqlCommand cmd1 = new SqlCommand("SELECT AllocationName FROM Allocations WHERE AllocationId= '" + Convert.ToInt32(invoice.AllocationId) + "'", conn1);
                            conn1.Open();
                            SqlDataReader dr1 = cmd1.ExecuteReader();
                            if (dr1.Read())
                            {
                                invoice.Allocation = Convert.ToString(dr1["AllocationName"]);
                            }
                            conn1.Close();
                        }

                        using (SqlConnection conn1 = new SqlConnection(DatabaseConnect))
                        {
                            SqlCommand cmd1 = new SqlCommand("SELECT VendorName FROM Vendors WHERE VendorId= '" + Convert.ToInt32(invoice.VendorId) + "'", conn1);
                            conn1.Open();
                            SqlDataReader dr1 = cmd1.ExecuteReader();
                            if (dr1.Read())
                            {
                               invoice.Vendor = Convert.ToString(dr1["VendorName"]);
                            }
                            conn1.Close();
                        }

                        using (SqlConnection conn1 = new SqlConnection(DatabaseConnect))
                        {
                            SqlCommand cmd1 = new SqlCommand("SELECT UnitName FROM BusinessUnits WHERE UnitId= '" + Convert.ToInt32(invoice.UnitId) + "'", conn1);
                            conn1.Open();
                            SqlDataReader dr1 = cmd1.ExecuteReader();
                            if (dr1.Read())
                            {
                                invoice.BusUnit = Convert.ToString(dr1["UnitName"]);
                            }

                            conn1.Close();
                        }

                        using (SqlConnection conn1 = new SqlConnection(DatabaseConnect))
                        {

                            SqlCommand cmd1 = new SqlCommand("SELECT ExpenseName FROM ExpenseTypes WHERE ExpId= '" + Convert.ToInt32(invoice.ExpId) + "'", conn1);
                            conn1.Open();
                            SqlDataReader dr1 = cmd1.ExecuteReader();
                            if (dr1.Read())
                            {
                              invoice.ExpType = Convert.ToString(dr1["ExpenseName"]);
                            }

                            conn1.Close();
                        }
                        parameters.Add(invoice);

                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                ViewBag.ErrorMessage = Helpers.Helpers.Messages.GENERAL_ERROR;
                return View();
            }
            return View(parameters);
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

        public static List<SelectListItem> PopulateDeparments()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            using (SqlConnection conn = new SqlConnection(DatabaseConnect))
            using (SqlCommand cmd = new SqlCommand("SELECT DepartmentId, DepartmentName FROM Departments ORDER BY DepartmentId", conn))
            {

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        items.Add(new SelectListItem
                        {
                            Text = Convert.ToString(dr["DepartmentName"]),
                            Value = Convert.ToString(dr["DepartmentId"])
                        });
                    }
                }
            }

            return items;
        }

        [HttpGet]
        public ActionResult GetDepartmentsByUnit(int UnitId)
        {

            List<SelectListItem> items = new List<SelectListItem>();

            using (SqlConnection conn = new SqlConnection(Helpers.Helpers.DatabaseConnect))
            {

                SqlCommand cmd = new SqlCommand("SELECT DepartmentId,DepartmentName FROM Departments WHERE UnitId=" + Convert.ToInt32(UnitId) + "", conn);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    items.Add(new SelectListItem
                    {
                        Text = Convert.ToString(dr["DepartmentName"]),
                        Value = Convert.ToString(dr["DepartmentId"])

                    });
                }

                conn.Close();
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }


        // GET: Invoicing/Create
        public ActionResult Create()
        {
            Invoice invoice = new Invoice();
            invoice.Currencies = PopulateCurrencies();
            invoice.Services = PopulateServices();
            invoice.Units = PopulateUnits();
            invoice.Vendors = PopulateVendors();
            invoice.Expenses = PopulateExpenses();
            invoice.PaymentTypes = PopulatePaymentTypes();
            invoice.Departments = PopulateDeparments();
            return View(invoice);
        }

        // POST: Invoicing/Create
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase PostedFile, FormCollection formcollection)
        {         

            Invoice invoice = new Invoice();
            invoice.Currencies = PopulateCurrencies();
            invoice.Services = PopulateServices();
            invoice.Units = PopulateUnits();
            invoice.Vendors = PopulateVendors();
            invoice.Expenses = PopulateExpenses();
            invoice.PaymentTypes = PopulatePaymentTypes();
            invoice.Departments = PopulateDeparments();

            try
            {
               
                invoice.StatusId = Convert.ToInt32(parameters.fresh);
                invoice.RequestedBy = Convert.ToString(Session["Username"]);               
                invoice.CreatedOn = DateTime.Now;
                int Status = 1;

                invoice.AllocationId= Convert.ToInt32(formcollection["AllocationId"]);
                invoice.UnitId = Convert.ToInt32(formcollection["UnitId"]);
                invoice.VendorId = Convert.ToInt32(formcollection["VendorId"]);
                invoice.CurrencyId = Convert.ToInt32(formcollection["CurrencyId"]);
                invoice.PaymentTypeId = Convert.ToInt32(formcollection["PaymentTypeId"]);
                invoice.ExpId = Convert.ToInt32(formcollection["ExpId"]);
                invoice.DepartmentId = Convert.ToInt32(formcollection["DepartmentId"]);
                invoice.Quantity = Convert.ToInt32(formcollection["Quantity"]);
                invoice.QuotedAmount = Convert.ToDouble(formcollection["QuotedAmount"]);
                invoice.ActualAmount = Convert.ToDouble(formcollection["ActualAmount"]);
                invoice.Description = Convert.ToString(formcollection["Description"]);


                if (Convert.ToString(formcollection["CurrencyId"]) != null && Convert.ToString(formcollection["VendorId"]) != null && Convert.ToString(formcollection["AllocationId"]) != null && Convert.ToString(formcollection["UnitId"]) != null && Convert.ToString(formcollection["PaymentTypeId"]) != null && Convert.ToString(formcollection["ExpId"]) != null)
                {

                    string FileExt = Path.GetExtension(PostedFile.FileName).ToUpper();

                    //if (FileExt == ".PDF" || FileExt == ".DOCX" && FileExt == ".TXT" || FileExt == ".EML" && FileExt == ".JPEG" || FileExt == ".JPG")
                    //{
                        var fileName = String.Format("{1}{2}{0}", Path.GetFileName(PostedFile.FileName), invoice.RequestedBy, DateTime.Now.ToString("yyyyMMddHHmmss_"));
                        var path = "";
                        var illegalChar = "/fbc/";
                        path = Path.Combine(Server.MapPath("~/Uploads/InvoiceAttachments/"), fileName);
                        var absPath = path.Replace(illegalChar, "/");
                        PostedFile.SaveAs(absPath);

                    if (invoice.ActualAmount < invoice.QuotedAmount)
                    {
                        invoice.Discount = invoice.QuotedAmount - invoice.ActualAmount;
                    }
                    else if (invoice.ActualAmount >= invoice.QuotedAmount)
                    {
                        invoice.Discount = 0.00;
                    }

                    using (SqlConnection conn = new SqlConnection(DatabaseConnect))
                    {

                            SqlCommand cmd = new SqlCommand("CreateInvoice", conn);
                            cmd.CommandType = CommandType.StoredProcedure;


                        if (Convert.ToInt32(invoice.ExpId) == parameters.fresh)
                        {
                            conn.Open();
                            SqlCommand cmdd = new SqlCommand("GetHighestCAPCode", conn);
                            cmdd.CommandType = CommandType.StoredProcedure;
                            SqlParameter param = new SqlParameter("@returnValue", SqlDbType.Int);
                            cmdd.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.Output;
                            cmdd.ExecuteNonQuery();
                            int Counter = Convert.ToInt32(cmdd.Parameters["@returnValue"].Value);
                            int newCode = Counter + 1;
                            conn.Close();
                            int x = newCode++;                           
                            var capex = "CAP" + x.ToString("000000");
                            invoice.Reference = capex;
                            cmd.Parameters.AddWithValue("@Reference", invoice.Reference);
                        }
                        else if (Convert.ToInt32(invoice.ExpId) == parameters.recommended)
                        {
                            conn.Open();
                            SqlCommand cmdd = new SqlCommand("GetHighestOPCode", conn);
                            cmdd.CommandType = CommandType.StoredProcedure;
                            SqlParameter param = new SqlParameter("@returnValue", SqlDbType.Int);
                            cmdd.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.Output;
                            cmdd.ExecuteNonQuery();
                            int Counter = Convert.ToInt32(cmdd.Parameters["@returnValue"].Value);
                            int newCode = Counter + 1;
                            conn.Close();
                            int x = newCode++;                           
                            var opex = "OPE" + x.ToString("000000");                           
                            invoice.Reference = opex;
                            cmd.Parameters.AddWithValue("@Reference", invoice.Reference);
                        }

                            cmd.Parameters.AddWithValue("@FileName", Path.GetFileName(PostedFile.FileName));
                            cmd.Parameters.AddWithValue("@AllocationId", invoice.AllocationId);
                            cmd.Parameters.AddWithValue("@VendorId", invoice.VendorId);
                            cmd.Parameters.AddWithValue("@UnitId", invoice.UnitId);
                            cmd.Parameters.AddWithValue("@CurrencyId", invoice.CurrencyId);
                            cmd.Parameters.AddWithValue("@ExpId", invoice.ExpId);
                            cmd.Parameters.AddWithValue("@DepartmentId", invoice.DepartmentId);
                            cmd.Parameters.AddWithValue("@QuotedAmount", invoice.QuotedAmount);
                            cmd.Parameters.AddWithValue("@ActualAmount", invoice.ActualAmount);
                            cmd.Parameters.AddWithValue("Quantity", invoice.Quantity);
                            cmd.Parameters.AddWithValue("@CreatedBy", invoice.RequestedBy);
                            cmd.Parameters.AddWithValue("@CreatedOn", invoice.CreatedOn);
                            cmd.Parameters.AddWithValue("@DocumentUrl", path);
                            //cmd.Parameters.AddWithValue("@DocumentUrl2", path);
                            cmd.Parameters.AddWithValue("@PaymentTypeId", invoice.PaymentTypeId);
                            cmd.Parameters.AddWithValue("@Discount", invoice.Discount);
                            
                            cmd.Parameters.AddWithValue("@Status", Status);
                            cmd.Parameters.AddWithValue("@Description", invoice.Description);
                            conn.Open();
                            cmd.ExecuteNonQuery();


                        using (SqlConnection connn = new SqlConnection(DatabaseConnect))
                        {
                            string ent = "INVOICE";
                            var create = parameters.CreateAction.ToString();
                            var username = Session["Username"].ToString();
                            invoice.Action = create;
                            invoice.Username = username;
                            invoice.Entity = ent.ToString();

                            SqlCommand cmdd = new SqlCommand("CreateTrail", connn);
                            cmdd.CommandType = CommandType.StoredProcedure;

                            cmdd.Parameters.AddWithValue("@Action", invoice.Action);
                            cmdd.Parameters.AddWithValue("@CreatedBy", invoice.RequestedBy);
                            cmdd.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
                            cmdd.Parameters.AddWithValue("@Entity", invoice.Entity);
                            cmdd.Parameters.AddWithValue("@Username", invoice.Username);
                            connn.Open();
                            cmdd.ExecuteNonQuery();
                            connn.Close();
                        }
                    }

                        ViewBag.SuccessMessage = Messages.OPERATION_COMPLETED_SUCCESSFULLY;
                        return RedirectToAction("Index");


                    //}

                    //else
                    //{
                    //    ViewBag.ErrorMessage = "Invalid file format!";
                    //    return View(invoice);
                    //}

                }
                else
                {
                    ViewBag.ErrorMessage = "Missing input !";
                   
                    return View(invoice);
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                //Invoice invoicee = new Invoice();
                invoice.Currencies = PopulateCurrencies();
                invoice.Services = PopulateServices();
                invoice.Units = PopulateUnits();
                invoice.Vendors = PopulateVendors();
                invoice.Expenses = PopulateExpenses();
                invoice.PaymentTypes = PopulatePaymentTypes();
                invoice.Departments = PopulateDeparments();
                return View(invoice);
            }
          
        }

        // GET: Invoicing/Edit/5
        public ActionResult UpdateInvoice(int id)
        {
            Invoice invoice = new Invoice();
            return View(invoice);
        }
        // POST: Invoicing/Edit/5
        [HttpPost]
        public ActionResult UpdateInvoice( FormCollection collection, HttpPostedFileBase PostedFile)
        {
            Invoice invoice = new Invoice();
            invoice.InvoiceId = Convert.ToInt32(Url.RequestContext.RouteData.Values["InvoiceId"]);
            try
            {
                String FileExt = Path.GetExtension(PostedFile.FileName).ToUpper();

                if (FileExt == ".PDF" || FileExt == ".DOCX" && FileExt == ".TXT" || FileExt == ".EML" && FileExt == ".JPEG" || FileExt == ".JPG")
                {
                    var fileName = String.Format("{1}{2}{0}", Path.GetFileName(PostedFile.FileName), invoice.RequestedBy, DateTime.Now.ToString("yyyyMMddHHmmss_"));
                    var path = "";
                    path = Path.Combine(Server.MapPath("~/Uploads/InvoiceAttachments/"), fileName);

                    using (SqlConnection conn = new SqlConnection(DatabaseConnect))
                    {
                        conn.Open();
                        SqlCommand cmdd = new SqlCommand("UpdateInvoice", conn);
                        cmdd.Parameters.AddWithValue("@DocumentUrl2", path);
                        cmdd.Parameters.AddWithValue("@ActualAmount", invoice.ActualAmount);
                        cmdd.Parameters.AddWithValue("@Status", Helpers.Helpers.parameters.recommended);
                        conn.Open();
                        cmdd.ExecuteNonQuery();

                        PostedFile.SaveAs(path);
                    }
                    ViewBag.SuccessMessage = Messages.OPERATION_COMPLETED_SUCCESSFULLY;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View(invoice);
            }
            return RedirectToAction("Index");
        }

        public FileResult download(int id)
        {
            var rdt = "Edtshuma";
            var usrname = rdt.ToString();
           // Convert.ToString(Session["UserId"])
            var invoice = Invoice.find(id);
            var downloader = new DownloadFile(invoice.DocumentUrl, String.Format("{0}_invoice",usrname));

            return downloader.download();
        }

        // GET: Invoicing/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Invoicing/Delete/5
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


        public ActionResult Vendors()
        {
            List<Invoice> parameters = new List<Invoice>();
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConnect))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Vendors ORDER BY VendorId", conn);

                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Invoice parameter = new Invoice();
                        parameter.VendorId = Convert.ToInt32(dr["VendorId"]);
                        parameter.Vendor = Convert.ToString(dr["VendorName"]);
                       
                        parameter.CreatedOn = Convert.ToDateTime(dr["CreatedOn"]);
                        parameter.Active = Convert.ToInt32(dr["Active"]);
                        if (parameter.Active == Helpers.Helpers.parameters.active)
                        {
                            parameter.Act = true;
                        }

                        else
                        {
                            parameter.Act = false;
                        }

                        parameters.Add(parameter);
                    }
                }
            }
            catch (Exception ex)
            {

                ExceptionLogging.SendErrorToText(ex);
                ViewBag.ErrorMessage = Messages.GENERAL_ERROR;
                return View();
            }
            return View(parameters);
        }

       
      

 [HttpGet]
        public ActionResult UpdateVendor(int id)
        {
            Invoice invoice = new Invoice();
            try
            {
                using (SqlConnection conn = new SqlConnection(Helpers.Helpers.DatabaseConnect))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Vendors WHERE VendorId=" + Convert.ToInt32(id) + "", conn);

                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {

                        invoice.VendorId = Convert.ToInt32(dr["VendorId"]);
                        invoice.Allocation = Convert.ToString(dr["VendorName"]);
                        invoice.Active = Convert.ToInt32(dr["Active"]);
                    }

                }

            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = Helpers.Helpers.Messages.GENERAL_ERROR;
                return View(invoice);
            }
            return View(invoice);
        }

        [HttpPost]
        public ActionResult UpdateVendor(FormCollection formcollection)
        {

            Invoice invoice = new Invoice();
            try
            {
                invoice.VendorId = Convert.ToInt32(formcollection["VendorId"]);
                invoice.Vendor = Helpers.Helpers.SanitiseInput(Convert.ToString(formcollection["VendorName"]));
                invoice.Active = Convert.ToInt32(formcollection["Active"]);
                invoice.ModifiedBy = Convert.ToString(Session["UserId"]);
                invoice.ModifiedOn = DateTime.Now;


                if (ModelState.IsValid)
                {

                    using (SqlConnection conn = new SqlConnection(Helpers.Helpers.DatabaseConnect))
                    {

                        SqlCommand cmd = new SqlCommand("UpdateVendor", conn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@VendorId", invoice.VendorId);
                        cmd.Parameters.AddWithValue("@VendorName", invoice.Vendor);
                        cmd.Parameters.AddWithValue("@Active", invoice.Active);
                        cmd.Parameters.AddWithValue("@ModifiedBy", invoice.ModifiedBy);
                        cmd.Parameters.AddWithValue("@ModifiedOn", invoice.ModifiedOn);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                    }
                    ViewBag.SuccessMessage = Helpers.Helpers.Messages.OPERATION_COMPLETED_SUCCESSFULLY;
                    return RedirectToAction("Vendors");
                }
            }

            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(invoice);
            }

            return RedirectToAction("Vendors");
        }

      [HttpGet]
        public ActionResult UpdateService(int id)
        {
            Invoice invoice = new Invoice();
            try
            {
                using (SqlConnection conn = new SqlConnection(Helpers.Helpers.DatabaseConnect))
                {

                    SqlCommand cmd = new SqlCommand("SELECT * FROM Allocations WHERE AllocationId=" + Convert.ToInt32(id) + "", conn);

                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        invoice.AllocationId = Convert.ToInt32(dr["AllocationId"]);
                        invoice.Allocation = Convert.ToString(dr["Allocation"]);
                        invoice.Active = Convert.ToInt32(dr["Active"]);
                    }
                }
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = Helpers.Helpers.Messages.GENERAL_ERROR;
                return View();
            }
            return View(invoice);
        }

        [HttpPost]
        public ActionResult UpdateService(FormCollection formcollection)
        {

            Invoice invoice = new Invoice();
            try
            {
                invoice.VendorId = Convert.ToInt32(formcollection["AllocationId"]);
                invoice.Vendor = Helpers.Helpers.SanitiseInput(Convert.ToString(formcollection["Allocation"]));
                invoice.Active = Convert.ToInt32(formcollection["Active"]);
                invoice.ModifiedBy = Convert.ToString(Session["UserId"]);
                invoice.ModifiedOn = DateTime.Now;

                if (ModelState.IsValid)
                {

                    using (SqlConnection conn = new SqlConnection(Helpers.Helpers.DatabaseConnect))
                    {
                        SqlCommand cmd = new SqlCommand("UpdateService", conn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@AllocationId", invoice.AllocationId);
                        cmd.Parameters.AddWithValue("@AllocationName", invoice.Allocation);
                        cmd.Parameters.AddWithValue("@Active", invoice.Active);
                        cmd.Parameters.AddWithValue("@ModifiedBy", invoice.ModifiedBy);
                        cmd.Parameters.AddWithValue("@ModifiedOn", invoice.ModifiedOn);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                    }
                    ViewBag.SuccessMessage = Helpers.Helpers.Messages.OPERATION_COMPLETED_SUCCESSFULLY;
                    return RedirectToAction("Services");
                }
            }

            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(invoice);
            }

            return RedirectToAction("Services");
        }



        public ActionResult Services()
        {
            List<Invoice> parameters = new List<Invoice>();
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConnect))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Allocations ORDER BY AllocationId", conn);

                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Invoice parameter = new Invoice();
                        parameter.AllocationId = Convert.ToInt32(dr["AllocationId"]);
                        parameter.Allocation = Convert.ToString(dr["AllocationName"]);
                        parameter.CreatedOn = Convert.ToDateTime(dr["CreatedOn"]);
                        parameter.Active= Convert.ToInt32(dr["Active"]);
                        if (parameter.Active == Helpers.Helpers.parameters.active)
                        {
                            parameter.Act = true;
                        }

                        else
                        {
                            parameter.Act = false;
                        }

                        parameters.Add(parameter);
                    }
                }
            }
            catch (Exception ex)
            {

                ExceptionLogging.SendErrorToText(ex);
                ViewBag.ErrorMessage = Messages.GENERAL_ERROR;
                return View();
            }
            return View(parameters);
        }

        public ActionResult Departments()
        {
            List<Invoice> parameters = new List<Invoice>();
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConnect))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM vw_DepartmentsByUnit ", conn);

                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Invoice parameter = new Invoice();
                        parameter.BusUnit = Convert.ToString(dr["UnitName"]);
                        parameter.Dpt = Convert.ToString(dr["DepartmentName"]);
                        parameter.CreatedOn = Convert.ToDateTime(dr["CreatedOn"]);
                        parameter.Active = Convert.ToInt32(dr["Active"]);
                        if (parameter.Active == Helpers.Helpers.parameters.active)
                        {
                            parameter.Act = true;
                        }

                        else
                        {
                            parameter.Act = false;
                        }

                        parameters.Add(parameter);
                    }
                }
            }
            catch (Exception ex)
            {

                ExceptionLogging.SendErrorToText(ex);
                ViewBag.ErrorMessage = Messages.GENERAL_ERROR;
                return View();
            }
            return View(parameters);
        }
    }
}
