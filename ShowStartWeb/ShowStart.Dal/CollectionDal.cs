using ShowStart.IDal;
using ShowStart.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStart.Dal
{
    public class CollectionDal:BaseDal<collection>,ICollectionDal
    {
        DbContext Db = Dal.DBContextFactory.createDbContext();
    }
}
