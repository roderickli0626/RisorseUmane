using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RisorseUmane.DAO
{
    public class DpiTypeDAO : BasicDAO
    {
        public DpiTypeDAO() { }

        public DpiType FindByID(int id)
        {
            return GetContext().DpiTypes.Where(d => d.Id == id).FirstOrDefault();
        }

        public List<DpiType> FindAll()
        {
            return GetContext().DpiTypes.ToList();
        }
    }
}