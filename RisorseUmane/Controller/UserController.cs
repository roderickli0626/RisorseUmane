using RisorseUmane.Common;
using RisorseUmane.DAO;
using RisorseUmane.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RisorseUmane.Controller
{
    public class UserController
    {
        private UserDAO userDao;

        public UserController()
        {
            userDao = new UserDAO();
        }

        public SearchResult Search(int start, int length, string searchVal)
        {
            SearchResult result = new SearchResult();
            IEnumerable<User> userList = userDao.FindAll();
            if (!string.IsNullOrEmpty(searchVal)) userList = userList.Where(x => x.Name.ToLower().Contains(searchVal.ToLower())).ToList();

            result.TotalCount = userList.Count();
            userList = userList.Skip(start).Take(length);

            List<object> checks = new List<object>();
            foreach (User user in userList)
            {
                UserCheck userCheck = new UserCheck(user);
                checks.Add(userCheck);
            }
            result.ResultList = checks;

            return result;
        }

        public SearchResult SearchForDashboard(int start, int length, string searchVal, DateTime? date)
        {
            SearchResult result = new SearchResult();
            IEnumerable<User> userList = userDao.FindAll().Where(u => u.Role != (int)Role.Staff);
            if (!string.IsNullOrEmpty(searchVal)) userList = userList.Where(x => x.Name.ToLower().Contains(searchVal.ToLower())).ToList();

            result.TotalCount = userList.Count();
            userList = userList.Skip(start).Take(length);

            if (date == null) date = DateTime.Now.Date;

            List<object> checks = new List<object>();
            PresenceDAO presenceDAO = new PresenceDAO();
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

        public bool DeleteUser(int id)
        {
            User item = userDao.FindByID(id);
            if (item == null) return false;

            return userDao.Delete(id);
        }

        public bool SaveUser(int? userID, string name, string email, EncryptedPass pass, string mobile, string surname, int? role)
        {
            User user = userDao.FindByID(userID ?? 0);
            if (user == null)
            {
                User existAdmin = userDao.FindByEmail(email);
                if (existAdmin != null) return false;
                user = new User();
                user.Name = name;
                user.Email = email;
                user.Mobile = mobile;
                user.Role = role;
                user.Surname = surname;
                user.FirstLogin = false;
                user.Password = pass?.Encrypted ?? "";

                return userDao.Insert(user);
            }
            else
            {
                user.Name = name;
                user.Email = email;
                user.Surname = surname;
                user.Mobile = mobile;
                user.Role = role;
                if (pass != null)
                {
                    user.Password = pass.Encrypted;
                }
                return userDao.Update(user);
            }
        }
    }
}