using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.DAL.UnitOfWork
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }
}
