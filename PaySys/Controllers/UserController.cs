using PaySys.Exceptions;
using PaySys.Models.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;
using PaySys.Helpers;


//[assembly: SecurityPermission(SecurityAction.RequestMinimum, Execution = true)]
//[assembly: DirectoryServicesPermission(SecurityAction.RequestMinimum)]
namespace PaySys.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public static DirectoryEntry GetDirectoryEntry()
        {
            DirectoryEntry de = new DirectoryEntry("LDAP://10.170.8.20:389/OU=FBC,DC=fbc,DC=corp");
            de.Path = "LDAP://";
            de.AuthenticationType = AuthenticationTypes.Secure;
            return de;
        }

        [HttpGet]
        public ActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }
      


        //  POST: User/Create
        [HttpPost]
       // public ActionResult Login(LoginViewModel model)
        public ActionResult Login(FormCollection formcollection)
        {
            LoginViewModel login = new LoginViewModel();

            login.EmployeeNumber = Convert.ToString(formcollection["EmployeeNumber"]);
            login.Username = Convert.ToString(formcollection["Username"]);
            login.Password = Convert.ToString(formcollection["Password"]);
            try
            {

                        using (SqlConnection conn = new SqlConnection(Helpers.Helpers.DatabaseConnect))
                        {
                            //if (login.Username == "" || login.Password == "" || login.UserId == "")
                            //{
                            //    ViewBag.ErrorMessage = Helpers.Helpers.Messages.EMPTY_USERNAME_PASSWORD;
                            //    return View();
                            //}

                            SqlCommand cmd = new SqlCommand("ValidateUserCredentials", conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();

                           cmd.Parameters.AddWithValue("@EmployeeNumber", login.EmployeeNumber);
                            //cmd.Parameters.AddWithValue("@Username", login.Username);
                            cmd.Parameters.AddWithValue("@Password", login.Password);
                            SqlDataReader dr = cmd.ExecuteReader();
                    //if (dr.Read())
                    //{
                    //    login.Username = dr["UserName"].ToString();
                    //    login.DisplayName = dr["DisplayName"].ToString();
                    //    login.UserId = Convert.ToString(dr["EmployeeNumber"]);
                    //}
                    //else
                    //{
                    //    ViewBag.ErrorMessage = Helpers.Helpers.Messages.INVALID_CREDENTIALS;
                    //    return View();

                    //}
                    conn.Close();
                        }
              //  Session["Username"] = login.Username;
                return RedirectToAction("Index", "User");
            }

            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                ViewBag.ErrorMessage = ex.Message;
            }
            return View();



            //try
            //{

            //    PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "FBC.CORP");

            //    // find a user
            //    UserPrincipal user = UserPrincipal.FindByIdentity(ctx, model.Username);

            //    if (user != null)
            //    {
            //        // check user lockout state
            //        if (user.IsAccountLockedOut())
            //        {
            //            ViewBag.Message = "Your account is locked out";
            //        }
            //        else
            //        {

            //            //Authenticate user

            //            bool authentic = false;
            //            try
            //            {
            //                // Helpers.Helpers.GetDirectoryEntry();
            //                DirectoryEntry entry = new DirectoryEntry("LDAP://10.170.8.20:389/OU=FBC,DC=fbc,DC=corp", model.Username, model.Password);
            //                DirectoryEntry ldapConnection = new DirectoryEntry("FBC.CORP");
            //                ldapConnection.Path = "LDAP://";
            //                ldapConnection.Username = "Nyakudyap";// "Mashingat";
            //                ldapConnection.Password = "legend45*";//"password1*"
            //                ldapConnection.AuthenticationType = AuthenticationTypes.Secure;

            //                ////Login with user
            //                object nativeObject = entry.NativeObject;
            //                authentic = true;

            //                if (authentic == true)
            //                {
            //                    //Get JobTitle
            //                    //DirectoryEntry de = Helpers.Helpers.GetDirectoryEntry();
            //                    //DirectorySearcher ds = new DirectorySearcher(de);
            //                    //ds.SearchScope = SearchScope.Subtree;
            //                    //SearchResultCollection results = ds.FindAll();
            //                    Session["Username"] = model.Username;
            //                    //DirectorySearcher searcher = new DirectorySearcher(entry);
            //                    //searcher.Filter = "(&(objectClass=user) (cn=" + model.Username + "))";
            //                    //SearchResult resultProp = searcher.FindOne();


            //                    //Session["JobTitle"] = resultProp.Properties["title"][0].ToString();
            //                   // Session["JobTitle"] = strValue;

            //                    return View("Index");
            //                }
            //                else
            //                {
            //                    //MsgBox("Insufficient rights to login!", this.Page, this);
            //                    ViewBag.Message = "FAILED TO LOGIN";
            //                }

            //            }
            //            catch (Exception ex)
            //            {
            //                ExceptionLogging.SendErrorToText(ex);
            //                ViewBag.ErrorMessage = Helpers.Helpers.Messages.GENERAL_ERROR;
            //                return View();
            //            }
            //        }

            //    }
            //    else
            //    {
            //        // MsgBox("Could not locate user " + Session["Mutumwa"].ToString() + " from FBC.CORP Domain", this.Page, this);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ExceptionLogging.SendErrorToText(ex);
            //    ViewBag.ErrorMessage = Helpers.Helpers.Messages.GENERAL_ERROR;
            //    return View();
            //}
            
        }

        public ActionResult Logout()
        {
            //killing sessions on logout
            Session.Clear();
            return RedirectToAction("Login");

        }


        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
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

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
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
