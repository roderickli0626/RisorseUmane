using Newtonsoft.Json;
using RisorseUmane.Common;
using RisorseUmane.Controller;
using RisorseUmane.DAO;
using RisorseUmane.Model;
using RisorseUmane.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebGrease.Css.Ast.Selectors;

namespace RisorseUmane
{
    public partial class DPI : System.Web.UI.Page
    {
        User user;
        LoginController loginSystem = new LoginController();
        private List<DPITypeForRequestCheck> DPITypeForRequestList = new List<DPITypeForRequestCheck>();
        RequestController requestController = new RequestController();
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
                LoadDPI();
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

        private void LoadDPI()
        {
            List<DpiType> dpiTypes = new List<DpiType>();
            DpiTypeDAO dpiTypeDAO = new DpiTypeDAO();
            dpiTypes = dpiTypeDAO.FindAll();
            ControlUtil.DataBind(ComboDPI, dpiTypes, "Id", "Description");
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;
            int senderID = user.Id;

            int? requestID = ParseUtil.TryParseInt(HfDPIMaterialID.Value);

            DPITypeForRequestList = JsonConvert.DeserializeObject<List<DPITypeForRequestCheck>>(HfDPIAlloc.Value) ?? new List<DPITypeForRequestCheck>();

            bool success = new RequestController().SaveDPIMaterialRequest(requestID, senderID, "", DPITypeForRequestList);
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

        protected void BtnDPI_Click(object sender, EventArgs e)
        {
            int? DPITypeID = ControlUtil.GetSelectedValue(ComboDPI);
            string Size = TxtSize.Text;

            if (DPITypeID == null || Size.Trim() == "")
            {
                ServerValidator1.IsValid = false;
                return;
            }

            DPITypeForRequestCheck addDPI = new DPITypeForRequestCheck();
            addDPI.DPITypeId = DPITypeID ?? 0;
            addDPI.Size = Size;
            addDPI.Description = new DpiTypeDAO().FindByID(DPITypeID ?? 0)?.Description ?? "";

            DPITypeForRequestList = JsonConvert.DeserializeObject<List<DPITypeForRequestCheck>>(HfDPIAlloc.Value) ?? new List<DPITypeForRequestCheck>();
            DPITypeForRequestList.Remove(DPITypeForRequestList.Find(d => d.DPITypeId == DPITypeID));
            DPITypeForRequestList.Add(addDPI);
            string jsonString = JsonConvert.SerializeObject(DPITypeForRequestList);
            HfDPIAlloc.Value = jsonString;

            DPIRepeater.DataSource = DPITypeForRequestList;
            DPIRepeater.DataBind();
        }

        protected void DPIRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                if (HfIsView.Value != "") return;

                int DPIId = 0;
                if (int.TryParse(e.CommandArgument.ToString(), out DPIId))
                {
                    DPITypeForRequestList = JsonConvert.DeserializeObject<List<DPITypeForRequestCheck>>(HfDPIAlloc.Value);
                    DPITypeForRequestList.Remove(DPITypeForRequestList.Find(s => s.DPITypeId == DPIId));
                    string jsonString = JsonConvert.SerializeObject(DPITypeForRequestList);
                    HfDPIAlloc.Value = jsonString;

                    DPIRepeater.DataSource = DPITypeForRequestList;
                    DPIRepeater.DataBind();
                }
            }
        }

        private void LoadDPITable()
        {
            int requestID = ParseUtil.TryParseInt(HfDPIMaterialID.Value) ?? 0;

            if (requestID == 0)
            {
                DPITypeForRequestList = new List<DPITypeForRequestCheck>();
            }
            else
            {
                DPITypeForRequestList = requestController.FindDPITypeForRequest(requestID);
            }
            string jsonString = JsonConvert.SerializeObject(DPITypeForRequestList);
            HfDPIAlloc.Value = jsonString;

            DPIRepeater.DataSource = DPITypeForRequestList;
            DPIRepeater.DataBind();
        }

        protected void BtnDPILoad_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "Key", "MyFun()", true);

            LoadDPITable();
        }
    }
}