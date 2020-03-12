using PaySys.Exceptions;
using PaySys.Models;
using PaySys.Models.Task;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static PaySys.Helpers.Helpers;

namespace PaySys.Controllers
{
    public class TaskController : Controller
    {
        // GET: Task
        public ActionResult Index()
        {
            List<Task> parameters = new List<Task>();
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConnect))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Tasks ORDER BY TaskId", conn);

                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Task parameter = new Task();
                        parameter.TaskId = Convert.ToInt32(dr["TaskId"]);
                        parameter.PriorityId = Convert.ToInt32(dr["PriorityId"]);
                        parameter.Reference = Convert.ToString(dr["Reference"]);
                        parameter.TaskName = Convert.ToString(dr["TaskName"]);
                        parameter.CreatedOn = Convert.ToDateTime(dr["CreatedOn"]);
                        parameter.StatusId = Convert.ToInt32(dr["Status"]);
                        parameter.AssignedTo = Convert.ToString(dr["AssignedTo"]);

                        using (SqlConnection conn1 = new SqlConnection(DatabaseConnect))
                        {

                            SqlCommand cmd1 = new SqlCommand("SELECT FlagText FROM LookUp WHERE Flag=" + Convert.ToInt32(parameter.StatusId) + "", conn1);

                            conn1.Open();
                            SqlDataReader dr1 = cmd1.ExecuteReader();
                            if (dr1.Read() == true)
                            {
                                parameter.taskStatus = Convert.ToString(dr1["FlagText"]);
                            }

                            conn1.Close();
                        }
                        using (SqlConnection conn1 = new SqlConnection(DatabaseConnect))
                        {

                            SqlCommand cmd1 = new SqlCommand("SELECT Priority FROM Priorities WHERE PriorityId= '" + Convert.ToInt32(parameter.PriorityId) + "'", conn1);
                            conn1.Open();
                            SqlDataReader dr1 = cmd1.ExecuteReader();
                            if (dr1.Read())
                            {
                                parameter.Priority = Convert.ToString(dr1["Priority"]);
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

        [HttpGet]
        public ActionResult Details(int id)
        {
            List<Task> parameters = new List<Task>();
            Task task = new Task();
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConnect))
                {

                    SqlCommand cmd = new SqlCommand("SELECT * FROM Tasks WHERE TaskId=" + Convert.ToInt32(id) + "", conn);

                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        task.StatusId = Convert.ToInt32(dr["Status"]);
                        task.TaskId = Convert.ToInt32(dr["TaskId"]);
                        task.Reference = Convert.ToString(dr["Reference"]);
                        task.Description = Convert.ToString(dr["Description"]);
                        task.CreatedOn = Convert.ToDateTime(dr["CreatedOn"]);
                        task.RequestedBy = Convert.ToString(dr["CreatedBy"]);
                        task.TaskName = Convert.ToString(dr["TaskName"]);
                        task.PriorityId = Convert.ToInt32(dr["PriorityId"]);
                        task.FinishBy = Convert.ToDateTime(dr["FinishBy"]);
                        task.AssignedTo = Convert.ToString(dr["AssignedTo"]);

                        using (SqlConnection conn1 = new SqlConnection(DatabaseConnect))
                        {
                            SqlCommand cmd1 = new SqlCommand("SELECT FlagText FROM LookUp WHERE Flag=" + Convert.ToInt32(task.StatusId) + "", conn1);

                            conn1.Open();
                            SqlDataReader dr1 = cmd1.ExecuteReader();
                            if (dr1.Read() == true)
                            {
                                task.taskStatus = Convert.ToString(dr1["FlagText"]);
                            }

                            conn1.Close();
                        }
                        using (SqlConnection conn1 = new SqlConnection(DatabaseConnect))
                        {
                            SqlCommand cmd1 = new SqlCommand("SELECT Priority FROM Priorities WHERE PriorityId= '" + Convert.ToInt32(task.PriorityId) + "'", conn1);
                            conn1.Open();
                            SqlDataReader dr1 = cmd1.ExecuteReader();
                            if (dr1.Read())
                            {
                                task.Priority = Convert.ToString(dr1["Priority"]);
                            }
                            conn1.Close();
                        }
                        parameters.Add(task);
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
        private static List<SelectListItem> PopulateEmployees()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            using (SqlConnection conn = new SqlConnection(DatabaseConnect))
            {

                SqlCommand cmd = new SqlCommand("SELECT EmployeeNumber,DisplayName FROM vw_Employee WHERE Active=" + Convert.ToInt32(parameters.active) + "", conn);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    items.Add(new SelectListItem
                    {
                        Text = Convert.ToString(dr["DisplayName"]),
                        Value = Convert.ToString(dr["EmployeeNumber"])

                    });
                }

                conn.Close();
            }

            return items;
        }


        public static List<SelectListItem> PopulatePriorities()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            using (SqlConnection conn = new SqlConnection(Helpers.Helpers.DatabaseConnect))
            using (SqlCommand cmd = new SqlCommand("SELECT PriorityId, Priority FROM Priorities ORDER BY PriorityId", conn))
            {

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        items.Add(new SelectListItem
                        {
                            Text = Convert.ToString(dr["Priority"]),
                            Value = Convert.ToString(dr["PriorityId"])
                        });
                    }
                }
            }

            return items;
        }


        public ActionResult ChooseReport()
        {
             Task task = new Task();
            return View(task);
        }

        public ActionResult CompleteTask()
        {

            return View();
        }

        [HttpPost]
        public ActionResult CompleteTask(FormCollection formcollection)
        {
            var user = "Admin";
            string admin = user.ToString();
            Models.Task.Task param = new Models.Task.Task();
            
            param.Status = Convert.ToInt32(formcollection["Status"]);
            param.Comment = Convert.ToString(formcollection["Comment"]);
            param.TaskId = Convert.ToInt32(Url.RequestContext.RouteData.Values["id"]);
            param.CompletedBy = admin;
            param.CompletedOn = DateTime.Now;

            if (ModelState.IsValid)
            {
                
                if (param.Status == Helpers.Helpers.parameters.approved)
                {
                    using (SqlConnection conn = new SqlConnection(DatabaseConnect))
                    {

                        SqlCommand cmd = new SqlCommand("CompleteTask", conn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@TaskId", param.TaskId);
                        cmd.Parameters.AddWithValue("@TaskStatus", param.Status);
                        cmd.Parameters.AddWithValue("@ComplComment", param.Comment);
                        cmd.Parameters.AddWithValue("@CompletedBy", param.CompletedBy);
                        cmd.Parameters.AddWithValue("@CompletedOn", param.CompletedOn);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                    }
                }
                else
                {
                    using (SqlConnection conn = new SqlConnection(DatabaseConnect))
                    {

                        SqlCommand cmd = new SqlCommand("DeferTask", conn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@TaskId", param.TaskId);
                        cmd.Parameters.AddWithValue("@TaskStatus", param.Status);
                        cmd.Parameters.AddWithValue("@ComplComment", param.Comment);
                        cmd.Parameters.AddWithValue("@DeferredBy", param.CompletedBy);
                        cmd.Parameters.AddWithValue("@DeferredOn", param.CompletedOn);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                    }

                }
            }
            return RedirectToAction("Index");
        }

       


        public ActionResult UpdateTask(int id)
        {
            Task task = new Task();
            task.TaskPriorities = PopulatePriorities();
            task.Employees = PopulateEmployees();
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConnect))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Tasks WHERE TaskId=" + Convert.ToInt32(id) + "", conn);

                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {

                        task.TaskId = Convert.ToInt32(dr["TaskId"]);
                        task.Description = Convert.ToString(dr["Description"]);
                        task.TaskName = Convert.ToString(dr["TaskName"]);
                        task.FinishBy = Convert.ToDateTime(dr["FinishBy"]);
                        task.PriorityId = Convert.ToInt32(dr["PriorityId"]);
                        task.EmployeeNumber = Convert.ToString(dr["AssignedTo"]);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                ViewBag.ErrorMessage = Messages.GENERAL_ERROR;
                return View(task);
            }
            return View(task);           
        }

        // POST: Invoicing/Edit/5
        [HttpPost]
        public ActionResult UpdateTask(FormCollection formCollection)
        {
            Task task = new Task();
            task.TaskPriorities = PopulatePriorities();
            task.Employees = PopulateEmployees();

            task.PriorityId = Convert.ToInt32(formCollection["PriorityId"]);
            task.TaskName = Convert.ToString(formCollection["TaskName"]);
            task.Description = Convert.ToString(formCollection["Description"]);
            task.TaskId = Convert.ToInt32(formCollection["TaskId"]);
            task.EmployeeNumber = Convert.ToString(formCollection["EmployeeNumber"]);
            task.FinishBy = Convert.ToDateTime(formCollection["FinishBy"]);
            try
            {
              //  task.TaskId = Convert.ToInt32(Url.RequestContext.RouteData.Values["id"]);
                string usernme = "Edtshuma";
                task.CompletedBy = usernme.ToString();
                task.CompletedOn = DateTime.Now;
               
                using (SqlConnection conn = new SqlConnection(DatabaseConnect))
                    {
                        conn.Open();
                        SqlCommand cmdd = new SqlCommand("UpdateTask", conn);
                    cmdd.Parameters.AddWithValue("@TaskId", task.TaskId);
                    cmdd.Parameters.AddWithValue("@Description", task.Description);
                        cmdd.Parameters.AddWithValue("@PriorityId",task.PriorityId);
                    cmdd.Parameters.AddWithValue("@AssignedTo", task.EmployeeNumber);
                    cmdd.Parameters.AddWithValue("@FinishBy", task.FinishBy);
                    cmdd.Parameters.AddWithValue("@TaskName", task.TaskName);

                    cmdd.ExecuteNonQuery();
                    conn.Close();
                }
                    ViewBag.SuccessMessage = Messages.OPERATION_COMPLETED_SUCCESSFULLY;
                    return RedirectToAction("Index");
               
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                task.TaskPriorities = PopulatePriorities();
                task.Employees = PopulateEmployees();

                return View(task);
            }
           
        }



        // GET: Task/Create
        public ActionResult Create()
        {
            Task task = new Task();
            //task.TaskPriorities = PopulatePriorities();
            //task.Employees = PopulateEmployees();
            return View(task);
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



        // POST: Task/Create
        [HttpPost]
        public ActionResult Create(FormCollection formcollection)
        {
            Task task = new Task();
            //task.TaskPriorities = PopulatePriorities();
            //task.Employees = PopulateEmployees();

          //  try
           // {
                string usernme = "Edtshuma";        
                task.RequestedBy = usernme.ToString();
                task.CreatedOn = DateTime.Now;
                int Status = 1;
                //task.CompletedBy= usernme.ToString();
                //task.CompletedOn= DateTime.Now;
                //task.DeferredBy = usernme.ToString();
                //task.DeferredTo = DateTime.Now;
                //task.PriorityId = Convert.ToInt32(formcollection["PriorityId"]);
                //task.FinishBy = Convert.ToDateTime(formcollection["FinishBy"]);
                //task.EmployeeNumber = Convert.ToString(formcollection["EmployeeNumber"]);
                task.TaskName = Convert.ToString(formcollection["TaskName"]);
                task.Description = Convert.ToString(formcollection["Description"]);


            using (SqlConnection conn = new SqlConnection(DatabaseConnect))
            {

                SqlCommand cmd = new SqlCommand("CreateVendor", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CreatedBy", task.RequestedBy);
                cmd.Parameters.AddWithValue("@CreatedOn", task.CreatedOn);
                cmd.Parameters.AddWithValue("@Vendor", task.TaskName);
                cmd.Parameters.AddWithValue("@Active", Status);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            using (SqlConnection conn = new SqlConnection(DatabaseConnect))
            {
                string ent = "VENDOR";
                var create = parameters.CreateAction.ToString();
                
                var username = Session["Username"].ToString();

                task.Action = create;
                task.Username = username;
                task.Entity = ent.ToString();

                SqlCommand cmd = new SqlCommand("CreateTrail", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", task.Action);
                cmd.Parameters.AddWithValue("@CreatedBy", task.RequestedBy);
                cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
                cmd.Parameters.AddWithValue("@Entity", task.Entity);
                cmd.Parameters.AddWithValue("@Username", task.Username);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }


            //    if (Convert.ToString(formcollection["PriorityId"]) != null && Convert.ToString(formcollection["EmployeeNumber"]) != null)
            //    {

            //        using (SqlConnection conn = new SqlConnection(DatabaseConnect))
            //        {

            //            SqlCommand cmd = new SqlCommand("CreateTask", conn);
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            //if (Convert.ToInt32(invoice.ExpId) == parameters.fresh)
            //            //{
            //            //    conn.Open();
            //            //    SqlCommand cmdd = new SqlCommand("GetHighestCAPCode", conn);
            //            //    cmdd.CommandType = CommandType.StoredProcedure;
            //            //    SqlParameter param = new SqlParameter("@returnValue", SqlDbType.Int);
            //            //    cmdd.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.Output;
            //            //    cmdd.ExecuteNonQuery();
            //            //    int Counter = Convert.ToInt32(cmdd.Parameters["@returnValue"].Value);
            //            //    int newCode = Counter + 1;
            //            //    conn.Close();
            //            var newCode = 1;
            //            int x = newCode++;
            //              var capex = "TSK" + x.ToString("000000");
            //               task.Reference = capex;
            //                cmd.Parameters.AddWithValue("@Reference",task.Reference);
            //            // }
            //            //else if (Convert.ToInt32(invoice.ExpId) == parameters.recommended)
            //            //{
            //            //    conn.Open();
            //            //    SqlCommand cmdd = new SqlCommand("GetHighestOPCode", conn);
            //            //    cmdd.CommandType = CommandType.StoredProcedure;
            //            //    SqlParameter param = new SqlParameter("@returnValue", SqlDbType.Int);
            //            //    cmdd.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.Output;
            //            //    cmdd.ExecuteNonQuery();
            //            //    int Counter = Convert.ToInt32(cmdd.Parameters["@returnValue"].Value);
            //            //    int newCode = Counter + 1;
            //            //    conn.Close();
            //            //    int x = newCode++;
            //            //    var opex = "OPE" + x.ToString("000000");
            //            //    invoice.Reference = opex;
            //            //    cmd.Parameters.AddWithValue("@Reference", invoice.Reference);
            //            //}

            //            // cmd.Parameters.AddWithValue("@FileName", Path.GetFileName(PostedFile.FileName));
            //            cmd.Parameters.AddWithValue("@PriorityId", task.PriorityId);
            //            cmd.Parameters.AddWithValue("@CreatedBy", task.RequestedBy);
            //            cmd.Parameters.AddWithValue("@CreatedOn", task.CreatedOn);
            //            cmd.Parameters.AddWithValue("@FinishBy", task.FinishBy);
            //            cmd.Parameters.AddWithValue("@TaskName", task.TaskName);
            //            cmd.Parameters.AddWithValue("@Status", Status);
            //            cmd.Parameters.AddWithValue("@Description", task.Description);
            //            cmd.Parameters.AddWithValue("@AssignedTo", task.EmployeeNumber);
            //            cmd.Parameters.AddWithValue("@CompletedBy", task.CompletedBy);
            //            cmd.Parameters.AddWithValue("@CompletedOn", task.CompletedOn);
            //            cmd.Parameters.AddWithValue("@DeferredBy", task.DeferredBy);
            //            cmd.Parameters.AddWithValue("@DeferredOn", task.DeferredTo);
            //            conn.Open();
            //            cmd.ExecuteNonQuery();
            //        }

            //        ViewBag.SuccessMessage = Messages.OPERATION_COMPLETED_SUCCESSFULLY;
            return RedirectToAction("Vendors");

            //    }
            //    else
            //    {
            //        ViewBag.ErrorMessage = "Missing input !";

            //        return View(task);
            //    }

            //}
            //catch (Exception ex)
            //{
            //    ExceptionLogging.SendErrorToText(ex);
            //    task.TaskPriorities = PopulatePriorities();
            //    task.Employees = PopulateEmployees();
            //    return View(task);
            //}
        }

        // GET: Task/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Task/Edit/5
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

        // GET: Task/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Task/Delete/5
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
