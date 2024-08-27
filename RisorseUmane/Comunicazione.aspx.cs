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
    public partial class Comunicazione : System.Web.UI.Page
    {
        User user;
        LoginController loginSystem = new LoginController();
        ComunicazioneController controller = new ComunicazioneController();
        protected void Page_Load(object sender, EventArgs e)
        {
            user = loginSystem.GetCurrentUserAccount();
            if (!loginSystem.IsCEOLoggedIn() && !loginSystem.IsAdminLoggedIn() && (user == null))
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (loginSystem.IsCEOLoggedIn()) HfLoginUserID.Value = ((int)ExtraIDs.CEO).ToString();
            else if (loginSystem.IsAdminLoggedIn()) HfLoginUserID.Value = ((int)ExtraIDs.ADMIN).ToString();
            else HfLoginUserID.Value = (user.Id).ToString();

            if (!IsPostBack)
            {
                LoadType();
                SetVisible();
            }
        }

        private void LoadType()
        {
            ComboType.Items.Clear();
            ComboType.Items.Add(new ListItem("TUTTI", "0"));
            ComboType.Items.Add(new ListItem("FROM ME", "1"));
            ComboType.Items.Add(new ListItem("TO ME", "2"));
        }

        private void SetVisible() 
        {
            if (loginSystem.IsCEOLoggedIn()) ForCEO.Visible = true;
            if (loginSystem.IsAdminLoggedIn()) ForAdmin.Visible = true;
            if (loginSystem.IsStaffLoggedIn()) ForStaff.Visible = true;
            if (loginSystem.IsLogisticLoggedIn() || loginSystem.IsEmployerLoggedIn())
            {
                ComboType.Visible = false;
                BtnForStaff.Visible = false;
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;
            string description = TxtDescription.Text;
            if (description.Trim() == "")
            {
                ServerValidatorForDescription.IsValid = false;
                return;
            }

            int? comunicazioneID = ParseUtil.TryParseInt(HfComunicazioneID.Value);

            bool success = controller.SaveComunicazione(comunicazioneID, description);
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

        protected void BtnAddSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;
            string description = TxtAddDescription.Text;
            if (description.Trim() == "")
            {
                CustomValidatorForDescription.IsValid = false;
                return;
            }

            bool success = true;
            if (loginSystem.IsCEOLoggedIn()) 
            {
                if (FromCEOToIndividual.Checked)
                {
                    int? receiverID = ParseUtil.TryParseInt(HfReceiverID.Value);
                    if (receiverID == null)
                    {
                        CustomValidatorForReceiver.IsValid = false;
                        return;
                    }
                    success = success && controller.AddComunicazione(description, (int)ExtraIDs.CEO, 0, receiverID ?? 0);
                }
                if (FromCEOToAdmin.Checked) success = success && controller.AddComunicazione(description, (int)ExtraIDs.CEO, (int)Role.Admin, (int)ExtraIDs.ADMIN);
                if (FromCEOToStaff.Checked) success = success && controller.AddComunicazione(description, (int)ExtraIDs.CEO, (int)Role.Staff, 0);
                if (FromCEOToEmployer.Checked) success = success && controller.AddComunicazione(description, (int)ExtraIDs.CEO, (int)Role.Employer, 0);
                if (FromCEOToLogistic.Checked) success = success && controller.AddComunicazione(description, (int)ExtraIDs.CEO, (int)Role.Logistic, 0);

                if (!FromCEOToAdmin.Checked && !FromCEOToStaff.Checked && !FromCEOToEmployer.Checked && !FromCEOToLogistic.Checked && !FromCEOToIndividual.Checked)
                {
                    CustomValidatorForReceiver.IsValid = false;
                    return;
                }
            }
            if (loginSystem.IsAdminLoggedIn()) 
            {
                if (FromAdminToIndividual.Checked)
                {
                    int? receiverID = ParseUtil.TryParseInt(HfReceiverID.Value);
                    if (receiverID == null)
                    {
                        CustomValidatorForReceiver.IsValid = false;
                        return;
                    }
                    success = success && controller.AddComunicazione(description, (int)ExtraIDs.ADMIN, 0, receiverID ?? 0);
                }
                if (FromAdminToCEO.Checked) success = success && controller.AddComunicazione(description, (int)ExtraIDs.ADMIN, (int)Role.CEO, (int)ExtraIDs.CEO);
                if (FromAdminToStaff.Checked) success = success && controller.AddComunicazione(description, (int)ExtraIDs.ADMIN, (int)Role.Staff, 0);
                if (FromAdminToEmployer.Checked) success = success && controller.AddComunicazione(description, (int)ExtraIDs.ADMIN, (int)Role.Employer, 0);
                if (FromAdminToLogistic.Checked) success = success && controller.AddComunicazione(description, (int)ExtraIDs.ADMIN, (int)Role.Logistic, 0);

                if (!FromAdminToCEO.Checked && !FromAdminToStaff.Checked && !FromAdminToEmployer.Checked && !FromAdminToLogistic.Checked && !FromAdminToIndividual.Checked)
                {
                    CustomValidatorForReceiver.IsValid = false;
                    return;
                }
            }
            if (loginSystem.IsStaffLoggedIn())
            {
                if (FromStaffToIndividual.Checked)
                {
                    int? receiverID = ParseUtil.TryParseInt(HfReceiverID.Value);
                    if (receiverID == null)
                    {
                        CustomValidatorForReceiver.IsValid = false;
                        return;
                    }
                    success = success && controller.AddComunicazione(description, user.Id, 0, receiverID ?? 0);
                }
                if (FromStaffToCEO.Checked) success = success && controller.AddComunicazione(description, user.Id, (int)Role.CEO, (int)ExtraIDs.CEO);
                if (FromStaffToAdmin.Checked) success = success && controller.AddComunicazione(description, user.Id, (int)Role.Admin, (int)ExtraIDs.ADMIN);
                if (FromStaffToEmployer.Checked) success = success && controller.AddComunicazione(description, user.Id, (int)Role.Employer, 0);
                if (FromStaffToLogistic.Checked) success = success && controller.AddComunicazione(description, user.Id, (int)Role.Logistic, 0);
                
                if (!FromStaffToCEO.Checked && !FromStaffToAdmin.Checked && !FromStaffToEmployer.Checked && !FromStaffToLogistic.Checked && !FromStaffToIndividual.Checked)
                {
                    CustomValidatorForReceiver.IsValid = false;
                    return;
                }
            }

            if (success)
            {
                Page.Response.Redirect(Page.Request.Url.ToString(), true);
            }
            else
            {
                CustomValidator.IsValid = false;
                return;
            }
        }
    }
}