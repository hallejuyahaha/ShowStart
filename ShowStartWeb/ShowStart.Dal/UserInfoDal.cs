using ShowStart.IDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowStart.Model;
using System.Linq.Expressions;

namespace ShowStart.Dal
{
    public class UserInfoDal : IUserInfoDal
    {
        testEntities Db = new testEntities();
        public bool AddEntity(userinfo entity)
        {
            return true;
        }

        public bool DeleteEntity(userinfo entity)
        {
            throw new NotImplementedException();
        }

        public bool EditEntity(userinfo entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<userinfo> LoadEntities(Expression<Func<userinfo, bool>> whereLambda)
        {
            throw new NotImplementedException();
        }

        public IQueryable<userinfo> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, Expression<Func<userinfo, bool>> whereLambda, Expression<Func<userinfo, s>> orderbyLambda, bool isAsc)
        {
            throw new NotImplementedException();
        }
    }
}
