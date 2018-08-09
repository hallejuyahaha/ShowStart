using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStart.IDal
{
    public interface IBaseDal<T>where T:class,new()
    {
        IQueryable<T> LoadEntities(System.Linq.Expressions.Expression<Func<T,bool>> whereLambda);
        IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderbyLambda, bool isAsc);
        bool DeleteEntity(T entity);
        //bool DeleteEntity(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda);
        bool EditEntity(T entity);
        bool AddEntity(T entity);
        //int DeleteEntityWhere(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda);
    }
}
