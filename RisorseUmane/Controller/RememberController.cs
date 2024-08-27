using Antlr.Runtime;
using RisorseUmane.Common;
using RisorseUmane.DAO;
using RisorseUmane.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RisorseUmane.Controller
{
    public class RememberController
    {
        RememberDateDAO rememberDateDao;

        public RememberController()
        {
            rememberDateDao = new RememberDateDAO();
        }

        public SearchResult Search(int start, int length, string searchVal, int role, int userId, DateTime? from, DateTime? to)
        {
            SearchResult result = new SearchResult();
            IEnumerable<RememberDate> rememberList = rememberDateDao.FindAll();
            if (role == (int)Role.Staff)
            {
                rememberList = rememberList.Where(r => r.UserId == userId || r.State == (int)State.Progress || r.CheckerID == userId);
            }
            else
            {
                rememberList = rememberList.Where(r => r.UserId == userId);
            }
            if (!string.IsNullOrEmpty(searchVal)) rememberList = rememberList.Where(x => x.Description.ToLower().Contains(searchVal.ToLower())).ToList();

            if (from != null)
                rememberList = rememberList.Where(u => u.Remember >= from.Value);

            if (to != null)
                rememberList = rememberList.Where(u => u.Remember <= to.Value);

            result.TotalCount = rememberList.Count();
            rememberList = rememberList.Skip(start).Take(length);

            List<object> checks = new List<object>();
            foreach (RememberDate rememberDate in rememberList)
            {
                RememeberCheck check = new RememeberCheck(rememberDate);
                checks.Add(check);
            }
            result.ResultList = checks;

            return result;
        }

        public bool DeleteRemember(int id)
        {
            RememberDate item = rememberDateDao.FindByID(id);
            if (item == null) return false;

            return rememberDateDao.Delete(id);
        }

        public bool SaveRemember(int? rememberID, int senderID, int state, DateTime? rememberDate, string description)
        {
            RememberDate remember = rememberDateDao.FindByID(rememberID ?? 0);
            if (remember == null)
            {
                remember = new RememberDate();
                remember.State = state;
                remember.Description = description;
                remember.Remember = rememberDate;
                remember.UserId = senderID;
                remember.Date = DateTime.Now;

                return rememberDateDao.Insert(remember);
            }
            else
            {
                remember.Remember = rememberDate;
                remember.Description= description;
                remember.State= state;
                remember.CheckerID = senderID;

                return rememberDateDao.Update(remember);
            }
        }
    }
}