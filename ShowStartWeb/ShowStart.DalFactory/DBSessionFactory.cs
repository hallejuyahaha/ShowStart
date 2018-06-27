using ShowStart.Dal;
using ShowStart.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ShowStart.DalFactory
{
    public class DBSessionFactory
    {
        public static IDBSession CreateDBSession()
        {
            IDBSession DBSession = HttpContext.Current.Items["IDBSession"] as IDBSession;
            if (DBSession == null)
            {
                DBSession = new DBSession();
                HttpContext.Current.Items["IDBSession"] = DBSession;
            }
            return DBSession;
        }
    }
}
