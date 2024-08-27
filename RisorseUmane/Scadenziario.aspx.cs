using RisorseUmane.Common;
using RisorseUmane.Controller;
using RisorseUmane.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RisorseUmane
{
    public partial class Scadenziario : System.Web.UI.Page
    {
        User user;
        LoginController loginSystem = new LoginController();
        RememberController controller = new RememberController();
        protected void Page_Load(object sender, EventArgs e)
        {
            user = loginSystem.GetCurrentUserAccount();
            if (!loginSystem.IsCEOLoggedIn() && !loginSystem.IsAdminLoggedIn() && (user == null))
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (loginSystem.IsEmployerLoggedIn() || loginSystem.IsLogisticLoggedIn()) HfUserLogin.Value = "User";
            else if (loginSystem.IsStaffLoggedIn())
            {
                HfUserLogin.Value = "Staff";
                HfLoginUserID.Value = (user.Id).ToString();
            }
            else HfUserLogin.Value = "false";

            if (!IsPostBack)
            {
                LoadType();
            }
        }

        private void LoadType()
        {
            ComboState.Items.Clear();
            ComboState.Items.Add(new ListItem("PROGRESS", ((int)State.Progress).ToString()));
            ComboState.Items.Add(new ListItem("ACCEPTED", ((int)State.Accepted).ToString()));
            ComboState.Items.Add(new ListItem("REJECTED", ((int)State.Rejected).ToString()));
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;
            string description = TxtDescription.Text;
            DateTime? rememberdate = ParseUtil.TryParseDate(TxtRememberDate.Text, "dd/MM/yyyy");
            int? rememberID = ParseUtil.TryParseInt(HfRememberID.Value);
            int senderID = 0;
            int state = ControlUtil.GetSelectedValue(ComboState) ?? 0;

            if (loginSystem.IsCEOLoggedIn())
            {
                state = (int)State.Accepted;
                senderID = (int)ExtraIDs.CEO;
            }
            else if (loginSystem.IsAdminLoggedIn())
            {
                state = (int)State.Accepted;
                senderID = (int)ExtraIDs.ADMIN;
            }
            else if (loginSystem.IsStaffLoggedIn())
            {
                senderID = user.Id;
                if (HfCheck.Value == "") state = (int)State.Accepted;
            }
            else
            {
                senderID = user.Id;
                state = (int)State.Progress;
            }

            if (description.Trim() == "" || rememberdate == null)
            {
                ServerValidatorForDescription.IsValid = false;
                return;
            }

            bool success = controller.SaveRemember(rememberID, senderID, state, rememberdate, description);
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