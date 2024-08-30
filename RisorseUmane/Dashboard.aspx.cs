using Newtonsoft.Json;
using RisorseUmane.Common;
using RisorseUmane.Controller;
using RisorseUmane.DAO;
using RisorseUmane.Model;
using RisorseUmane.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
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
        UserDAO userDAO = new UserDAO();
        PresenceDAO presenceDAO = new PresenceDAO();
        protected void Page_Load(object sender, EventArgs e)
        {
            user = loginSystem.GetCurrentUserAccount();
            if (!loginSystem.IsCEOLoggedIn() && !loginSystem.IsAdminLoggedIn() && (user == null))
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (loginSystem.IsEmployerLoggedIn() || loginSystem.IsLogisticLoggedIn())
            {
                HfLoginRole.Value = "User";
                SearchDiv.Visible = false;
            }
            else if (loginSystem.IsStaffLoggedIn())
            {
                HfLoginRole.Value = "Staff";
            }
            else HfLoginRole.Value = "Admin";

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
            result = result.Where(r => (r.Remember ?? DateTime.Now.Date) <= DateTime.Now.Date.AddDays(7)).ToList();

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

            ComboFERState.Items.Clear();
            ComboFERState.Items.Add(new ListItem("PROGRESS", ((int)State.Progress).ToString()));
            ComboFERState.Items.Add(new ListItem("ACCEPTED", ((int)State.Accepted).ToString()));
            ComboFERState.Items.Add(new ListItem("REJECTED", ((int)State.Rejected).ToString()));

            ComboMALState.Items.Clear();
            ComboMALState.Items.Add(new ListItem("PROGRESS", ((int)State.Progress).ToString()));
            ComboMALState.Items.Add(new ListItem("ACCEPTED", ((int)State.Accepted).ToString()));
            ComboMALState.Items.Add(new ListItem("REJECTED", ((int)State.Rejected).ToString()));

            ComboMANState.Items.Clear();
            ComboMANState.Items.Add(new ListItem("PROGRESS", ((int)State.Progress).ToString()));
            ComboMANState.Items.Add(new ListItem("ACCEPTED", ((int)State.Accepted).ToString()));
            ComboMANState.Items.Add(new ListItem("REJECTED", ((int)State.Rejected).ToString()));

            ComboDPIState.Items.Clear();
            ComboDPIState.Items.Add(new ListItem("PROGRESS", ((int)State.Progress).ToString()));
            ComboDPIState.Items.Add(new ListItem("ACCEPTED", ((int)State.Accepted).ToString()));
            ComboDPIState.Items.Add(new ListItem("REJECTED", ((int)State.Rejected).ToString()));
        }

        private Hashtable GetRequests(DateTime? searchDate, string searchKey, int state, int type)
        {
            Hashtable holiday = new Hashtable();

            List<Request> requests = requestDAO.FindAll().ToList();
            if (loginSystem.IsEmployerLoggedIn() || loginSystem.IsLogisticLoggedIn()) 
            { 
                requests = requests.Where(r => r.SenderId == user.Id).ToList();
            }
            if (state != 0) requests = requests.Where(r => (r.AState != (int)State.Progress && r.AState == state) || (r.AState == (int)State.Progress && r.SState == state)).ToList();
            if (type != 0) requests = requests.Where(r => r.RequestType == type).ToList();
            requests = requests.Where(r => new UserDAO().FindByID(r.SenderId).Name.Contains(searchKey)).ToList();
            if (searchDate != null) requests = requests.Where(r => r.FromDate != null && r.ToDate != null && r.FromDate <= searchDate.Value && r.ToDate >= searchDate.Value).ToList();

            foreach (Request request in requests)
            {
                string senderName = userDAO.FindByID(request.SenderId).Name;
                int requestState = 0;
                if (request.AState != (int)State.Progress) requestState = request.AState ?? 0;
                else requestState = request.SState ?? 0;

                string requestType = "";
                if (request.RequestType == (int)RequestType.Malattia) requestType = "MAL";
                else if (request.RequestType == (int)RequestType.Ferie) requestType = "FER";
                else if (request.RequestType == (int)RequestType.Manutenzione) requestType = "MAN";
                else if (request.RequestType == (int)RequestType.Dpi) requestType = "DPI";

                string stateString = "";
                if (requestState == (int)State.Accepted) stateString = "btn-success";
                else if (requestState == (int)State.Rejected) stateString = "btn-primary";
                else if (requestState == (int)State.Progress) stateString = "btn-info";

                string requestContent = new JavaScriptSerializer().Serialize(new MalattiaRequestChecker(request));

                holiday[request.CreatedDate?.ToString("dd/MM/yyyy")] += "<button type='button' data-content='" + requestContent + "' class='mb-1 btn " + 
                    stateString + " btn" + requestType + "' title='" + requestType + ":" + senderName + 
                    "' style='border: 1px solid white;width: 55px; margin-right: -10px; margin-left: -10px;color:white;padding:0px'>" + 
                    requestType + "</button>";
            }

            return holiday;
        }
        protected void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            UpdateCalendar();
        }

        protected void ComboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCalendar();
        }

        protected void ComboState_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCalendar();
        }

        protected void TxtDate_TextChanged(object sender, EventArgs e)
        {
            UpdateCalendar();
        }

        protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
        {
            if (RequestList[e.Day.Date.ToString("dd/MM/yyyy")] != null)
            {
                Literal literal1 = new Literal();
                literal1.Text = "<br/>" + (string)RequestList[e.Day.Date.ToString("dd/MM/yyyy")];
                e.Cell.Controls.Add(literal1);
            }
        }

        protected void Calendar1_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            UpdateCalendar();
        }

        private void UpdateCalendar()
        {
            int state = ControlUtil.GetSelectedValue(ComboState) ?? 0;
            int type = ControlUtil.GetSelectedValue(ComboType) ?? 0;

            DateTime? date = null;

            if (!string.IsNullOrEmpty(TxtDate.Text))
                date = DateTime.ParseExact(TxtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            string searchKey = TxtSearch.Text;

            RequestList = GetRequests(date, searchKey, state, type);
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            UpdateCalendar();
        }

        protected void BtnHiddenForDate_Click(object sender, EventArgs e)
        {
            UpdateCalendar();
        }

        protected void BtnSaveFER_Click(object sender, EventArgs e)
        {
            bool success = false;

            int reqeustID = ParseUtil.TryParseInt(HfRequestID.Value) ?? 0;
            Request request = requestDAO.FindByID(reqeustID);

            int O = ParseUtil.TryParseInt(TxtFERO.Text) ?? 0;
            int S = ParseUtil.TryParseInt(TxtFERS.Text) ?? 0;
            string A = TxtFERA.Text;

            int state = ControlUtil.GetSelectedValue(ComboFERState) ?? (int)State.Progress;
            if (state == (int)State.Accepted && string.IsNullOrEmpty(A))
            {
                ReqValA.IsValid = false;
                return;
            }

            if (loginSystem.IsEmployerLoggedIn() || loginSystem.IsLogisticLoggedIn() || request == null) return;
            else if (loginSystem.IsStaffLoggedIn())
            {
                request.O = O;
                request.S = S;
                request.A = A;
                request.SState = state;
                request.SCheckDate = DateTime.Now;
                request.SCheckerId = user.Id;

                success = requestDAO.Update(request);

                // Add(if State is Accepted) presence data
                DateTime? startDate = request.FromDate;
                DateTime? endDate = request.ToDate;
                if (startDate != null && endDate != null && state == (int)State.Accepted)
                {
                    for (DateTime date = (startDate ?? DateTime.Now); date <= endDate; date = date.AddDays(1))
                    {
                        // TODO: Before Add, Need to Delete by Date and User???

                        Presence presence = new Presence();
                        presence.Date = date;
                        presence.O = O;
                        presence.S = S;
                        presence.A = A;
                        presence.UserId = request.SenderId;

                        presenceDAO.Insert(presence);
                    }
                }
            }
            else
            {
                request.O = O;
                request.S = S;
                request.A = A;
                request.AState = state;
                request.ACheckDate = DateTime.Now;
                request.ACheckerId = loginSystem.IsCEOLoggedIn() ? (int)ExtraIDs.CEO : (int)ExtraIDs.ADMIN;

                success = requestDAO.Update(request);

                // Add(Accepted) or Remove(Rejected) presence data
                DateTime? startDate = request.FromDate;
                DateTime? endDate = request.ToDate;
                if (startDate != null && endDate != null)
                {
                    if (state != (int)State.Progress)
                    {
                        for (DateTime date = (startDate ?? DateTime.Now); date <= endDate; date = date.AddDays(1))
                        {
                            Presence presence = presenceDAO.FindByDate(date).Where(p => p.UserId == request.SenderId).FirstOrDefault();
                            if (presence != null) presenceDAO.Delete(presence.Id);
                        }
                    }
                    if (state == (int)State.Accepted)
                    {
                        for (DateTime date = (startDate ?? DateTime.Now); date <= endDate; date = date.AddDays(1))
                        {
                            Presence presence = new Presence();
                            presence.Date = date;
                            presence.O = O;
                            presence.S = S;
                            presence.A = A;
                            presence.UserId = request.SenderId;

                            presenceDAO.Insert(presence);
                        }
                    }
                }
            }

            if (success)
            {
                Page.Response.Redirect(Page.Request.Url.ToString(), true);
            }
            else
            {
                ServerValidatorFER.IsValid = false;
                return;
            }
        }

        protected void BtnSaveMAL_Click(object sender, EventArgs e)
        {
            bool success = false;

            int reqeustID = ParseUtil.TryParseInt(HfRequestID.Value) ?? 0;
            Request request = requestDAO.FindByID(reqeustID);

            int O = ParseUtil.TryParseInt(TxtMALO.Text) ?? 0;
            int S = ParseUtil.TryParseInt(TxtMALS.Text) ?? 0;
            string A = TxtMALA.Text;

            int state = ControlUtil.GetSelectedValue(ComboMALState) ?? (int)State.Progress;
            if (state == (int)State.Accepted && string.IsNullOrEmpty(A))
            {
                ReqValAForMAL.IsValid = false;
                return;
            }

            if (loginSystem.IsEmployerLoggedIn() || loginSystem.IsLogisticLoggedIn() || request == null) return;
            else if (loginSystem.IsStaffLoggedIn())
            {
                request.O = O;
                request.S = S;
                request.A = A;
                request.SState = state;
                request.SCheckDate = DateTime.Now;
                request.SCheckerId = user.Id;

                success = requestDAO.Update(request);

                // Add(if State is Accepted) presence data
                DateTime? startDate = request.FromDate;
                DateTime? endDate = request.ToDate;
                if (startDate != null && endDate != null && state == (int)State.Accepted)
                {
                    for (DateTime date = (startDate ?? DateTime.Now); date <= endDate; date = date.AddDays(1))
                    {
                        // TODO: Before Add, Need to Delete by Date and User???

                        Presence presence = new Presence();
                        presence.Date = date;
                        presence.O = O;
                        presence.S = S;
                        presence.A = A;
                        presence.UserId = request.SenderId;

                        presenceDAO.Insert(presence);
                    }
                }
            }
            else
            {
                request.O = O;
                request.S = S;
                request.A = A;
                request.AState = state;
                request.ACheckDate = DateTime.Now;
                request.ACheckerId = loginSystem.IsCEOLoggedIn() ? (int)ExtraIDs.CEO : (int)ExtraIDs.ADMIN;

                success = requestDAO.Update(request);

                // Add(Accepted) or Remove(Rejected) presence data
                DateTime? startDate = request.FromDate;
                DateTime? endDate = request.ToDate;
                if (startDate != null && endDate != null)
                {
                    if (state != (int)State.Progress)
                    {
                        for (DateTime date = (startDate ?? DateTime.Now); date <= endDate; date = date.AddDays(1))
                        {
                            Presence presence = presenceDAO.FindByDate(date).Where(p => p.UserId == request.SenderId).FirstOrDefault();
                            if (presence != null) presenceDAO.Delete(presence.Id);
                        }
                    }
                    if (state == (int)State.Accepted)
                    {
                        for (DateTime date = (startDate ?? DateTime.Now); date <= endDate; date = date.AddDays(1))
                        {
                            Presence presence = new Presence();
                            presence.Date = date;
                            presence.O = O;
                            presence.S = S;
                            presence.A = A;
                            presence.UserId = request.SenderId;

                            presenceDAO.Insert(presence);
                        }
                    }
                }
            }

            if (success)
            {
                Page.Response.Redirect(Page.Request.Url.ToString(), true);
            }
            else
            {
                ServerValidatorMAL.IsValid = false;
                return;
            }
        }

        protected void BtnSaveMAN_Click(object sender, EventArgs e)
        {
            bool success = false;

            int reqeustID = ParseUtil.TryParseInt(HfRequestID.Value) ?? 0;
            Request request = requestDAO.FindByID(reqeustID);

            int state = ControlUtil.GetSelectedValue(ComboMANState) ?? (int)State.Progress;

            if (loginSystem.IsEmployerLoggedIn() || loginSystem.IsLogisticLoggedIn() || request == null) return;
            else if (loginSystem.IsStaffLoggedIn())
            {
                request.SState = state;
                request.SCheckDate = DateTime.Now;
                request.SCheckerId = user.Id;

                success = requestDAO.Update(request);
            }
            else
            {
                request.AState = state;
                request.ACheckDate = DateTime.Now;
                request.ACheckerId = loginSystem.IsCEOLoggedIn() ? (int)ExtraIDs.CEO : (int)ExtraIDs.ADMIN;

                success = requestDAO.Update(request);
            }

            if (success)
            {
                Page.Response.Redirect(Page.Request.Url.ToString(), true);
            }
            else
            {
                ServerValidatorMAN.IsValid = false;
                return;
            }
        }

        protected void BtnSaveDPI_Click(object sender, EventArgs e)
        {
            bool success = false;

            int reqeustID = ParseUtil.TryParseInt(HfRequestID.Value) ?? 0;
            Request request = requestDAO.FindByID(reqeustID);

            int state = ControlUtil.GetSelectedValue(ComboDPIState) ?? (int)State.Progress;

            if (loginSystem.IsEmployerLoggedIn() || loginSystem.IsLogisticLoggedIn() || request == null) return;
            else if (loginSystem.IsStaffLoggedIn())
            {
                request.SState = state;
                request.SCheckDate = DateTime.Now;
                request.SCheckerId = user.Id;

                success = requestDAO.Update(request);
            }
            else
            {
                request.AState = state;
                request.ACheckDate = DateTime.Now;
                request.ACheckerId = loginSystem.IsCEOLoggedIn() ? (int)ExtraIDs.CEO : (int)ExtraIDs.ADMIN;

                success = requestDAO.Update(request);
            }

            if (success)
            {
                Page.Response.Redirect(Page.Request.Url.ToString(), true);
            }
            else
            {
                ServerValidatorDPI.IsValid = false;
                return;
            }
        }

        protected void BtnDPILoad_Click(object sender, EventArgs e)
        {
            int requestID = ParseUtil.TryParseInt(HfRequestID.Value) ?? 0;

            List<DPITypeForRequestCheck> DPITypeForRequestList = new List<DPITypeForRequestCheck>();

            if (requestID == 0)
            {
                DPITypeForRequestList = new List<DPITypeForRequestCheck>();
            }
            else
            {
                DPITypeForRequestList = new RequestController().FindDPITypeForRequest(requestID);
            }
            
            DPIRepeater.DataSource = DPITypeForRequestList;
            DPIRepeater.DataBind();

            UpdateCalendar();
        }
    }
}