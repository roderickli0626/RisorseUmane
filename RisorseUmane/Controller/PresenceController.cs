using RisorseUmane.Common;
using RisorseUmane.DAO;
using RisorseUmane.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RisorseUmane.Controller
{
    public class PresenceController
    {
        PresenceDAO presenceDAO;
        UserDAO userDAO;
        public PresenceController()
        {
            presenceDAO = new PresenceDAO();
            userDAO = new UserDAO();
        }

        public SearchResult Search(int start, int length, string searchVal, DateTime? date)
        {
            SearchResult result = new SearchResult();
            IEnumerable<User> userList = userDAO.FindAll().Where(u => u.Role != (int)Role.Staff);
            if (!string.IsNullOrEmpty(searchVal)) userList = userList.Where(x => x.Name.ToLower().Contains(searchVal.ToLower())).ToList();

            result.TotalCount = userList.Count();
            userList = userList.Skip(start).Take(length);

            if (date == null) date = DateTime.Now.Date;

            List<object> checks = new List<object>();
            foreach (User user in userList)
            {
                Presence presence = presenceDAO.FindByDateAndUser(date ?? DateTime.Now.Date, user.Id);
                if (presence != null) 
                { 
                    checks.Add(new PresenceCheck(presence));
                }
                else
                {
                    PresenceCheck presenceCheck = new PresenceCheck();
                    presenceCheck.Name = user.Name;
                    presenceCheck.Date = date?.ToString("dd/MM/yyyy");
                    presenceCheck.UserId = user.Id;
                    presenceCheck.O = 8;
                    presenceCheck.S = 0;
                    presenceCheck.A = "";
                    checks.Add(presenceCheck);
                }
            }
            result.ResultList = checks;

            return result;
        }

        public bool SavePresence(int presenceID, int O, int S, string A, int userID, DateTime? date)
        {
            Presence presence = presenceDAO.FindById(presenceID);
            if (presence == null)
            {
                presence = new Presence();
                presence.O = O;
                presence.S = S;
                presence.A = A;
                presence.Date = date;
                presence.UserId = userID;
                return presenceDAO.Insert(presence);
            }
            else
            {
                presence.O = O; 
                presence.S = S; 
                presence.A = A;

                return presenceDAO.Update(presence);
            }
        }
    }
}