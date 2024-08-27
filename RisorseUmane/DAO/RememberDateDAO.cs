using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace RisorseUmane.DAO
{
    public class RememberDateDAO : BasicDAO
    {
        public RememberDateDAO() { }

        public List<RememberDate> FindAll()
        {
            return GetContext().RememberDates.ToList();
        }
        public RememberDate FindByID(int id)
        {
            return GetContext().RememberDates.Where(r => r.Id == id).FirstOrDefault();
        }

        public List<RememberDate> FindByUser(int userId) 
        { 
            return GetContext().RememberDates.Where(r => r.UserId == userId).ToList();
        }

        public bool Insert(RememberDate rememberDate)
        {
            GetContext().RememberDates.InsertOnSubmit(rememberDate);
            GetContext().SubmitChanges();
            return true;
        }

        public bool Update(RememberDate rememberDate)
        {
            GetContext().SubmitChanges();
            GetContext().Refresh(RefreshMode.OverwriteCurrentValues, rememberDate);
            return true;
        }
        public bool Delete(int id)
        {
            RememberDate place = GetContext().RememberDates.SingleOrDefault(u => u.Id == id);
            GetContext().RememberDates.DeleteOnSubmit(place);
            GetContext().SubmitChanges();
            return true;
        }
    }
}