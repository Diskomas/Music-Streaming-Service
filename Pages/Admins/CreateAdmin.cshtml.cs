using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Group_Project.Pages.Admins
{
    public class CreateAdminModel : PageModel
    {
        // --------------------- VALIDATE SESSION ---------------------
        public string LoginAdminemail;
        public const string SessionKeyName1 = "LoginAdminemail";


        public string LoginAdminusername;
        public const string SessionKeyName2 = "LoginAdminusername";

        public string SessionID;
        public const string SessionKeyName3 = "AdminsessionID";
        // --------------------- VALIDATE SESSION ---------------------
        public string Message { get; set; }

        [BindProperty]
        public Models.Admins Admindetails { get; set; }

        public IActionResult OnGet()
        {
            // --------------------- VALIDATE SESSION ---------------------
            LoginAdminemail = HttpContext.Session.GetString(SessionKeyName1);
            LoginAdminusername = HttpContext.Session.GetString(SessionKeyName2);
            SessionID = HttpContext.Session.GetString(SessionKeyName3);

            if (string.IsNullOrEmpty(LoginAdminemail) && string.IsNullOrEmpty(LoginAdminusername) && string.IsNullOrEmpty(SessionID))
            {
                return RedirectToPage("/Index");
            }
            // --------------------- VALIDATE SESSION ---------------------

            return Page();
        }

        public IActionResult OnPost()
        {

            DatabaseConnection.Database_Connection dbstring = new DatabaseConnection.Database_Connection();
            string DbConnection = dbstring.DatabaseString();

            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"INSERT INTO Admin (AdminUsername, AdminEmail, AdminPasword, AdminResetUniq) VALUES (@Adminusername, @AdminEmail, @AdminPassword, @AdminResetUniq)";

                if(string.IsNullOrEmpty(Admindetails.AdminUserName) || string.IsNullOrEmpty(Admindetails.AdminEmail) || string.IsNullOrEmpty(Admindetails.AdminPassword) || string.IsNullOrEmpty(Admindetails.AdminReset))
                {
                    Message = "Please don't leave empty fields!";
                    return Page();
                }
                else
                {
                    command.Parameters.AddWithValue("@Adminusername", Admindetails.AdminUserName);
                    command.Parameters.AddWithValue("@AdminEmail", Admindetails.AdminEmail);
                    command.Parameters.AddWithValue("@AdminPassword", Admindetails.AdminPassword);
                    command.Parameters.AddWithValue("@AdminResetUniq", Admindetails.AdminReset);


                    command.ExecuteNonQuery();
                }

                    
            }



            return RedirectToPage("/admins/adminview");
        }

    }
}