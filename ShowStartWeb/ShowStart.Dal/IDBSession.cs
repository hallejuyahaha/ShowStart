using ShowStart.IDal;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStart.Dal
{
    public interface IDBSession
    {
         DbContext Db { get; }
         IUserInfoDal UserInfoDal { get; }
         bool SaveChanges();
    }
}
