using RisorseUmane.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RisorseUmane
{
    public partial class Page : System.Web.UI.MasterPage
    {
        private User user;
        private LoginController loginSystem = new LoginController();
        protected void Page_Load(object sender, EventArgs e)
        {
            user = loginSystem.GetCurrentUserAccount();

            if (loginSystem.IsCEOLoggedIn())
            {
                AdminName.InnerText = "CEO";
                SubName.InnerText = "CEO";
                UserName.InnerText = "CEO";
                NavUserName.InnerText = "CEO";
                liStatistic.Visible = true;
                liUser.Visible = true;
            }
            else if (loginSystem.IsAdminLoggedIn())
            {
                AdminName.InnerText = "ADMIN";
                SubName.InnerText = "ADMIN";
                UserName.InnerText = "ADMIN";
                NavUserName.InnerText = "ADMIN";
                liStatistic.Visible = true;
                liUser.Visible = true;
            }
            else if (loginSystem.IsStaffLoggedIn()) 
            {
                AdminName.InnerText = user.Name;
                SubName.InnerText = "STAFF";
                UserName.InnerText = user.Name;
                NavUserName.InnerText = user.Name;
                liPresence.Visible = true;
            }
            else
            {
                AdminName.InnerText = user.Name;
                SubName.InnerText = (loginSystem.IsEmployerLoggedIn() ? "EMPLOYER" : "LOGISTIC");
                UserName.InnerText = user.Name;
                NavUserName.InnerText = user.Name;
                liMalattia.Visible = true;
                liFerie.Visible = true;
                liDPI.Visible = true;
                liManutenzione.Visible = true;
            }

            SetMenuHighlight();
        }

        protected void SetMenuHighlight()
        {
            string path = HttpContext.Current.Request.Url.AbsolutePath;

            if (path.Equals("/Presenza.aspx"))
            {
                liPresence.Attributes["class"] += " active";
            }
            else if (path.Equals("/User.aspx"))
            {
                liUser.Attributes["class"] += " active";
            }
            else if (path.Equals("/Dashboard.aspx"))
            {
                liDashboard.Attributes["class"] += " active";
            }
            else if (path.Equals("/Statistic.aspx"))
            {
                liStatistic.Attributes["class"] += " active";
            }
            else if (path.Equals("/Malattia.aspx"))
            {
                liMalattia.Attributes["class"] += " active";
            }
            else if (path.Equals("/Ferie.aspx"))
            {
                liFerie.Attributes["class"] += " active";
            }
            else if (path.Equals("/DPI.aspx"))
            {
                liDPI.Attributes["class"] += " active";
            }
            else if (path.Equals("/Manutenzione.aspx"))
            {
                liManutenzione.Attributes["class"] += " active";
            }
            else if (path.Equals("/Comunicazione.aspx"))
            {
                liComunicazione.Attributes["class"] += " active";
            }
            else if (path.Equals("/Scadenziario.aspx"))
            {
                liScadenziario.Attributes["class"] += " active";
            }
        }
    }
}