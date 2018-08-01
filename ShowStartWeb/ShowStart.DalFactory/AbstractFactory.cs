
using ShowStart.IDal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShowStart.DalFactory
{
    /// <summary>
    /// 通过反射机制创建类的实例
    /// </summary>
    public class AbstractFactory
    {
        private static readonly string AssemblyPath = ConfigurationManager.AppSettings["AssemblyPath"];
        private static readonly string NameSpace = ConfigurationManager.AppSettings["NameSpace"];
        public static IUserInfoDal createUserInfoDal()
        {
            string fullClassName = NameSpace + ".UserInfoDal";
            return CreateInstance(fullClassName) as IUserInfoDal;
        }

        public static IShowStartDal createShowStartDal()
        {
            string fullClassName = NameSpace + ".ShowStartDal";
            return CreateInstance(fullClassName) as IShowStartDal;
        }

        public static ICollectionDal createCollectionDal()
        {
            string fullClassName = NameSpace + ".CollectionDal";
            return CreateInstance(fullClassName) as ICollectionDal;
        }

        private static object CreateInstance(string ClassName)
        {
            var assembly = Assembly.Load(AssemblyPath);
            return assembly.CreateInstance(ClassName);
        }         
    }
}
