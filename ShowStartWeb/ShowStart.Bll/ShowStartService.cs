using ShowStart.IBll;
using ShowStart.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStart.Bll
{
    public class ShowStartService : BaseService<showstart>, IShowStartService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = this.CurrentDBSession.ShowStartDal;
        }
    }
}
