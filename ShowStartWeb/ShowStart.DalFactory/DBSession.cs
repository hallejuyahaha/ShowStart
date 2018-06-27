using ShowStart.Dal;
using ShowStart.IDal;
using ShowStart.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStart.DalFactory
{
    /// <summary>
    /// 数据会话层：工厂类，负责完成所有数据操作实例的创建，然后业务逻辑层通过工厂类来获取Dal的实例
    /// 在数据会话层中提供一个方法，一次完成对多个数据的操作
    /// </summary>
   public class DBSession:IDBSession
    {
        //testEntities Db = new testEntities();
        public DbContext Db
        {
            get { return Dal.DBContextFactory.createDbContext(); }
        }
        private IUserInfoDal _UserinfoDal;
        public IUserInfoDal UserInfoDal
        {
            get
            {
                if (_UserinfoDal == null)
                {
                    //_UserinfoDal = new UserInfoDal();
                    _UserinfoDal = AbstractFactory.createUserInfoDal();//通过抽象工厂创建实例
                }
                return _UserinfoDal;
            }
            set
            {
                _UserinfoDal = value;
            }
        }
        /// <summary>
        /// 连接一次数据库，对多张表进行操作
        /// 工作单元模式
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            return Db.SaveChanges()>0;
        }
    }
}
