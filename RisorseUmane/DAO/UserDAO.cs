﻿using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;

namespace RisorseUmane.DAO
{
    public class UserDAO : BasicDAO
    {
        public UserDAO() { }
        public List<User> FindAll()
        {
            return GetContext().Users.ToList();
        }

        public User FindByID(int id)
        {
            return GetContext().Users.Where(u => u.Id == id).FirstOrDefault();
        }

        public User FindByEmail(string email)
        {
            return GetContext().Users.Where(u => u.Email == email).FirstOrDefault();
        }

        public bool Insert(User user)
        {
            GetContext().Users.InsertOnSubmit(user);
            GetContext().SubmitChanges();
            return true;
        }

        public bool Update(User user)
        {
            GetContext().SubmitChanges();
            GetContext().Refresh(RefreshMode.OverwriteCurrentValues, user);
            return true;
        }
        public bool Delete(int id)
        {
            User user = GetContext().Users.SingleOrDefault(u => u.Id == id);
            GetContext().Users.DeleteOnSubmit(user);
            GetContext().SubmitChanges();
            return true;
        }
    }
}