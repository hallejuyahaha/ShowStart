﻿using ShowStart.IDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowStart.Model;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Data;

namespace ShowStart.Dal
{
    public class UserInfoDal:BaseDal<userinfo>, IUserInfoDal
    {
        DbContext Db = Dal.DBContextFactory.createDbContext();
    }
}
