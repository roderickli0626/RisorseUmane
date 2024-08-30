using RisorseUmane.Common;
using RisorseUmane.Controller;
using RisorseUmane.DAO;
using RisorseUmane.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RisorseUmane
{
    public partial class Presenza : System.Web.UI.Page
    {
        User user;
        LoginController loginSystem = new LoginController();
        protected void Page_Load(object sender, EventArgs e)
        {
            user = loginSystem.GetCurrentUserAccount();
            if (!loginSystem.IsStaffLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            TxtDate.Text = HfDate.Value;
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            bool success = false;

            int presenceID = ParseUtil.TryParseInt(HfPresenceID.Value) ?? 0;
            int userID = ParseUtil.TryParseInt(HfUserID.Value) ?? 0;
            DateTime? date = ParseUtil.TryParseDate(HfDate.Value, "dd/MM/yyyy");

            int O = ParseUtil.TryParseInt(TxtO.Text) ?? 0;
            int S = ParseUtil.TryParseInt(TxtS.Text) ?? 0;
            string A = TxtA.Text;

            PresenceController presenceController = new PresenceController();
            success = presenceController.SavePresence(presenceID, O, S, A, userID, date);

            if (success)
            {
                string script = "$('#PresenceModal').modal('hide');datatable.fnDraw();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal", script, true);
                return;
                // Page.Response.Redirect(Page.Request.Url.ToString(), true);
            }
            else
            {
                ServerValidator.IsValid = false;
                return;
            }
        }
    }
}