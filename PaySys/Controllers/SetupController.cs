using PaySys.Models;
using PaySys.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Security.Permissions;
using PaySys.Exceptions;
using PaySys.Models.User;

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






        public static string GetProperty(SearchResult searchResult,
               string PropertyName)
        {
            if (searchResult.Properties.Contains(PropertyName))
            {
                return searchResult.Properties[PropertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }


        public ActionResult CreateADUsers()
        {
            LoginViewModel login = new LoginViewModel();
            login.Username = Convert.ToString(Session["Username"]);
            try
            {
                DirectoryEntry entry = new DirectoryEntry("LDAP://10.170.8.20:389/OU=FBC,DC=fbc,DC=corp");
                DirectoryEntry ldapConnection = new DirectoryEntry("FBC.CORP");
                ldapConnection.Path = "LDAP://";
                ldapConnection.Username = "Nyakudyap";// "Mashingat";
                ldapConnection.Password = "legend45*";//"password1*"
                ldapConnection.AuthenticationType = AuthenticationTypes.Secure;
                DirectorySearcher adSearch = new DirectorySearcher(entry);

                // Get the currently logged in user
                string name = Environment.UserName;

                adSearch.Filter = "(&(objectClass=user)(l=" + name + "))";

                foreach (SearchResult singleADUser in adSearch.FindAll())
                {
                    string employeeTitle = GetProperty(singleADUser, "title");
                    string empDisplayName = GetProperty(singleADUser, "cn"); //or displayname
                    string empUnit = GetProperty(singleADUser, "company");
                    string empInitials = GetProperty(singleADUser, "initials");
                    string empName = GetProperty(singleADUser, "givenName");
                    string empSurname = GetProperty(singleADUser, "sn");

                    string userInfo = empDisplayName + "" + employeeTitle;
                    using (SqlConnection conn = new SqlConnection(Helpers.Helpers.DatabaseConnect))
                    {
                        var act = 1;
                        SqlCommand cmd = new SqlCommand("CreateADUser", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserInfo", userInfo);
                        cmd.Parameters.AddWithValue("@UserName", empName);
                        cmd.Parameters.AddWithValue("@Surname", empSurname);
                        cmd.Parameters.AddWithValue("@UserTitle", employeeTitle);
                        cmd.Parameters.AddWithValue("@DisplayName", empDisplayName);
                        cmd.Parameters.AddWithValue("@CreatedBy", login.Username);
                        cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
                       cmd.Parameters.AddWithValue("@Active", act);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    //foreach (string singleAttribute in ((ResultPropertyCollection)singleADUser.Properties).PropertyNames)
                    //{
                    //    Console.WriteLine(singleAttribute + " = ");
                    //    foreach (Object singleValue in ((ResultPropertyCollection)singleADUser.Properties)[singleAttribute])
                    //    {
                    //        Console.WriteLine("\t" + singleValue);
                    //    }
                    //}

                }
                return View("Index");
               
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
               // ViewBag.ErrorMessage = Helpers.Helpers.Messages.GENERAL_ERROR;
                return View();
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
                        string ent = "VENDOR";
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