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
    public partial class Manutenzione : System.Web.UI.Page
    {
        User user;
        LoginController loginSystem = new LoginController();
        protected void Page_Load(object sender, EventArgs e)
        {
            user = loginSystem.GetCurrentUserAccount();
            if (!loginSystem.IsEmployerLoggedIn() && !loginSystem.IsLogisticLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadTagType();
            }
        }

        private void LoadTagType()
        {
            ComboType.Items.Clear();
            ComboType.Items.Add(new ListItem("TUTTI", "0"));
            ComboType.Items.Add(new ListItem("ACCEPTED", ((int)State.Accepted).ToString()));
            ComboType.Items.Add(new ListItem("REJECTED", ((int)State.Rejected).ToString()));
            ComboType.Items.Add(new ListItem("PROGRESS", ((int)State.Progress).ToString()));
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;
            string description = TxtDescription.Text;
            int senderID = user.Id;

            int? requestID = ParseUtil.TryParseInt(HfManutenzioneID.Value);

            bool success = new RequestController().SaveManutenzioneRequest(requestID, senderID, description);
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