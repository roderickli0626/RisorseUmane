using RisorseUmane.Common;
using RisorseUmane.Controller;
using RisorseUmane.DAO;
using RisorseUmane.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RisorseUmane
{
    public partial class Dashboard : System.Web.UI.Page
    {
        Hashtable RequestList;

        User user;
        LoginController loginSystem = new LoginController();
        RequestDAO requestDAO = new RequestDAO();
        protected void Page_Load(object sender, EventArgs e)
        {
            user = loginSystem.GetCurrentUserAccount();
            if (!loginSystem.IsCEOLoggedIn() && !loginSystem.IsAdminLoggedIn() && (user == null))
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            RequestList = GetRequests(null, "", 0, 0);

            Calendar1.FirstDayOfWeek = FirstDayOfWeek.Sunday;
            Calendar1.NextPrevFormat = NextPrevFormat.ShortMonth;
            Calendar1.TitleFormat = TitleFormat.Month;
            Calendar1.ShowGridLines = true;
            Calendar1.DayStyle.Height = new Unit(80);
            Calendar1.DayStyle.Width = new Unit(200);
            Calendar1.DayStyle.HorizontalAlign = HorizontalAlign.Center;
            Calendar1.DayStyle.VerticalAlign = VerticalAlign.Middle;

            if (!IsPostBack)
            {
                LoadExpireDates();
                LoadType();
                LoadState();
            }
        }

        public void LoadExpireDates()
        {
            List<RememberDate> result = new RememberDateDAO().FindAll().Where(r => r.State == (int)State.Accepted).ToList();
            if (loginSystem.IsEmployerLoggedIn() || loginSystem.IsLogisticLoggedIn())
            {
                result = result.Where(r => r.UserId == user.Id).ToList();
            }

            ExpireDateRepeater.DataSource = result.Select(r => new RememeberCheck(r));
            ExpireDateRepeater.DataBind();
        }
        public void LoadType()
        {
            ComboType.Items.Clear();
            ComboType.Items.Add(new ListItem("TUTTI TIPO", "0"));
            ComboType.Items.Add(new ListItem("FEIRE", ((int)RequestType.Ferie).ToString()));
            ComboType.Items.Add(new ListItem("MALATTIA", ((int)RequestType.Malattia).ToString()));
            ComboType.Items.Add(new ListItem("DPI", ((int)RequestType.Dpi).ToString()));
            ComboType.Items.Add(new ListItem("MANUTENZIONE", ((int)RequestType.Manutenzione).ToString()));
        }
        public void LoadState() 
        {
            ComboState.Items.Clear();
            ComboState.Items.Add(new ListItem("TUTTI STATO", "0"));
            ComboState.Items.Add(new ListItem("PROGRESS", ((int)State.Progress).ToString()));
            ComboState.Items.Add(new ListItem("ACCEPTED", ((int)State.Accepted).ToString()));
            ComboState.Items.Add(new ListItem("REJECTED", ((int)State.Rejected).ToString()));
        }

        private Hashtable GetRequests(DateTime? searchDate, string searchKey, int state, int type)
        {
            Hashtable holiday = new Hashtable();

            List<Request> requests = requestDAO.FindAll().ToList();
            if (loginSystem.IsEmployerLoggedIn() || loginSystem.IsLogisticLoggedIn()) 
            { 
                requests = requests.Where(r => r.SenderId == user.Id).ToList();
            }
            if (state != 0) requests = requests.Where(r => (r.AState != (int)State.Progress && r.AState == state) || r.SState == state).ToList();
            if (type != 0) requests = requests.Where(r => r.RequestType == type).ToList();
            requests = requests.Where(r => new UserDAO().FindByID(r.SenderId).Name.Contains(searchKey)).ToList();
            if (searchDate != null) requests = requests.Where(r => r.FromDate != null && r.ToDate != null && r.FromDate <= searchDate.Value && r.ToDate >= searchDate.Value).ToList();

            foreach (Request request in requests)
            {
                //string subTitle = "Status: ";

                //holiday[request.CreatedDate?.ToString("dd/MM/yyyy")] += "<a class='d-block' title='" + subTitle + "' href='AdminOrderEdit.aspx?id=" + order.Id + "'>" + order.Host.Name + " " + order.Id + "<br/>(End:" + order.EndDate?.ToString("dd/MM/yyyy") + ")</a>";
            }

            return holiday;
        }
        protected void TxtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        protected void ComboType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ComboState_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void TxtDate_TextChanged(object sender, EventArgs e)
        {

        }
    }
}