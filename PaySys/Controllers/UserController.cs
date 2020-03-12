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

        // GET: User/Create
        public ActionResult Create()
        {

            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }

        //[HttpPost]
        //[ActionName("Login")]
        //public ActionResult UserLogin(FormCollection formcollection)

        //{

        //    LoginViewModel model = new LoginViewModel();
        //    try

        //    {
        //        using (SqlConnection conn12 = new SqlConnection(Helpers.Helpers.DatabaseConnect))
        //        {
        //            //string logSystem = Helpers.EventViewer();
        //            SqlCommand cmd12 = new SqlCommand("SELECT * FROM EmployeeInfo", conn12);

        //            conn12.Open();
        //            SqlDataReader dr12 = cmd12.ExecuteReader();
        //            if (dr12.Read())
        //            {

        //               model.UserId = Convert.ToString(formcollection["UserId"]);
        //                // login.UserName = formcollection["Username"].ToString();
        //               model.UserPassword = formcollection["UserPassword"].ToString();
        //                var password = model.UserPassword;
        //                model.UserPassword = Helpers.Helpers.HashPassword(model.UserPassword);

        //                //if (login.UserName == "" || login.UserPassword == "" || login.UserId == "")
        //                if (model.UserPassword == "" || model.UserId == "")
        //                {
        //                    ViewBag.ErrorMessage = Helpers.Helpers.Messages.EMPTY_USERNAME_PASSWORD;
        //                    return View();
        //                }
        //                else
        //                {
        //                    using (SqlConnection conn2 = new SqlConnection(Helpers.Helpers.DatabaseConnect))
        //                    {
        //                        SqlCommand cmd2 = new SqlCommand("SELECT * FROM EmployeeInfo ", conn2);

        //                        conn2.Open();
        //                        SqlDataReader dr2 = cmd2.ExecuteReader();
        //                        if (dr2.Read() == true)
        //                        {
        //                            using (SqlConnection conn1 = new SqlConnection(Helpers.Helpers.DatabaseConnect))
        //                            {

        //                                SqlCommand cmd1 = new SqlCommand("SELECT EmploymentType,JobTitle,Department FROM EmployeeInfo WHERE EmployeeNumber='" + Convert.ToString(model.UserId) + "'", conn1);

        //                                conn1.Open();
        //                                SqlDataReader dr1 = cmd1.ExecuteReader();
        //                                if (dr1.Read() == true)
        //                                {
        //                                    model.JobTitle = Convert.ToInt32(dr1["JobTitle"]);
        //                                    model.Department = Convert.ToString(dr1["Department"]);
        //                                    model.EmploymentType = Convert.ToInt32(dr1["EmploymentType"]);
        //                                    // login.EmployeeGender = Convert.ToString(dr1["Sex"]);
        //                                }
        //                                conn1.Close();
        //                            }

        //                            using (SqlConnection conn1 = new SqlConnection(Helpers.Helpers.DatabaseConnect))
        //                            {
        //                                SqlCommand cmd1 = new SqlCommand("SELECT SystemRole,JobTitleName FROM vw_JobTitle WHERE JobTitleId=" + Convert.ToInt32(login.JobTitle) + "", conn1);

        //                                conn1.Open();
        //                                SqlDataReader dr1 = cmd1.ExecuteReader();
        //                                if (dr1.Read() == true)
        //                                {
        //                                    model.RoleId = Convert.ToInt32(dr1["SystemRole"]);
        //                                    model.Position = Convert.ToString(dr1["JobTitleName"]);
        //                                }
        //                                conn1.Close();
        //                            }
        //                            using (SqlConnection conn1 = new SqlConnection(Helpers.Helpers.DatabaseConnect))
        //                            {
        //                                SqlCommand cmd1 = new SqlCommand("SELECT SystemRole,JobTitleName FROM vw_JobTitle WHERE JobTitleId=" + Convert.ToInt32(model.JobTitle) + "", conn1);

        //                                conn1.Open();
        //                                SqlDataReader dr1 = cmd1.ExecuteReader();
        //                                if (dr1.Read() == true)
        //                                {
        //                                    model.RoleId = Convert.ToInt32(dr1["SystemRole"]);
        //                                    model.Position = Convert.ToString(dr1["JobTitleName"]);
        //                                }
        //                                conn1.Close();
        //                            }
        //                        }

        //                        conn2.Close();
        //                    }

        //                    using (SqlConnection conn = new SqlConnection(Helpers.Helpers.DatabaseConnect))
        //                    {

        //                        SqlCommand cmd = new SqlCommand("ValidateUserCredentials", conn);
        //                        cmd.CommandType = CommandType.StoredProcedure;
        //                        conn.Open();

        //                        //Passing the entered employee number,username and password to the database for authentication
        //                        cmd.Parameters.AddWithValue("@UserId", model.UserId);
        //                        // cmd.Parameters.AddWithValue("@Username", login.UserName);
        //                        cmd.Parameters.AddWithValue("@Password", model.UserPassword);
        //                        SqlDataReader dr = cmd.ExecuteReader();
        //                        if (dr.Read())
        //                        {
        //                            // login.UserName = dr["UserName"].ToString();
        //                            model.DisplayName = dr["DisplayName"].ToString();
        //                            model.UserId = Convert.ToString(dr["EmployeeNumber"]);
        //                        }
        //                        else
        //                        {
        //                            ViewBag.ErrorMessage = Helpers.Helpers.Messages.INVALID_CREDENTIALS;
        //                            return View();

        //                        }
        //                        conn.Close();
        //                    }

        //                    //Session["picture"] = login.picture;
        //                    Session["JobTitle"] = model.Position;
        //                    Session["JobTitleId"] = model.JobTitle;
        //                    Session["Username"] = model.DisplayName;
        //                    Session["UserId"] =  model.UserId;
        //                    Session["Department"] = model.Department;
        //                    Session["UserRole"] = model.RoleId;
        //                    //Session["LogUserName"] = login.UserName;
        //                    //Session["FinancialYear"] = login.FiscalYear;
        //                    Session["EmploymentType"] = model.EmploymentType;
        //                    // Session["Gender"] = login.EmployeeGender;


        //                    //Get user workstation

        //                    string workstation;
        //                   // string MacAddress = Helpers.Helpers.GetMacAddress();
        //                    string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        //                    if (string.IsNullOrEmpty(ipAddress))
        //                        ipAddress = Request.UserHostAddress;
        //                    workstation = Request.UserHostName;


        //                    //using (SqlConnection conn = new SqlConnection(Helpers.Helpers.DatabaseConnect))
        //                    //{

        //                    //    SqlCommand cmd = new SqlCommand("SubmitLog", conn);
        //                    //    cmd.CommandType = CommandType.StoredProcedure;

        //                    //    cmd.Parameters.AddWithValue("@IpAddress", ipAddress);
        //                    //    cmd.Parameters.AddWithValue("@MacAddress", MacAddress);
        //                    //    cmd.Parameters.AddWithValue("@WorkStation", workstation);
        //                    //    cmd.Parameters.AddWithValue("@AccessTime", DateTime.Now);
        //                    //    cmd.Parameters.AddWithValue("@UserId", login.UserId);

        //                    //    conn.Open();
        //                    //    cmd.ExecuteNonQuery();

        //                    //}

        //                    // Prompting user to change password if not yet changed

        //                    //if (password == login.UserName + "!")
        //                    //{
        //                    //    return View("ChangePassword");
        //                    //}
        //                    return RedirectToAction("Index", "FileUpload");

        //                }
        //            }

        //            else
        //            {

        //                login.UserId = Convert.ToString(formcollection["UserId"]);
        //                //  login.UserName = formcollection["Username"].ToString();
        //                login.UserPassword = formcollection["UserPassword"].ToString();
        //                model.UserPassword = Helpers.HashPassword(model.UserPassword);

        //                if (model.UserPassword == "" || model.UserId == "")
        //                // if (login.UserName == "" || login.UserPassword == "" || login.UserId == "")
        //                {
        //                    ViewBag.ErrorMessage = Helpers.Messages.EMPTY_USERNAME_PASSWORD;
        //                    return View();

        //                }

        //                else
        //                {

        //                    Session["JobTitle"] = parameters.Admin;
        //                    //  Session["Username"] = login.UserName;
        //                    Session["UserId"] = login.UserId;
        //                    Session["UserRole"] = parameters.hraadmin;
        //                    Session["FinancialYear"] = login.FiscalYear;

        //                }

        //                return RedirectToAction("Index", "Home");
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.ErrorMessage = ex.Message;
        //    }

        //    return View("Login");
        //}




        //  POST: User/Create
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {


            //LoginViewModel model = new LoginViewModel();
            //try

            //{
            //    using (SqlConnection conn12 = new SqlConnection(Helpers.Helpers.DatabaseConnect))
            //    {
            //        //string logSystem = Helpers.EventViewer();
            //        SqlCommand cmd12 = new SqlCommand("SELECT * FROM EmployeeInfo", conn12);

            //        conn12.Open();
            //        SqlDataReader dr12 = cmd12.ExecuteReader();
            //        if (dr12.Read())
            //        {

            //            model.UserId = Convert.ToString(formcollection["UserId"]);
            //            // login.UserName = formcollection["Username"].ToString();
            //            model.UserPassword = formcollection["UserPassword"].ToString();
            //            var password = model.UserPassword;
            //            model.UserPassword = Helpers.Helpers.HashPassword(model.UserPassword);

            //            //if (login.UserName == "" || login.UserPassword == "" || login.UserId == "")
            //            if (model.UserPassword == "" || model.UserId == "")
            //            {
            //                ViewBag.ErrorMessage = Helpers.Helpers.Messages.EMPTY_USERNAME_PASSWORD;
            //                return View();
            //            }
            //            else
            //            {
            //                using (SqlConnection conn2 = new SqlConnection(Helpers.Helpers.DatabaseConnect))
            //                {
            //                    SqlCommand cmd2 = new SqlCommand("SELECT * FROM EmployeeInfo ", conn2);

            //                    conn2.Open();
            //                    SqlDataReader dr2 = cmd2.ExecuteReader();
            //                    if (dr2.Read() == true)
            //                    {
            //                        using (SqlConnection conn1 = new SqlConnection(Helpers.Helpers.DatabaseConnect))
            //                        {

            //                            SqlCommand cmd1 = new SqlCommand("SELECT EmploymentType,JobTitle,Department FROM EmployeeInfo WHERE EmployeeNumber='" + Convert.ToString(model.UserId) + "'", conn1);

            //                            conn1.Open();
            //                            SqlDataReader dr1 = cmd1.ExecuteReader();
            //                            if (dr1.Read() == true)
            //                            {
            //                                model.JobTitle = Convert.ToInt32(dr1["JobTitle"]);
            //                                model.Department = Convert.ToString(dr1["Department"]);
            //                                model.EmploymentType = Convert.ToInt32(dr1["EmploymentType"]);
            //                                // login.EmployeeGender = Convert.ToString(dr1["Sex"]);
            //                            }
            //                            conn1.Close();
            //                        }

            //                        using (SqlConnection conn1 = new SqlConnection(Helpers.Helpers.DatabaseConnect))
            //                        {
            //                            SqlCommand cmd1 = new SqlCommand("SELECT SystemRole,JobTitleName FROM vw_JobTitle WHERE JobTitleId=" + Convert.ToInt32(model.JobTitle) + "", conn1);

            //                            conn1.Open();
            //                            SqlDataReader dr1 = cmd1.ExecuteReader();
            //                            if (dr1.Read() == true)
            //                            {
            //                                model.RoleId = Convert.ToInt32(dr1["SystemRole"]);
            //                                model.Position = Convert.ToString(dr1["JobTitleName"]);
            //                            }
            //                            conn1.Close();
            //                        }
            //                        using (SqlConnection conn1 = new SqlConnection(Helpers.Helpers.DatabaseConnect))
            //                        {
            //                            SqlCommand cmd1 = new SqlCommand("SELECT SystemRole,JobTitleName FROM vw_JobTitle WHERE JobTitleId=" + Convert.ToInt32(model.JobTitle) + "", conn1);

            //                            conn1.Open();
            //                            SqlDataReader dr1 = cmd1.ExecuteReader();
            //                            if (dr1.Read() == true)
            //                            {
            //                                model.RoleId = Convert.ToInt32(dr1["SystemRole"]);
            //                                model.Position = Convert.ToString(dr1["JobTitleName"]);
            //                            }
            //                            conn1.Close();
            //                        }
            //                    }

            //                    conn2.Close();
            //                }

            //                using (SqlConnection conn = new SqlConnection(Helpers.Helpers.DatabaseConnect))
            //                {

            //                    SqlCommand cmd = new SqlCommand("ValidateUserCredentials", conn);
            //                    cmd.CommandType = CommandType.StoredProcedure;
            //                    conn.Open();

            //                    //Passing the entered employee number,username and password to the database for authentication
            //                    cmd.Parameters.AddWithValue("@UserId", model.UserId);
            //                    // cmd.Parameters.AddWithValue("@Username", login.UserName);
            //                    cmd.Parameters.AddWithValue("@Password", model.UserPassword);
            //                    SqlDataReader dr = cmd.ExecuteReader();
            //                    if (dr.Read())
            //                    {
            //                        // login.UserName = dr["UserName"].ToString();
            //                        model.DisplayName = dr["DisplayName"].ToString();
            //                        model.UserId = Convert.ToString(dr["EmployeeNumber"]);
            //                    }
            //                    else
            //                    {
            //                        ViewBag.ErrorMessage = Helpers.Helpers.Messages.INVALID_CREDENTIALS;
            //                        return View();

            //                    }
            //                    conn.Close();
            //                }

            //                //Session["picture"] = login.picture;
            //                Session["JobTitle"] = model.Position;
            //                Session["JobTitleId"] = model.JobTitle;
            //                Session["Username"] = model.DisplayName;
            //                Session["UserId"] = model.UserId;
            //                Session["Department"] = model.Department;
            //                Session["UserRole"] = model.RoleId;
            //                //Session["LogUserName"] = login.UserName;
            //                //Session["FinancialYear"] = login.FiscalYear;
            //                Session["EmploymentType"] = model.EmploymentType;
            //                // Session["Gender"] = login.EmployeeGender;


            //                //Get user workstation

            //                string workstation;
            //                // string MacAddress = Helpers.Helpers.GetMacAddress();
            //                string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            //                if (string.IsNullOrEmpty(ipAddress))
            //                    ipAddress = Request.UserHostAddress;
            //                workstation = Request.UserHostName;


            //                //using (SqlConnection conn = new SqlConnection(Helpers.Helpers.DatabaseConnect))
            //                //{

            //                //    SqlCommand cmd = new SqlCommand("SubmitLog", conn);
            //                //    cmd.CommandType = CommandType.StoredProcedure;

            //                //    cmd.Parameters.AddWithValue("@IpAddress", ipAddress);
            //                //    cmd.Parameters.AddWithValue("@MacAddress", MacAddress);
            //                //    cmd.Parameters.AddWithValue("@WorkStation", workstation);
            //                //    cmd.Parameters.AddWithValue("@AccessTime", DateTime.Now);
            //                //    cmd.Parameters.AddWithValue("@UserId", login.UserId);

            //                //    conn.Open();
            //                //    cmd.ExecuteNonQuery();

            //                //}

            //                // Prompting user to change password if not yet changed

            //                //if (password == login.UserName + "!")
            //                //{
            //                //    return View("ChangePassword");
            //                //}
            //                return RedirectToAction("Index");

            //            }
            //        }

            //        else
            //        {

            //            model.UserId = Convert.ToString(formcollection["UserId"]);
            //            //  login.UserName = formcollection["Username"].ToString();
            //            model.UserPassword = formcollection["UserPassword"].ToString();
            //            model.UserPassword = Helpers.Helpers.HashPassword(model.UserPassword);

            //            if (model.UserPassword == "" || model.UserId == "")
            //            // if (login.UserName == "" || login.UserPassword == "" || login.UserId == "")
            //            {
            //                ViewBag.ErrorMessage = Helpers.Helpers.Messages.EMPTY_USERNAME_PASSWORD;
            //                return View();

            //            }

            //            else
            //            {

            //                Session["JobTitle"] = Helpers.Helpers.parameters.Admin;
            //                //  Session["Username"] = login.UserName;
            //                Session["UserId"] = model.UserId;
            //                Session["UserRole"] = Helpers.Helpers.parameters.hraadmin;

            //            }

            //            return RedirectToAction("Index");
            //        }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    ExceptionLogging.SendErrorToText(ex);
            //    //    ViewBag.ErrorMessage = Helpers.Helpers.Messages.GENERAL_ERROR;
            //    ViewBag.ErrorMessage = ex.Message;
            //}

            //  return View();


            try
            {
                // set up domain context
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "FBC.CORP");

                // find a user
                UserPrincipal user = UserPrincipal.FindByIdentity(ctx, model.Username);

                if (user != null)
                {
                    // check user lockout state
                    if (user.IsAccountLockedOut())
                    {
                        ViewBag.Message = "Your account is locked out";
                    }
                    else
                    {

                        //Authenticate user

                        bool authentic = false;
                        try
                        {
                            DirectoryEntry entry = new DirectoryEntry("LDAP://10.170.8.20:389/OU=FBC,DC=fbc,DC=corp", model.Username, model.Password);
                            DirectoryEntry ldapConnection = new DirectoryEntry("FBC.CORP");
                            ldapConnection.Path = "LDAP://";
                            ldapConnection.Username = "Nyakudyap";// "Mashingat";
                            ldapConnection.Password = "legend45*";//"password1*"
                            ldapConnection.AuthenticationType = AuthenticationTypes.Secure;

                            //Login with user
                            object nativeObject = entry.NativeObject;
                            authentic = true;

                            if (authentic == true)
                            {
                                //Navigate to home
                                Session["Username"] = model.Username;
                                return View("Index");
                            }
                            else
                            {
                                //MsgBox("Insufficient rights to login!", this.Page, this);
                                ViewBag.Message = "FAILED TO LOGIN";
                            }

                        }
                        catch (Exception ex)
                        {
                            ExceptionLogging.SendErrorToText(ex);
                            ViewBag.ErrorMessage = Helpers.Helpers.Messages.GENERAL_ERROR;
                            return View();
                        }
                    }

                }
                else
                {
                    // MsgBox("Could not locate user " + Session["Mutumwa"].ToString() + " from FBC.CORP Domain", this.Page, this);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                ViewBag.ErrorMessage = Helpers.Helpers.Messages.GENERAL_ERROR;
                return View();
            }
            return View();
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
