using ShowStart.Dal;
using ShowStart.IDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStart.IBll
{
    public interface IBaseService<T> where T : class, new()
    {
        IDBSession CurrentDBSession { get; }
        IBaseDal<T> CurrentDal { get; set; }
        IQueryable<T> LoadEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda);
        IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderbyLambda, bool isAsc);
        bool DeleteEntity(T entity);
        //int DeleteEntityWhere(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda = null);
        bool EditEntity(T entity);
        bool AddEntity(T entity);
    }
}
