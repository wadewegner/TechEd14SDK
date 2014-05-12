using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Salesforce.Force.UnitTests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public async Task Auth_HasApiVersion()
        {
            var auth = new AuthenticationClient();

            Assert.IsNotNull(auth.ApiVersion);
        }
    }
}
