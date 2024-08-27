using RisorseUmane.Common;
using RisorseUmane.Controller;
using RisorseUmane.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RisorseUmane
{
    public partial class User1 : System.Web.UI.Page
    {
        User user;
        LoginController loginSystem = new LoginController();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!loginSystem.IsCEOLoggedIn() && !loginSystem.IsAdminLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadUserRole();
            }
        }

        private void LoadUserRole()
        {
            ComboType1.Items.Clear();
            ComboType1.Items.Add(new ListItem("STAFF", ((int)Role.Staff).ToString()));
            ComboType1.Items.Add(new ListItem("EMPLOYER", ((int)Role.Employer).ToString()));
            ComboType1.Items.Add(new ListItem("LOGISTIC", ((int)Role.Logistic).ToString()));
        }


        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;
            string name = TxtName.Text;
            string email = TxtEmail.Text;
            string surname = TxtSurname.Text;
            int? role = ControlUtil.GetSelectedValue(ComboType1);
            string password = TxtPassword.Text;
            string repeatPW = TxtPasswordRepeat.Text;
            string mobile = TxtMobile.Text;

            if (password != repeatPW)
            {
                PasswordValidator.IsValid = false;
                return;
            }
            string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            if (!Regex.IsMatch(email, emailPattern))
            {
                EmailValidator.IsValid = false;
                return;
            }
            EncryptedPass pass = null;
            if (!string.IsNullOrEmpty(password))
            {
                pass = new EncryptedPass() { Encrypted = new CryptoController().EncryptStringAES(password), UnEncrypted = password };
            }
            int? userID = ParseUtil.TryParseInt(HfUserID.Value);

            bool success = new UserController().SaveUser(userID, name, email, pass, mobile, surname, role);
            if (success)
            {
                Page.Response.Redirect(Page.Request.Url.ToString(), true);
            }
            else
            {
                ServerValidator.IsValid = false;
                return;
            }
        }
    }
}