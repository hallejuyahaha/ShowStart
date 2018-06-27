using ShowStart.Bll;
using ShowStart.IBll;
using ShowStart.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStart.Bll
{
    public class UserInfoService : BaseService<userinfo>,IUserInfoService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = this.CurrentDBSession.UserInfoDal;
        }
    }
}
