using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthSharp.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace AuthSharp.Controllers.Tests
{
    [TestClass()]
    public class UserControllerTests
    {
        [TestMethod()]
        public void LoginTest()
        {
            UserController userController = new UserController();
            Assert.IsNotNull(userController);

        }

        public void LoginTest2() { }

        [TestMethod()]
        public void LoginConfirmedTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void PortalTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void MessageTest()
        {
            throw new NotImplementedException();
        }
    }
}
