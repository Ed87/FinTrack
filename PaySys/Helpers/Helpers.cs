using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace PaySys.Helpers
{
    public static class Helpers
    {
        public class DownloadFile : Controller
        {
            private string _Url;
            private string _fileName;
            public DownloadFile(string url, string fileName)
            {

                _Url = url;
                _fileName = fileName;

            }
            public FileResult download()
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(_Url);
                string[] a = _Url.Split('.');
                _fileName = String.Format("{1}.{0}", a[a.Length - 1], _fileName);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, _fileName);
            }
        }
        public static class Messages
        {

            public static string JDMismatch = "This Job Title Does Not Belong to This Department ";
            public static string BONUS_ALREADY_SET = "Already Configured Bonus For This Tax Year ";
            public static string PASSWORD_CHANGE_SUCCESSFULL = "Password Has Been Changed Successfully. Next Login Use The New Password ";
            public static string NoTitleChange = "There is no change with Employee's current Job Title";
            public static string NoDepartmentChange = "There is no change with Employee's current Department";
            public static string NoStationChange = "There is no change with Employee's current Work Station";
            public static string INSUFFICIENTDAYS = "You Do Not Have Sufficient Leave Days, Your Annual Leave Balance Is ";
            public static string INVALID_NUMBERING = "Invalid Numbering Of Days, Make Sure That Days Are Normally Arranged";
            public static string AlreadyProcessSalaryForThisMonth = "Already Processd Salary For This Month";
            public static string AlreadySubscribedToCredit = "Employee Already Subscribed to this Tax Concession";
            public static string AlreadyProcessZimdefForThisMonth = "ZIMDEF Remittance Job Already Processed For This Month";
            public static string AlreadyProcessSdfForThisMonth = "SDF Remittance Job Already Processed For This Month";
            public static string AlreadyProcessBenefit = "Business Rule Violation ! Grade already configured for this Benefit Type";
            public static string AlreadyProcessClaim = "Employee Already Has An Active Maintenance Claim";
            public static string AgeViolation = "Age below 55 yrs! Employee not Eligible for this Credit";
            public static string THRESHOLD_VIOLATION = "Claim Exceeds Threshold for thIs Employee.Input a Lower Amount";
            public static string AlreadyProcessConcession = "Business Rule Violation ! Employee has reached Maximum Number of Tax Credit Claims";
            public static string PayRunDateError = "You can Not process Payroll At This Time, Wait For The Approved Date";
            public static string RECRUITMENT_REQUISITION_EXHASUSTED = "Number of recuits needed on this requisition code has been reached";
            public static string INVALID_CREDENTIALS = "You have entered invalid credentials";
            public static string ALREADY_APPLIED_FOR_SALARY_ADVANCE = "You have Already Made Application Which is Waiting For Approval, Please Relax";
            public static string APPLICATION_IN_PROGRESS = "You have Already Made Application Which is Waiting For Approval, Please Relax";

            public static string EMPTY_USERNAME_PASSWORD = "Empty username and/or password";
            public static string OPERATION_COMPLETED_SUCCESSFULLY = "Operation Completed Successfully";
            public static string WRONG_FULL_NAME_FORMAT = "Wrong full name format, it should be like " + "Samson Fiado" + " and avoid spaces";
            public static string PASSWORD_COMFIRM_PASSWORD_MISMATCH = "Password and Comfirm Password Mismatch";
            public static string DONT_HAVE_ACCESS = "You do not have access to the page";
            public static string MINIMUM_DAYS_TO_APPLY_FOR_LEAVE_GRANT = "Minimum leave days to apply for leave grant is 7, you are applying for: ";
            public static string START_DATE_MUST_BE_SMALLER = "Starting Date must be smaller than end date";
            public static string INVALID_LEAVE_DATES_ENTRY = "Invalid leave dates entry";
            public static string APPLICATION_CANT_BE_EDITED_AT_THIS_STAGE = "Application can not be edited at this stage,talk to your line manager";
            public static string ALREADY_APPLIED_FOR_LEAVE_GRANT_THIS_FISCAL_YEAR = "You have already applied for Leave Grant this financial year";
            public static string NOT_ELIGIBLE_FOR_LEAVE_GRANT = "You are not eligible for leave grant";
            public static string GENERAL_ERROR = "Invalid Action!";
            public static string SORT_OUT_SALARY_ADVANCE_ISSUE = "Sort out Salary Advance Issues First";
            public static string LOAN_POLICY_VIOLATION_LIMIT_PERC = "Loan Policy Violation, Monthly Loan Deduction Superceed the Recommended ";
            public static string ADVANCE_POLICY_VIOLATION_LIMIT_PERC = "Advance Policy Violation,Deduction Superceed the Recommended ";
            public static string NoModeSelected = "Please Select Salary Adjustment Mode";
            public static string NoEmployeeSelected = "Please Select Employee for Claim";
            public static string NoPayCodeSelected = "Please Select Salary Pay Day Code";
            public static string NotBusinessDay = "You cannot perform this action today. Transaction can only be done on a Working Day.";
            public static string NoGradeBenefitSelected = "Missing Parameters ! Select both Benefit Type and Job Grade ";
            public static string view { get; set; }
            public static string TRAINING = "Development Training";
            public static string SKILLSGAP = "Skills Gap";


        }
        public static string ConvertStringArrayToString(string[] array)
        {
            // Concatenate all the elements into a StringBuilder.
            StringBuilder builder = new StringBuilder();
            foreach (string value in array)
            {
                builder.Append(value);
                builder.Append(',');
            }
            return builder.ToString();
        }

     


        public struct TrainingReason
        {
            public string ID;
            public string REASON;

            public TrainingReason(string id, string reason)
            {
                ID = id;
                REASON = reason;
            }
        }

        public struct Punishment
        {
            public string ID { get; set; }
            public string PUNISHMENT { get; set; }

            public Punishment(string id, string punishment)
            {
                ID = id;
                PUNISHMENT = punishment;
            }
        }


        public struct NotificationType
        {
            public string ID { get; set; }
            public string TYPE { get; set; }

            public NotificationType(string id, string type)
            {
                ID = id;
                TYPE = type;
            }
        }

        public class parameters
        {
            public static int Total_Records_Per_Page_On_Search = Int32.MaxValue - 1;
            public static string Married = "Married";
            public static string Admin = "Admin";
            public static string General = "SGM";
            public static string SystemUser = "Index";
            public static string SectionHead = "hos";
            public static string DepartmentalHead = "hod";
            public static string Administration = "HRAdmin";
            public static string Executive = "exec";

        
            //FileFormats
            public static int Pdf = 1;
            public static int Excel = 2;
            public static int Csv = 3;
            public static int Html = 4;
            public static int Xml = 5;


            //ActionTypes
            public static string DeleteAction = "DELETE";
            public static string CreateAction = "CREATE";
            public static string UpdateAction = "UPDATE";
            public static string ProcessAction = "PROCESS";





            //Tx Credit params
            //user roles strings
            public static int user = 1;
            public static int active = 1;

            //CompensationParameters
            public static String Taxable = "1";
            public static String NotTaxable = "0";
            public static String Active = "1";
            public static String NotActive = "0";


            //user roles strings
            public static string userrole = "1";
            public static string hosrole = "2";
            public static string hodrole = "3";
            public static string hradminrole = "4";
            public static string execrole = "5";


            public static int uncomplented = 1;
            public static int fresh = 2;
            public static int recommended = 3;
            public static int NotRecommended = 3;
            public static int approved = 3;
            public static int rejected = 2;
            public static int Cancelled = 6;


            


            //system roles

            public static int User = 1;
            public static int sectionhead = 2;
            public static int hraadmin = 1002;
            public static int payrollmaster = 4;
            public static int admin = 5;
            public static int superadmin = 6;
            public static int IHead = 1002;





            //Training Reasons

            public static IEnumerable<TrainingReason> TRAINING_REASONS = new List<TrainingReason>
            {
                new TrainingReason("1","Skills Gap"),
                new TrainingReason("2","Developmental Training")

            };

            public static IEnumerable<NotificationType> NOTIFICATION_TYPES = new List<NotificationType>()
            {
                new NotificationType("Announcement","Announcement"),
                new NotificationType("Memorandum","Memorandum")
            };

            public static IEnumerable<Punishment> PUNISHMENTS = new List<Punishment>()
            {
                new Punishment("first Written Warning","first Written Warning"),
                new Punishment("second Written Warning","second Written Warning"),
                new Punishment("final Written Warning","final Written Warning"),
                new Punishment("dismissal","dismissal")
            };

        }
        public static string ToSafeFileName(string s)
        {
            return s
                .Replace("\\", "")
                .Replace("/", "")
                .Replace("\"", "")
                .Replace("*", "")
                .Replace(":", "")
                .Replace("?", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("|", "");
        }
        public static string DatabaseConnect = ConfigurationManager.ConnectionStrings["PayPoint"].ConnectionString;
        public static bool CheckConnection()
        {
            String address = "208.69.34.231";
            System.Net.NetworkInformation.Ping pingsender = new System.Net.NetworkInformation.Ping();
            System.Net.NetworkInformation.PingReply reply = pingsender.Send(address, 1000);
            if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //Duplicates in NewBroker
        public static string AddSuffix(string filename, string suffix)
        {
            string fDir = Path.GetDirectoryName(filename);
            string fName = Path.GetFileNameWithoutExtension(filename);
            string fExt = Path.GetExtension(filename);
            string renamedFilePath = Path.Combine(fDir, String.Concat(fName, suffix, fExt));
            return renamedFilePath;
        }

        //public static class Extensions
        //{
        //    public static string ConvertToString(this char[] array)
        //    {
        //        return new string(array);
        //    }
        //}
        // string s = array.ConvertToString();


        public static string CovertString(this char[] array)
        {
            //public static string ConvertToString(this char[] array)
            //{
            //    return new string(array);
            //}
            return new string(array);
        }


        public static int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }


        private const string cryptoKey = "AHCX@2017";

        public static string BandIdentifier = "Tier";

        public const string VALID_MOBILE_REGEX = @"^(099|088|+265)\d{7}$";
        public const string VALID_TELEPHONE_REGEX = @"^(01)\d{6}$";

        public static readonly string NumericRegex = @"^\d+$";

        // The Initialization Vector for the DES encryption routine
        private static readonly byte[] IV = new byte[8] { 240, 3, 45, 29, 0, 76, 173, 59 };


        // Decrypts provided string parameter

        public static string Decrypt(string s)
        {
            if (s == null || s.Length == 0) return string.Empty;
            string result = string.Empty;
            try
            {
                byte[] buffer = Convert.FromBase64String(s);
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
                des.Key = MD5.ComputeHash(Encoding.ASCII.GetBytes(cryptoKey));
                des.IV = IV;
                result = Encoding.ASCII.GetString(des.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch
            {
                return string.Empty;
            }
            return result;
        }

        public static T IsNull<T>(T Value, T DefaultValue)
        {
            if (Value == null || Convert.IsDBNull(Value)) return DefaultValue;
            else return Value;

        }


        // Sanitises input by removing invalid characters and trimming spaces

        public static string SanitiseInput(string InputString)
        {
            try
            {
                return Regex.Replace(InputString, "[^.A-Za-z0-9 _]", string.Empty).Trim();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        //gets SelectedTxt of SelectList
        public static string getText(SelectList selectList)
        {
            string text = selectList.Where(x => x.Selected).FirstOrDefault().Text;
            return text;
        }
        // Converts a string to proper Title Case (e.g. SAMSON FIADO ==> Samson Fiado)

        public static string TitleCaseString(string s)
        {
            if (s == null) return s;
            string[] words = s.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length == 0) continue;
                char firstChar = char.ToUpper(words[i][0]);
                string rest = string.Empty;
                if (words[i].Length > 1)
                {
                    rest = words[i].Substring(1).ToLower();
                }
                words[i] = firstChar + rest;
            }
            return string.Join(" ", words);
        }

        public static string HashPassword(string plainText)
        {
            try
            {
                SHA512Managed managed = new SHA512Managed();
                return Convert.ToBase64String(managed.ComputeHash(Encoding.ASCII.GetBytes(plainText)));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetUsername(string input)
        {
            String[] names = input.Split(' ');
            String lname = names.Last();
            String fname = names.First();
            char fletter = fname[0];

            String username = fletter + lname;

            return username.ToLower();

        }
        public static long GetPassword()
        {
            return DateTime.Now.Ticks;
        }



        public static string Zimra = "Zimbabwe";


        public static string FormatName(string theName)
        {

            TextInfo theNameFormatting = Thread.CurrentThread.CurrentCulture.TextInfo;
            theName = Regex.Replace(theName, "[ ]{2,}", " ");
            if (!string.IsNullOrEmpty(theName))
            {
                return Helpers.TitleCaseString(theNameFormatting.ToTitleCase(theName)).Trim();
            }
            else
            {
                return string.Empty;
            }
        }


        public static string RemoveWhitespace(string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }

        public static string ConvertToCommonDateFormat(DateTime Input)
        {
            try
            {
                return Input.ToString("dd MMM yyyy HH:mm");
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}