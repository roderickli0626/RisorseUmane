using RisorseUmane.Common;
using RisorseUmane.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;

namespace RisorseUmane.Model
{
    public class MalattiaRequestChecker
    {
        Request request;
        public MalattiaRequestChecker() { }

        public MalattiaRequestChecker(Request request)
        {
            this.request = request;
            if (request == null) return;

            UserDAO userDAO = new UserDAO();
            Id = request.Id;
            Subject = request.Subject;
            Sender = request.User.Name;
            SChecker = userDAO.FindByID(request.SCheckerId ?? 0)?.Name ?? "";
            if (request.ACheckerId == (int)ExtraIDs.CEO) AChecker = "CEO";
            else if (request.ACheckerId == (int)ExtraIDs.ADMIN) AChecker = "ADMIN";
            else AChecker = "";
            RequestType = request.RequestType;
            CreatedDate = request.CreatedDate?.ToString("dd/MM/yyyy HH.mm");
            From = request.FromDate?.ToString("dd/MM/yyyy");
            To = request.ToDate?.ToString("dd/MM/yyyy");
            Description = request.Description;
            SState = request.SState ?? 0;
            AState = request.AState ?? 0;
            O = request.O ?? 0;
            S = request.S ?? 0;
            A = request.A;
            SCheckDate = request.SCheckDate?.ToString("dd/MM/yyyy HH.mm") ?? "";
            ACheckDate = request.ACheckDate?.ToString("dd/MM/yyyy HH.mm") ?? "";

            DPIDescription = string.Join(", ", request.DpiTypeForRequests.Select(d => d.DpiType.Description));
        }

        public int Id
        {
            get; set;
        }
        public string Subject
        {
            get; set;
        }
        public string Sender
        {
            get; set;
        }
        public string SChecker
        {
            get; set;
        }
        public string AChecker
        {
            get; set;
        }
        public int RequestType
        {
            get; set;
        }
        public string CreatedDate
        {
            get; set;
        }
        public string From
        {
            get; set;
        }
        public string To
        {
            get; set;
        }
        public string Description
        {
            get; set;
        }
        public string DPIDescription
        {
            get; set;
        }
        public int SState
        {
            get; set;
        }
        public int AState
        {
            get; set;
        }
        public string A
        {
            get; set;
        }
        public int O
        {
            get; set;
        }
        public int S
        {
            get; set;
        }
        public string SCheckDate
        {
            get; set;
        }
        public string ACheckDate
        {
            get; set;
        }
    }
}