using RisorseUmane.Common;
using RisorseUmane.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace RisorseUmane.Model
{
    public class RememeberCheck
    {
        RememberDate rememberDate;

        public RememeberCheck() { }
        public RememeberCheck(RememberDate remember)
        {
            this.rememberDate = remember;
            if (rememberDate == null) { return; }

            UserDAO userDAO = new UserDAO();

            Id = rememberDate.Id;
            Remember = rememberDate.Remember?.ToString("dd/MM/yyyy");
            CreatedDate = rememberDate.Date?.ToString("dd/MM/yyyy HH.mm");
            Description = rememberDate.Description;
            State = rememberDate.State;
            UserID = rememberDate.UserId;
            if (rememberDate.UserId == (int)ExtraIDs.CEO) UserName = "CEO";
            else if (rememberDate.UserId == (int)ExtraIDs.ADMIN) UserName = "ADMIN";
            else UserName = userDAO.FindByID(rememberDate.UserId ?? 0).Name;

            CheckerID = rememberDate.CheckerID;
            Checker = userDAO.FindByID(rememberDate.CheckerID ?? 0)?.Name ?? "";
        }

        public int Id
        {
            get; set;
        }
        public string Description
        {
            get; set;
        }
        public string Remember
        {
            get; set;
        }
        public int? State
        {
            get; set;
        }
        public string CreatedDate
        {
            get; set;
        }
        public int? UserID
        {
            get; set; 
        }
        public string UserName
        {
            get; set;
        }
        public int? CheckerID
        {
            get; set;
        }
        public string Checker
        {
            get; set;
        }
    }
}