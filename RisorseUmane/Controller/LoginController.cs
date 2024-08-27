using RisorseUmane.Common;
using RisorseUmane.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace RisorseUmane.Controller
{
    public class EncryptedPass
    {
        public string Encrypted { get; set; }
        public string UnEncrypted { get; set; }

    }

    public class LoginController
    {
        private UserDAO userDao;
        public LoginController()
        {
            userDao = new UserDAO();
        }

        public LoginCode LoginAndSaveSession(string email, EncryptedPass pass)
        {
            string CEOEmail = System.Configuration.ConfigurationManager.AppSettings["CEOUserName"];
            string CEOPass = System.Configuration.ConfigurationManager.AppSettings["CEOPassword"];

            string AdminEmail = System.Configuration.ConfigurationManager.AppSettings["AdminUserName"];
            string AdminPass = System.Configuration.ConfigurationManager.AppSettings["AdminPassword"];

            if (email.CompareTo(CEOEmail) == 0 && pass.UnEncrypted.CompareTo(CEOPass) == 0)
            {
                new SessionController().SetCEO();
                new SessionController().SetCurrentUserId((int)ExtraIDs.CEO);
                new SessionController().SetCurrentUserEmail(email);
                new SessionController().SetPassword(pass);
                new SessionController().SetTimeout(60 * 24 * 7 * 2);
                return LoginCode.Success;
            }

            if (email.CompareTo(AdminEmail) == 0 && pass.UnEncrypted.CompareTo(AdminPass) == 0)
            {
                new SessionController().SetAdmin();
                new SessionController().SetCurrentUserId((int)ExtraIDs.ADMIN);
                new SessionController().SetCurrentUserEmail(email);
                new SessionController().SetPassword(pass);
                new SessionController().SetTimeout(60 * 24 * 7 * 2);
                return LoginCode.Success;
            }

            User user = userDao.FindByEmail(email);
            if (user == null) { return LoginCode.Failed; }
            string modelPW = new CryptoController().DecryptStringAES(user.Password);
            if (pass.UnEncrypted.CompareTo(modelPW) == 0)
            {
                if (user.Role == (int)Role.Staff)
                {
                    new SessionController().SetStaff();
                }
                else if (user.Role == (int)Role.Employer)
                {
                    new SessionController().SetEmployer();
                }
                else
                {
                    new SessionController().SetLogistic();
                }
                new SessionController().SetCurrentUserId(user.Id);
                new SessionController().SetCurrentUserEmail(user.Email);
                new SessionController().SetPassword(pass);
                new SessionController().SetTimeout(60 * 24 * 7 * 2);

                return LoginCode.Success;
            }
            else
            {
                return LoginCode.Failed;
            }
        }
        public bool IsCEOLoggedIn()
        {
            return new SessionController().GetCEO() == true;
        }
        public bool IsAdminLoggedIn()
        {
            return new SessionController().GetAdmin() == true;
        }
        public bool IsStaffLoggedIn()
        {
            return new SessionController().GetStaff() == true;
        }
        public bool IsEmployerLoggedIn()
        {
            return new SessionController().GetEmployer() == true;
        }
        public bool IsLogisticLoggedIn()
        {
            return new SessionController().GetLogistic() == true;
        }
        public User GetCurrentUserAccount()
        {
            User user = null;
            int? id = new SessionController().GetCurrentUserId();
            if (id == null) return null;
            user = userDao.FindByID(id.Value);
            return user;
        }
    }
}