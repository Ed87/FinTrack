using PaySys.Models;
using PaySys.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PaySys.Exceptions;

namespace PaySys.Controllers
{
    public class SetupController : Controller
    {
        // GET: Setup
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult Create()
        {
            Invoice invoice = new Invoice();

            return View(invoice);
        }

        [HttpPost]
        public ActionResult Create(FormCollection formcollection)
        {
            Invoice invoice = new Invoice();

            try
            {              
                invoice.RequestedBy = Session["Username"].ToString();
                invoice.CreatedOn = DateTime.Now;
                invoice.Active = Convert.ToInt32(formcollection["Active"]);
                invoice.AllocationName = Convert.ToString(formcollection["AllocationName"]);

                using (SqlConnection conn = new SqlConnection(Helpers.Helpers.DatabaseConnect))
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

                ViewBag.SuccessMessage = Helpers.Helpers.Messages.OPERATION_COMPLETED_SUCCESSFULLY;
                return RedirectToAction("Index");


            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);

                return View(invoice);
            }
        }











        public ActionResult AddVendor()
        {
            Invoice invoice = new Invoice();

            return View(invoice);
        }
        [HttpPost]
        public ActionResult AddVendor(FormCollection formcollection)
        {
            string usrname = "eddie";
            Invoice invoice = new Invoice();
            invoice.Active = Convert.ToInt32(formcollection["Active"]);
            invoice.VendorName = Convert.ToString(formcollection["VendorName"]);
           
            try
            {
                invoice.RequestedBy = usrname.ToString();
                //invoice.RequestedBy = Session["Username"].ToString();
                invoice.CreatedOn = DateTime.Now;

                using (SqlConnection conn = new SqlConnection(Helpers.Helpers.DatabaseConnect))
                {

                    SqlCommand cmd = new SqlCommand("CreateVendor", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CreatedBy", invoice.RequestedBy);
                    cmd.Parameters.AddWithValue("@CreatedOn", invoice.CreatedOn);
                    cmd.Parameters.AddWithValue("@Vendor", invoice.VendorName);
                    cmd.Parameters.AddWithValue("@Active", invoice.Active);
                    conn.Open();
                    cmd.ExecuteNonQuery();



                    using (SqlConnection connn = new SqlConnection(Helpers.Helpers.DatabaseConnect))
                    {
                        string ent = "SERVICE";
                        var create = Helpers.Helpers.parameters.CreateAction.ToString();
                        
                        var username = Session["Username"].ToString();

                        invoice.Action = create;
                        invoice.Username = usrname.ToString();
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

                ViewBag.SuccessMessage = Helpers.Helpers.Messages.OPERATION_COMPLETED_SUCCESSFULLY;
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return View(invoice);
            }

        }





    }
}