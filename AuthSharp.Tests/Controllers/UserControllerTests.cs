using AuthSharp.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AuthSharp.Controllers.Tests
{
    [TestClass()]
    public class UserControllerTests
    {
        [TestMethod()]
        public void LoginTest()
        {
            UserController userController = new UserController();
            Assert.IsNotNull(userController.Login("","","",""));

        }

        [TestMethod]
        public void LoginConfirmedTest()
        {
            //UserController userController = new UserController();
            //var result = userController.LoginConfirmed("", "", "") as RedirectResult;
            //Assert.AreEqual(result.Url, "");
        }
    }
}
