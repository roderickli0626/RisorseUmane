using RisorseUmane.Common;
using RisorseUmane.Controller;
using RisorseUmane.DAO;
using RisorseUmane.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace RisorseUmane
{
    /// <summary>
    /// Summary description for DataService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DataService : System.Web.Services.WebService
    {
        LoginController loginSystem = new LoginController();
        private JavaScriptSerializer serializer = new JavaScriptSerializer();

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void FindUsers(int draw, int start, int length, string searchVal)
        {
            User user = loginSystem.GetCurrentUserAccount();
            if (!loginSystem.IsCEOLoggedIn() && !loginSystem.IsAdminLoggedIn()) return;

            UserController userController = new UserController();
            SearchResult searchResult = userController.Search(start, length, searchVal);

            JSDataTable result = new JSDataTable();
            result.data = (IEnumerable<object>)searchResult.ResultList;
            result.draw = draw;
            result.recordsTotal = searchResult.TotalCount;
            result.recordsFiltered = searchResult.TotalCount;

            ResponseJson(result);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void FindDashboardUsers(int draw, int start, int length, string searchVal, string searchDate)
        {
            User user = loginSystem.GetCurrentUserAccount();
            if (!loginSystem.IsCEOLoggedIn() && !loginSystem.IsAdminLoggedIn() && (user == null)) return;

            UserController userController = new UserController();

            DateTime? date = null;

            if (!string.IsNullOrEmpty(searchDate))
                date = DateTime.ParseExact(searchDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            SearchResult searchResult = userController.SearchForDashboard(start, length, searchVal, date);

            JSDataTable result = new JSDataTable();
            result.data = (IEnumerable<object>)searchResult.ResultList;
            result.draw = draw;
            result.recordsTotal = searchResult.TotalCount;
            result.recordsFiltered = searchResult.TotalCount;

            ResponseJson(result);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void FindAllUsers(string term)
        {
            User user = loginSystem.GetCurrentUserAccount();
            if (!loginSystem.IsCEOLoggedIn() && !loginSystem.IsAdminLoggedIn() && !loginSystem.IsStaffLoggedIn()) return;

            List<User> result = new UserDAO().FindAll();
            List<UserCheck> result1 = result.Where(x => x.Name.Contains(term)).Select(r => new UserCheck(r)).ToList();
            ResponseJson(result1);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void DeleteUser(int id)
        {
            //Is Logged in?
            if (!loginSystem.IsCEOLoggedIn() && !loginSystem.IsAdminLoggedIn()) return;

            UserController userController = new UserController();
            bool success = userController.DeleteUser(id);

            ResponseProc(success, "");
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void FindMalattias(int draw, int start, int length, string searchVal, int type)
        {
            User user = loginSystem.GetCurrentUserAccount();
            if (!loginSystem.IsEmployerLoggedIn() && !loginSystem.IsLogisticLoggedIn()) return;

            RequestController requestController = new RequestController();
            SearchResult searchResult = requestController.SearchMalattias(start, length, searchVal, type, user);

            JSDataTable result = new JSDataTable();
            result.data = (IEnumerable<object>)searchResult.ResultList;
            result.draw = draw;
            result.recordsTotal = searchResult.TotalCount;
            result.recordsFiltered = searchResult.TotalCount;

            ResponseJson(result);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void FindAllMalattias(string term)
        {
            User user = loginSystem.GetCurrentUserAccount();
            if (!loginSystem.IsEmployerLoggedIn() && !loginSystem.IsLogisticLoggedIn()) return;

            List<Request> result = new RequestDAO().FindAll().Where(r => r.RequestType == (int)RequestType.Malattia && r.SenderId == user.Id).ToList();
            List<MalattiaRequestChecker> result1 = result.Where(x => x.Description.Contains(term)).Select(r => new MalattiaRequestChecker(r)).ToList();
            ResponseJson(result1);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void DeleteMalattia(int id)
        {
            //Is Logged in?
            if (!loginSystem.IsEmployerLoggedIn() && !loginSystem.IsLogisticLoggedIn()) return;

            RequestController requestController = new RequestController();
            bool success = requestController.DeleteMalattia(id);

            ResponseProc(success, "");
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void FindFeries(int draw, int start, int length, string searchVal, int type)
        {
            User user = loginSystem.GetCurrentUserAccount();
            if (!loginSystem.IsEmployerLoggedIn() && !loginSystem.IsLogisticLoggedIn()) return;

            RequestController requestController = new RequestController();
            SearchResult searchResult = requestController.SearchFeries(start, length, searchVal, type, user);

            JSDataTable result = new JSDataTable();
            result.data = (IEnumerable<object>)searchResult.ResultList;
            result.draw = draw;
            result.recordsTotal = searchResult.TotalCount;
            result.recordsFiltered = searchResult.TotalCount;

            ResponseJson(result);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void FindAllFeries(string term)
        {
            User user = loginSystem.GetCurrentUserAccount();
            if (!loginSystem.IsEmployerLoggedIn() && !loginSystem.IsLogisticLoggedIn()) return;

            List<Request> result = new RequestDAO().FindAll().Where(r => r.RequestType == (int)RequestType.Ferie && r.SenderId == user.Id).ToList();
            List<MalattiaRequestChecker> result1 = result.Where(x => x.Description.Contains(term)).Select(r => new MalattiaRequestChecker(r)).ToList();
            ResponseJson(result1);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void DeleteFerie(int id)
        {
            //Is Logged in?
            if (!loginSystem.IsEmployerLoggedIn() && !loginSystem.IsLogisticLoggedIn()) return;

            RequestController requestController = new RequestController();
            bool success = requestController.DeleteFerie(id);

            ResponseProc(success, "");
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void FindManutenziones(int draw, int start, int length, string searchVal, int type)
        {
            User user = loginSystem.GetCurrentUserAccount();
            if (!loginSystem.IsEmployerLoggedIn() && !loginSystem.IsLogisticLoggedIn()) return;

            RequestController requestController = new RequestController();
            SearchResult searchResult = requestController.SearchManutenziones(start, length, searchVal, type, user);

            JSDataTable result = new JSDataTable();
            result.data = (IEnumerable<object>)searchResult.ResultList;
            result.draw = draw;
            result.recordsTotal = searchResult.TotalCount;
            result.recordsFiltered = searchResult.TotalCount;

            ResponseJson(result);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void FindAllManutenziones(string term)
        {
            User user = loginSystem.GetCurrentUserAccount();
            if (!loginSystem.IsEmployerLoggedIn() && !loginSystem.IsLogisticLoggedIn()) return;

            List<Request> result = new RequestDAO().FindAll().Where(r => r.RequestType == (int)RequestType.Manutenzione && r.SenderId == user.Id).ToList();
            List<MalattiaRequestChecker> result1 = result.Where(x => x.Description.Contains(term)).Select(r => new MalattiaRequestChecker(r)).ToList();
            ResponseJson(result1);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void DeleteManutenzione(int id)
        {
            //Is Logged in?
            if (!loginSystem.IsEmployerLoggedIn() && !loginSystem.IsLogisticLoggedIn()) return;

            RequestController requestController = new RequestController();
            bool success = requestController.DeleteManutenzione(id);

            ResponseProc(success, "");
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void FindDPIMaterials(int draw, int start, int length, string searchVal, int type)
        {
            User user = loginSystem.GetCurrentUserAccount();
            if (!loginSystem.IsEmployerLoggedIn() && !loginSystem.IsLogisticLoggedIn()) return;

            RequestController requestController = new RequestController();
            SearchResult searchResult = requestController.SearchDPIMaterials(start, length, searchVal, type, user);

            JSDataTable result = new JSDataTable();
            result.data = (IEnumerable<object>)searchResult.ResultList;
            result.draw = draw;
            result.recordsTotal = searchResult.TotalCount;
            result.recordsFiltered = searchResult.TotalCount;

            ResponseJson(result);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void FindAllDPIMaterials(string term)
        {
            User user = loginSystem.GetCurrentUserAccount();
            if (!loginSystem.IsEmployerLoggedIn() && !loginSystem.IsLogisticLoggedIn()) return;

            List<Request> result = new RequestDAO().FindAll().Where(r => r.RequestType == (int)RequestType.Dpi && r.SenderId == user.Id).ToList();
            List<MalattiaRequestChecker> result1 = result.Where(x => string.Join(", ", x.DpiTypeForRequests.Select(d => d.DpiType.Description)).Contains(term)).Select(r => new MalattiaRequestChecker(r)).ToList();
            ResponseJson(result1);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void DeleteDPIMaterial(int id)
        {
            //Is Logged in?
            if (!loginSystem.IsEmployerLoggedIn() && !loginSystem.IsLogisticLoggedIn()) return;

            RequestController requestController = new RequestController();
            bool success = requestController.DeleteDPIMaterial(id);

            ResponseProc(success, "");
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void FindComunicaziones(int draw, int start, int length, string searchVal, int type)
        {
            User user = loginSystem.GetCurrentUserAccount();
            if (!loginSystem.IsCEOLoggedIn() && !loginSystem.IsAdminLoggedIn() && (user == null)) return;

            ComunicazioneController controller = new ComunicazioneController();

            int role = 0;
            if (loginSystem.IsCEOLoggedIn()) role = (int)Role.CEO;
            else if (loginSystem.IsAdminLoggedIn()) role = (int)Role.Admin;
            else role = user.Role ?? 0;

            SearchResult searchResult = controller.Search(start, length, searchVal, type, role, user?.Id ?? 0);

            JSDataTable result = new JSDataTable();
            result.data = (IEnumerable<object>)searchResult.ResultList;
            result.draw = draw;
            result.recordsTotal = searchResult.TotalCount;
            result.recordsFiltered = searchResult.TotalCount;

            ResponseJson(result);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void FindAllComunicaziones(string term)
        {
            User user = loginSystem.GetCurrentUserAccount();
            if (!loginSystem.IsCEOLoggedIn() && !loginSystem.IsAdminLoggedIn() && (user == null)) return;

            List<Communication> result = new CommunicationDAO().FindAll();
            if (loginSystem.IsCEOLoggedIn())
            {
                result = result.Where(c => c.SenderId == (int)ExtraIDs.CEO || c.ToRole == (int)Role.CEO).ToList();
            }
            else if (loginSystem.IsAdminLoggedIn())
            {
                result = result.Where(c => c.SenderId == (int)ExtraIDs.ADMIN || c.ToRole == (int)Role.Admin).ToList();
            }
            else result = result.Where(c => c.SenderId == user.Id || c.ToRole == user.Role || c.ReceiverId == user.Id).ToList();
            
            List<ComunicazioneCheck> result1 = result.Where(x => x.Description.Contains(term)).Select(r => new ComunicazioneCheck(r)).ToList();
            ResponseJson(result1);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void DeleteComunicazione(int id)
        {
            User user = loginSystem.GetCurrentUserAccount();
            if (!loginSystem.IsCEOLoggedIn() && !loginSystem.IsAdminLoggedIn() && (user == null)) return;

            ComunicazioneController controller = new ComunicazioneController();
            bool success = controller.DeleteComunicazione(id);

            ResponseProc(success, "");
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void FindRemembers(int draw, int start, int length, string searchVal, string searchFrom, string searchTo)
        {
            User user = loginSystem.GetCurrentUserAccount();
            if (!loginSystem.IsCEOLoggedIn() && !loginSystem.IsAdminLoggedIn() && (user == null)) return;

            RememberController controller = new RememberController();

            int role = 0;
            if (loginSystem.IsCEOLoggedIn()) role = (int)Role.CEO;
            else if (loginSystem.IsAdminLoggedIn()) role = (int)Role.Admin;
            else role = user.Role ?? 0;

            DateTime? from = null;
            DateTime? to = null;

            if (!string.IsNullOrEmpty(searchFrom))
                from = DateTime.ParseExact(searchFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (!string.IsNullOrEmpty(searchTo))
                to = DateTime.ParseExact(searchTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            SearchResult searchResult = controller.Search(start, length, searchVal, role, user?.Id ?? 0, from, to);

            JSDataTable result = new JSDataTable();
            result.data = (IEnumerable<object>)searchResult.ResultList;
            result.draw = draw;
            result.recordsTotal = searchResult.TotalCount;
            result.recordsFiltered = searchResult.TotalCount;

            ResponseJson(result);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void FindAllRemembers(string term)
        {
            User user = loginSystem.GetCurrentUserAccount();
            if (!loginSystem.IsCEOLoggedIn() && !loginSystem.IsAdminLoggedIn() && (user == null)) return;

            List<RememberDate> result = new RememberDateDAO().FindAll();
            if (loginSystem.IsStaffLoggedIn())
            {
                result = result.Where(c => c.UserId == user.Id || c.State == (int)State.Progress || c.CheckerID == user.Id).ToList();
            }
            else result = result.Where(c => c.UserId == user.Id).ToList();

            List<RememeberCheck> result1 = result.Where(x => x.Description.Contains(term)).Select(r => new RememeberCheck(r)).ToList();
            ResponseJson(result1);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void DeleteRemember(int id)
        {
            User user = loginSystem.GetCurrentUserAccount();
            if (!loginSystem.IsCEOLoggedIn() && !loginSystem.IsAdminLoggedIn() && (user == null)) return;

            RememberController controller = new RememberController();
            bool success = controller.DeleteRemember(id);

            ResponseProc(success, "");
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void FindPresences(int draw, int start, int length, string searchVal, string searchDate)
        {
            User user = loginSystem.GetCurrentUserAccount();
            if (!loginSystem.IsStaffLoggedIn()) return;

            PresenceController controller = new PresenceController();

            DateTime? date = null;

            if (!string.IsNullOrEmpty(searchDate))
                date = DateTime.ParseExact(searchDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            SearchResult searchResult = controller.Search(start, length, searchVal, date);

            JSDataTable result = new JSDataTable();
            result.data = (IEnumerable<object>)searchResult.ResultList;
            result.draw = draw;
            result.recordsTotal = searchResult.TotalCount;
            result.recordsFiltered = searchResult.TotalCount;

            ResponseJson(result);
        }

        protected void ResponseJson(Object result)
        {
            HttpResponse Response = Context.Response;
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(serializer.Serialize(result));
        }
        protected void ResponseJson(Object result, bool success)
        {
            HttpResponse Response = Context.Response;
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(serializer.Serialize(result));
            if (success)
            {
                Response.StatusCode = 200;
            }
            Response.Flush();
        }

        protected void ResponseProc(bool success, object data, string message = "")
        {
            ProcResult result = new ProcResult();
            result.success = success;
            result.data = data;
            result.message = message;
            ResponseJson(result, success);
        }

    }
}
