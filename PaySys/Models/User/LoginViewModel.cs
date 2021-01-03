using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaySys.Models.User
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string EmployeeNumber { get; set; }
        public string Password { get; set; }
        public string UserId{ get; set; }
        public string UserPassword { get; set; }
        public String EmployeeGender { get; set; }
        public string picture { get; set; }
        public int ActiveStatus { get; set; }
        public bool Active { get; set; }
        public String DisplayName { get; set; }
        public String DepartmentCode { get; set; }
        public String Department { get; set; }
        public int EmploymentType { get; set; }

        public int RoleId { get; set; }
        public int JobTitle { get; set; }

        public String Position { get; set; }
    }
}