using project.BL.Facades;
using project.BL.Facades.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.BL.tests
{
    public class UserFacadeTests :  FacadeTestsBase
    {
        private readonly IUserFacade _userFacadeSUT;

        public UserFacadeTests()
        {
            _userFacadeSUT = new UserFacade(UnitOfWorkFactory, UserModelMapper);
        }

    }
}
