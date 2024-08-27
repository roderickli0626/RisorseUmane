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
    public partial class Ferie : System.Web.UI.Page
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
            DateTime? sdate = ParseUtil.TryParseDate(TxtFrom.Text, "dd/MM/yyyy");
            DateTime? edate = ParseUtil.TryParseDate(TxtTo.Text, "dd/MM/yyyy");
            int senderID = user.Id;

            int? requestID = ParseUtil.TryParseInt(HfFerieID.Value);

            bool success = new RequestController().SaveFerieRequest(requestID, senderID, sdate, edate, description);
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