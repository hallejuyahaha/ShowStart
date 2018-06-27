using ShowStart.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ShowStart.Dal
{
    /// <summary>
    /// EF中 DBContext 确保线程唯一
    /// </summary>
   public class DBContextFactory
    {
        public static DbContext createDbContext()
        {
            DbContext dbContext = HttpContext.Current.Items["dbContext"] as DbContext;
            if (dbContext == null)
            {
                dbContext = new testEntities();
                HttpContext.Current.Items["dbContext"] = dbContext;
            }
            return dbContext;
        }
    }
}
