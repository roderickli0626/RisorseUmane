using Antlr.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;

namespace RisorseUmane.DAO
{
    public class DpiTypeForRequestDAO : BasicDAO
    {
        public DpiTypeForRequestDAO() { }

        public List<DpiTypeForRequest> FindByRequest(int requestID)
        {
            return GetContext().DpiTypeForRequests.Where(d => d.RequestId == requestID).ToList();
        }

        public DpiTypeForRequest FindByRequestAndType(int requestID, int typeID)
        {
            return GetContext().DpiTypeForRequests.Where(d => d.RequestId == requestID && d.DpiTypeId == typeID).FirstOrDefault();
        }

        public bool Insert(DpiTypeForRequest request)
        {
            GetContext().DpiTypeForRequests.InsertOnSubmit(request);
            GetContext().SubmitChanges();
            return true;
        }

        public bool InsertAll(List<DpiTypeForRequest> requests)
        {
            GetContext().DpiTypeForRequests.InsertAllOnSubmit(requests);
            GetContext().SubmitChanges();
            return true;
        }

        public bool Update(DpiTypeForRequest request)
        {
            GetContext().SubmitChanges();
            GetContext().Refresh(RefreshMode.OverwriteCurrentValues, request);
            return true;
        }

        public bool Delete(int id)
        {
            DpiTypeForRequest request = GetContext().DpiTypeForRequests.SingleOrDefault(d => d.Id == id);
            GetContext().DpiTypeForRequests.DeleteOnSubmit(request);
            GetContext().SubmitChanges();
            return true;
        }

        public bool DeleteByRequest(int requestID)
        {
            List<DpiTypeForRequest> reqeusts = GetContext().DpiTypeForRequests.Where(d => d.RequestId == requestID).ToList();
            GetContext().DpiTypeForRequests.DeleteAllOnSubmit(reqeusts);
            GetContext().SubmitChanges();
            return true;
        }
    }
}