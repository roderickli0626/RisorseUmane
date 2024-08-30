using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace RisorseUmane.DAO
{
    public class PresenceDAO : BasicDAO
    {
        public PresenceDAO() { }

        public List<Presence> FindAll()
        {
            return GetContext().Presences.ToList();
        }

        public Presence FindById(int id)
        {
            return GetContext().Presences.Where(p => p.Id == id).FirstOrDefault();
        }

        public List<Presence> FindByUser(int userId)
        {
            return GetContext().Presences.Where(p => p.UserId == userId).ToList();
        }

        public List<Presence> FindByDate(DateTime date)
        {
            return GetContext().Presences.Where(p => p.Date == date).ToList();
        }
        public Presence FindByDateAndUser(DateTime date, int userID)
        {
            return GetContext().Presences.Where(p => p.Date == date && p.UserId == userID).FirstOrDefault();
        }

        public bool Insert(Presence presence)
        {
            GetContext().Presences.InsertOnSubmit(presence);
            GetContext().SubmitChanges();
            return true;
        }

        public bool Update(Presence presence)
        {
            GetContext().SubmitChanges();
            GetContext().Refresh(RefreshMode.OverwriteCurrentValues, presence);
            return true;
        }

        public bool Delete(int id)
        {
            Presence presence = GetContext().Presences.SingleOrDefault(p => p.Id == id);
            GetContext().Presences.DeleteOnSubmit(presence);
            GetContext().SubmitChanges();
            return true;
        }
    }
}