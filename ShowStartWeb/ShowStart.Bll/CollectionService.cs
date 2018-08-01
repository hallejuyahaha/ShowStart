using ShowStart.IBll;
using ShowStart.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStart.Bll
{
    public class CollectionService : BaseService<collection>, ICollectionService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = this.CurrentDBSession.CollectionDal;
        }
    }
}
