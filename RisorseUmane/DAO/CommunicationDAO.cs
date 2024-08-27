using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace RisorseUmane.DAO
{
    public class CommunicationDAO : BasicDAO
    {
        public CommunicationDAO() { }

        public List<Communication> FindAll()
        {
            Table<Communication> table = GetContext().Communications;
            return table.ToList();
        }

        public Communication FindByID(int id)
        {
            return GetContext().Communications.Where(c => c.Id == id).FirstOrDefault();
        }

        public List<Communication> FindBySender(int id) 
        {
            return GetContext().Communications.Where(c => c.SenderId == id).ToList();
        }

        public List<Communication> FindByReceiver(int id)
        {
            return GetContext().Communications.Where(c => c.ReceiverId == id).ToList();
        }

        public List<Communication> FindByRole(int role)
        {
            return GetContext().Communications.Where(c => c.ToRole == role).ToList();
        }

        public bool Insert(Communication communication)
        {
            GetContext().Communications.InsertOnSubmit(communication);
            GetContext().SubmitChanges();
            return true;
        }

        public bool Update(Communication communication)
        {
            GetContext().SubmitChanges();
            GetContext().Refresh(RefreshMode.OverwriteCurrentValues, communication);
            return true;
        }

        public bool Delete(int id)
        {
            Communication communication = GetContext().Communications.SingleOrDefault(c => c.Id == id);
            GetContext().Communications.DeleteOnSubmit(communication);
            GetContext().SubmitChanges();
            return true;
        }

    }
}