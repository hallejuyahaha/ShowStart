using ShowStart.Dal;
using ShowStart.DalFactory;
using ShowStart.IDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStart.Bll
{
    public abstract class BaseService<T> where T:class,new()
    {
        public IDBSession CurrentDBSession
        {
            get { return DBSessionFactory.CreateDBSession(); }
        }
        public IBaseDal<T> CurrentDal { get; set; }
        public abstract void SetCurrentDal();
        public BaseService()
        {
            SetCurrentDal();
        }
        public IQueryable<T> LoadEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {
            return CurrentDal.LoadEntities(whereLambda);
        }
        public IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderbyLambda, bool isAsc)
        {
            return CurrentDal.LoadPageEntities<s>(pageIndex, pageSize, out totalCount, whereLambda, orderbyLambda, isAsc);
        }
        public bool DeleteEntity(T entity)
        {
             CurrentDal.DeleteEntity(entity);
            return CurrentDBSession.SaveChanges();
        }
        public bool EditEntity(T entity)
        {
             CurrentDal.EditEntity(entity);
            return CurrentDBSession.SaveChanges();
        }
        public bool AddEntity(T entity)
        {
            CurrentDal.AddEntity(entity);
            return CurrentDBSession.SaveChanges();
        }
    }
}
