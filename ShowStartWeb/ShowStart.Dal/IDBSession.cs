using ShowStart.IDal;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowStart.Model;

namespace ShowStart.Dal
{
    public interface IDBSession
    {
        DbContext Db { get; }
        IUserInfoDal UserInfoDal { get; }
        IShowStartDal ShowStartDal { get; }
        ICollectionDal CollectionDal { get; set; }

        bool SaveChanges();
    }
}
