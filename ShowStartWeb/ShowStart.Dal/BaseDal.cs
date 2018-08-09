using ShowStart.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShowStart.Dal
{
    public class BaseDal<T>where T:class,new()
    {
        //testEntities Db = new testEntities();
        DbContext Db = Dal.DBContextFactory.createDbContext();
        public bool AddEntity(T entity)
        {
            try
            {
                Db.Set<T>().Add(entity);
                //Db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool DeleteEntity(T entity)
        {
            Db.Entry<T>(entity).State = EntityState.Deleted;
            return true;
        }

        //public int DeleteEntityWhere(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda = null)
        //{
        //    var set = Db.Set<T>().AsQueryable();
        //    set = (whereLambda == null) ? set : set.Where(whereLambda);

        //    int i = 0;
        //    foreach (var item in set)
        //    {
        //        DeleteEntity(item);
        //        i++;
        //    }
        //    return i;
        //}

        public bool EditEntity(T entity)
        {
            Db.Entry<T>(entity).State = EntityState.Modified;
            //return Db.SaveChanges() > 0;
            return true;
        }

        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            return Db.Set<T>().Where<T>(whereLambda);
        }

        public IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, s>> orderbyLambda, bool isAsc)
        {
            var temp = Db.Set<T>().Where<T>(whereLambda);
            totalCount = temp.Count();
            if (isAsc)
            {
                temp = temp.OrderBy<T, s>(orderbyLambda).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }
            else
            {
                temp = temp.OrderByDescending<T, s>(orderbyLambda).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }
            return temp;
        }
    }
}
