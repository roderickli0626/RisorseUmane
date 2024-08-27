using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RisorseUmane.Model
{
    public class UserCheck
    {
        User user;
        public UserCheck() { }

        public UserCheck(User user)
        {
            this.user = user;
            if (user == null) return;
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            Surname = user.Surname;
            Mobile = user.Mobile;
            Role = user.Role;
            FirstLogin = user.FirstLogin;
        }

        public int Id
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public string Surname
        {
            get; set;
        }
        public string Email
        {
            get; set;
        }
        public string Mobile
        {
            get; set;
        }
        public int? Role
        {
            get; set;
        }
        public bool? FirstLogin
        {
            get; set;
        }
        public bool Presence
        {
            get; set;
        }
    }
}