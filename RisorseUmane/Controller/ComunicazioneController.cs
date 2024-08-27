using Microsoft.Ajax.Utilities;
using RisorseUmane.Common;
using RisorseUmane.DAO;
using RisorseUmane.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RisorseUmane.Controller
{
    public class ComunicazioneController
    {
        CommunicationDAO communicationDAO;

        public ComunicazioneController ()
        {
            communicationDAO = new CommunicationDAO ();
        }

        public SearchResult Search(int start, int length, string searchVal, int type, int role, int userId)
        {
            SearchResult result = new SearchResult();
            IEnumerable<Communication> communicationList = communicationDAO.FindAll();
            if (role == (int)Role.CEO)
            {
                if (type == 1) communicationList = communicationList.Where(c => c.SenderId == (int)ExtraIDs.CEO);
                else if (type == 2) communicationList = communicationList.Where(c => c.ToRole == (int)Role.CEO);
                else communicationList = communicationList.Where(c => c.SenderId == (int)ExtraIDs.CEO || c.ToRole == (int)Role.CEO);
            }
            else if (role == (int)Role.Admin)
            {
                if (type == 1) communicationList = communicationList.Where(c => c.SenderId == (int)ExtraIDs.ADMIN);
                else if (type == 2) communicationList = communicationList.Where(c => c.ToRole == (int)Role.Admin);
                else communicationList = communicationList.Where(c => c.SenderId == (int)ExtraIDs.ADMIN || c.ToRole == (int)Role.Admin);
            }
            else
            {
                if (type == 1) communicationList = communicationList.Where(c => c.SenderId == userId);
                else if (type == 2) communicationList = communicationList.Where(c => c.ToRole == role || c.ReceiverId == userId);
                else communicationList = communicationList.Where(c => c.SenderId == userId || c.ToRole == role || c.ReceiverId == userId);
            }
            if (!string.IsNullOrEmpty(searchVal)) communicationList = communicationList.Where(x => x.Description.ToLower().Contains(searchVal.ToLower())).ToList();

            result.TotalCount = communicationList.Count();
            communicationList = communicationList.Skip(start).Take(length);

            List<object> checks = new List<object>();
            foreach (Communication communication in communicationList)
            {
                ComunicazioneCheck check = new ComunicazioneCheck(communication);
                checks.Add(check);
            }
            result.ResultList = checks;

            return result;
        }

        public bool DeleteComunicazione(int id)
        {
            Communication item = communicationDAO.FindByID(id);
            if (item == null) return false;

            return communicationDAO.Delete(id);
        }

        public bool SaveComunicazione(int? comunicazioneID, string description)
        {
            Communication communication = communicationDAO.FindByID(comunicazioneID ?? 0);
            if (communication == null)
            {
                return false;
            }
            else
            {
                communication.Description = description;
                return communicationDAO.Update(communication);
            }
        }

        public bool AddComunicazione(string description, int senderID, int toRole, int receiverID)
        {
            Communication communication = new Communication();
            communication.Description = description;
            communication.SenderId = senderID;
            communication.ToRole = toRole;
            communication.ReceiverId = receiverID;
            communication.Date = DateTime.Now;
            communication.Subject = "COMUNICAZIONE";

            return communicationDAO.Insert(communication);
        }
    }
}