using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AuthSharp.Models;

namespace AuthSharp.Tests.Controllers
{
    [TestClass]
    public class DataSizeTest
    {
        [TestMethod]
        public void ToStringIsNotNull()
        {
            //var testItems = Enumerable.Range(1, 100000); //new List<int>();
            foreach (var item in Enumerable.Range(-1000000, 10000000))
            {
                DataSize testObj = new DataSize(item);
                Assert.IsNotNull(testObj.ToString());
            }
            
        }

        [TestMethod]
        public void ToStringIsRight()
        {
            var testItem = new[]{
                new {Value = 1, String = "1 B" },
                new {Value = 999, String = "999 B" },
                new {Value = 1000, String = "0.977 KB" },
                new {Value = 1024, String = "1 KB" },
                new {Value = 5000, String = "4.88 KB" },
                new {Value = 500000, String = "488 KB" },
            };
            //Assert.IsTrue(
            //    testItem.All(item => new DataSize(item.Value).ToString() == item.String)
            //);
            foreach (var item in testItem)
            {
                DataSize testObj = new DataSize(item.Value);
                var expected = item.String;
                var actual = testObj.ToString();
                Assert.AreEqual(expected, actual);
            }
        }
    }
}
