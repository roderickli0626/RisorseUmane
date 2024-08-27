using RisorseUmane.Common;
using RisorseUmane.DAO;
using RisorseUmane.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace RisorseUmane.Controller
{
    public class RequestController
    {
        private RequestDAO requestDAO;

        public RequestController()
        {
            requestDAO = new RequestDAO();
        }
        public SearchResult SearchMalattias(int start, int length, string searchVal, int type)
        {
            SearchResult result = new SearchResult();
            IEnumerable<Request> requestList = requestDAO.FindAll().Where(r => r.RequestType == (int)RequestType.Malattia);
            if (!string.IsNullOrEmpty(searchVal)) requestList = requestList.Where(x => x.Description.ToLower().Contains(searchVal.ToLower())).ToList();
            if (type != 0) requestList = requestList.Where(x => ((x.SState == type) && x.AState == (int)State.Progress) || ((x.AState == type) && x.AState != (int)State.Progress)).ToList();

            result.TotalCount = requestList.Count();
            requestList = requestList.Skip(start).Take(length);

            List<object> checks = new List<object>();
            foreach (Request request in requestList)
            {
                MalattiaRequestChecker malattiacheck = new MalattiaRequestChecker(request);
                checks.Add(malattiacheck);
            }
            result.ResultList = checks;

            return result;
        }



        public bool SaveMalattiaRequest(int? requestID, int senderID, DateTime? sdate, DateTime? edate, string description)
        {
            Request request = requestDAO.FindByID(requestID ?? 0);
            if (request == null)
            {
                request = new Request();
                request.SenderId = senderID;
                request.CreatedDate = DateTime.Now;
                request.Description = description;
                request.FromDate = sdate;
                request.ToDate = edate;
                request.Subject = "ILLNESS REQUEST";
                request.RequestType = (int)RequestType.Malattia;
                request.SState = (int)State.Progress;
                request.AState = (int)State.Progress;

                return requestDAO.Insert(request);
            }
            else
            {
                request.CreatedDate = DateTime.Now;
                request.Description = description;
                request.FromDate = sdate;
                request.ToDate = edate;

                return requestDAO.Update(request);
            }
        }
        
        public bool DeleteMalattia(int id)
        {
            Request item = requestDAO.FindByID(id);
            if (item == null) return false;

            return requestDAO.Delete(id);
        }

        // Ferie Modulo
        public SearchResult SearchFeries(int start, int length, string searchVal, int type)
        {
            SearchResult result = new SearchResult();
            IEnumerable<Request> requestList = requestDAO.FindAll().Where(r => r.RequestType == (int)RequestType.Ferie);
            if (!string.IsNullOrEmpty(searchVal)) requestList = requestList.Where(x => x.Description.ToLower().Contains(searchVal.ToLower())).ToList();
            if (type != 0) requestList = requestList.Where(x => ((x.SState == type) && x.AState == (int)State.Progress) || ((x.AState == type) && x.AState != (int)State.Progress)).ToList();

            result.TotalCount = requestList.Count();
            requestList = requestList.Skip(start).Take(length);

            List<object> checks = new List<object>();
            foreach (Request request in requestList)
            {
                MalattiaRequestChecker malattiacheck = new MalattiaRequestChecker(request);
                checks.Add(malattiacheck);
            }
            result.ResultList = checks;

            return result;
        }
        public bool SaveFerieRequest(int? requestID, int senderID, DateTime? sdate, DateTime? edate, string description)
        {
            Request request = requestDAO.FindByID(requestID ?? 0);
            if (request == null)
            {
                request = new Request();
                request.SenderId = senderID;
                request.CreatedDate = DateTime.Now;
                request.Description = description;
                request.FromDate = sdate;
                request.ToDate = edate;
                request.Subject = "HOLIDAY REQUEST";
                request.RequestType = (int)RequestType.Ferie;
                request.SState = (int)State.Progress;
                request.AState = (int)State.Progress;

                return requestDAO.Insert(request);
            }
            else
            {
                request.CreatedDate = DateTime.Now;
                request.Description = description;
                request.FromDate = sdate;
                request.ToDate = edate;

                return requestDAO.Update(request);
            }
        }

        public bool DeleteFerie(int id)
        {
            Request item = requestDAO.FindByID(id);
            if (item == null) return false;

            return requestDAO.Delete(id);
        }

        // Manutenzione Modulo
        public SearchResult SearchManutenziones(int start, int length, string searchVal, int type)
        {
            SearchResult result = new SearchResult();
            IEnumerable<Request> requestList = requestDAO.FindAll().Where(r => r.RequestType == (int)RequestType.Manutenzione);
            if (!string.IsNullOrEmpty(searchVal)) requestList = requestList.Where(x => x.Description.ToLower().Contains(searchVal.ToLower())).ToList();
            if (type != 0) requestList = requestList.Where(x => ((x.SState == type) && x.AState == (int)State.Progress) || ((x.AState == type) && x.AState != (int)State.Progress)).ToList();

            result.TotalCount = requestList.Count();
            requestList = requestList.Skip(start).Take(length);

            List<object> checks = new List<object>();
            foreach (Request request in requestList)
            {
                MalattiaRequestChecker malattiacheck = new MalattiaRequestChecker(request);
                checks.Add(malattiacheck);
            }
            result.ResultList = checks;

            return result;
        }
        public bool SaveManutenzioneRequest(int? requestID, int senderID, string description)
        {
            Request request = requestDAO.FindByID(requestID ?? 0);
            if (request == null)
            {
                request = new Request();
                request.SenderId = senderID;
                request.CreatedDate = DateTime.Now;
                request.Description = description;
                request.Subject = "MANUTENZIONE REQUEST";
                request.RequestType = (int)RequestType.Manutenzione;
                request.SState = (int)State.Progress;
                request.AState = (int)State.Progress;

                return requestDAO.Insert(request);
            }
            else
            {
                request.CreatedDate = DateTime.Now;
                request.Description = description;

                return requestDAO.Update(request);
            }
        }

        public bool DeleteManutenzione(int id)
        {
            Request item = requestDAO.FindByID(id);
            if (item == null) return false;

            return requestDAO.Delete(id);
        }

        // DPI/Material Modulo
        public SearchResult SearchDPIMaterials(int start, int length, string searchVal, int type)
        {
            SearchResult result = new SearchResult();
            IEnumerable<Request> requestList = requestDAO.FindAll().Where(r => r.RequestType == (int)RequestType.Dpi);
            if (!string.IsNullOrEmpty(searchVal)) requestList = requestList.Where(x => string.Join(", ", x.DpiTypeForRequests.Select(d => d.DpiType.Description)).ToLower().Contains(searchVal.ToLower())).ToList();
            if (type != 0) requestList = requestList.Where(x => ((x.SState == type) && x.AState == (int)State.Progress) || ((x.AState == type) && x.AState != (int)State.Progress)).ToList();

            result.TotalCount = requestList.Count();
            requestList = requestList.Skip(start).Take(length);

            List<object> checks = new List<object>();
            foreach (Request request in requestList)
            {
                MalattiaRequestChecker malattiacheck = new MalattiaRequestChecker(request);
                checks.Add(malattiacheck);
            }
            result.ResultList = checks;

            return result;
        }
        public bool SaveDPIMaterialRequest(int? requestID, int senderID, string description, List<DPITypeForRequestCheck> DPIs)
        {
            Request request = requestDAO.FindByID(requestID ?? 0);
            if (request == null)
            {
                request = new Request();
                request.SenderId = senderID;
                request.CreatedDate = DateTime.Now;
                request.Description = description;
                request.Subject = "MATERIAL REQUEST/DPI";
                request.RequestType = (int)RequestType.Dpi;
                request.SState = (int)State.Progress;
                request.AState = (int)State.Progress;

                int addedRequestID = requestDAO.InsertWithID(request);
                return AddDPITypeForRequest(addedRequestID, DPIs);
            }
            else
            {
                request.CreatedDate = DateTime.Now;
                request.Description = description;

                requestDAO.Update(request);
                return AddDPITypeForRequest(request.Id, DPIs);
            }
        }

        private bool AddDPITypeForRequest(int requestID, List<DPITypeForRequestCheck> DPIs)
        {
            List<DpiTypeForRequest> list = new List<DpiTypeForRequest>();
            foreach(DPITypeForRequestCheck check in DPIs)
            {
                DpiTypeForRequest dpiTypeForRequest = new DpiTypeForRequest();
                dpiTypeForRequest.DpiTypeId = check.DPITypeId;
                dpiTypeForRequest.Size = check.Size;
                dpiTypeForRequest.RequestId = requestID;

                list.Add(dpiTypeForRequest);
            }

            DpiTypeForRequestDAO dpiDao = new DpiTypeForRequestDAO();
            dpiDao.DeleteByRequest(requestID);
            return dpiDao.InsertAll(list);
        }

        public bool DeleteDPIMaterial(int id)
        {
            Request item = requestDAO.FindByID(id);
            if (item == null) return false;

            return requestDAO.Delete(id);
        }

        public List<DPITypeForRequestCheck> FindDPITypeForRequest(int requestID)
        {
            List<DpiTypeForRequest> dpiList = new DpiTypeForRequestDAO().FindByRequest(requestID);
            List<DPITypeForRequestCheck> result = new List<DPITypeForRequestCheck>();
            foreach(DpiTypeForRequest dpiType in dpiList)
            {
                result.Add(new DPITypeForRequestCheck(dpiType));
            }
            return result;
        }
    }
}