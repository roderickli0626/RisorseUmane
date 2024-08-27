using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace RisorseUmane.DAO
{
    public class RequestDAO : BasicDAO
    {
        public RequestDAO() { }

        public List<Request> FindAll()
        {
            return GetContext().Requests.ToList();
        }

        public Request FindByID(int id)
        {
            return GetContext().Requests.Where(r => r.Id == id).FirstOrDefault();
        }

        public List<Request> FindBySender(int senderId)
        {
            return GetContext().Requests.Where(r => r.SenderId == senderId).ToList();
        }

        public List<Request> FindByType(int type)
        {
            return GetContext().Requests.Where(r => r.RequestType == type).ToList();
        }

        public List<Request> FindBySenderAndType(int senderID, int type)
        {
            return GetContext().Requests.Where(r => r.SenderId == senderID && r.RequestType == type).ToList();
        }

        public bool Insert(Request request)
        {
            GetContext().Requests.InsertOnSubmit(request);
            GetContext().SubmitChanges();
            return true;
        }

        public int InsertWithID(Request request)
        {
            GetContext().Requests.InsertOnSubmit(request);
            GetContext().SubmitChanges();
            return request.Id;
        }

        public bool Update(Request request)
        {
            GetContext().SubmitChanges();
            GetContext().Refresh(RefreshMode.OverwriteCurrentValues, request);
            return true;
        }
        public bool Delete(int id)
        {
            Request request = GetContext().Requests.SingleOrDefault(u => u.Id == id);
            GetContext().Requests.DeleteOnSubmit(request);
            GetContext().SubmitChanges();
            return true;
        }
    }
}