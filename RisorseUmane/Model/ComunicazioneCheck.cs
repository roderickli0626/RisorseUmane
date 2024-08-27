using Microsoft.AspNet.SignalR;
using RisorseUmane.Common;
using RisorseUmane.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RisorseUmane.Model
{
    public class ComunicazioneCheck
    {
        Communication comun;

        public ComunicazioneCheck() { }

        public ComunicazioneCheck(Communication comunicazione)
        {
            this.comun = comunicazione;
            if (comunicazione == null) { return; }

            UserDAO userDAO = new UserDAO();

            Id = comun.Id;
            Subject = comun.Subject;
            Description = comun.Description;
            if (comun.SenderId == (int)ExtraIDs.CEO) Sender = "CEO";
            else if (comun.SenderId == (int)ExtraIDs.ADMIN) Sender = "ADMIN";
            else Sender = "STAFF " + userDAO.FindByID(comun.SenderId).Name;
            SenderID = comun.SenderId;

            if (comun.ToRole == (int)Role.CEO) Receiver = "CEO";
            else if (comun.ToRole == (int)Role.Admin) Receiver = "ADMIN";
            else if (comun.ToRole == (int)Role.Staff) Receiver = "All Staffs";
            else if (comun.ToRole == (int)Role.Employer) Receiver = "All Employers";
            else if (comun.ToRole == (int)Role.Logistic) Receiver = "All Logistics";
            else Receiver = userDAO.FindByID(comun.ReceiverId ?? 0)?.Name ?? "";
            ReceiverID = comun.ReceiverId ?? 0;

            CreatedDate = comun.Date?.ToString("dd/MM/yyyy HH.mm");
        }

        public int Id
        {
            get; set;
        }
        public string Description
        {
            get; set;
        }
        public string Subject
        {
            get; set;
        }
        public int SenderID
        {
            get; set;
        }
        public string Sender
        {
            get; set;
        }
        public int ReceiverID
        {
            get; set; 
        }
        public string Receiver
        {
            get; set;
        }
        public string CreatedDate
        {
            get; set;
        }
    }
}