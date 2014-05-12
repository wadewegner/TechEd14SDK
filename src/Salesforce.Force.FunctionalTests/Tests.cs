using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Salesforce.Force.FunctionalTests
{
    [TestFixture]
    public class Tests
    {
        private static string _securityToken = Encoding.UTF8.GetString(Convert.FromBase64String("bGxMUjZZdVd3b2lBOEdFbE5TSXYzTkIzTQ=="));
        private static string _clientId = Encoding.UTF8.GetString(Convert.FromBase64String("M01WRzlKWl9yLlF6clM3aXpYVldyRVRjM3YwTmZCeHdGZ0NheTg3dXY0M0I0TVdEZ1JTVmFUbkdHLnpuMnIwMVlrOHRSazJlYTVtZW1PS2c0aGQyMg=="));
        private static string _clientSecret = Encoding.UTF8.GetString(Convert.FromBase64String("ODQzOTM0NzQ5Njg2MDQzOTgwNg=="));
        private static string _username = "api@dotnetbuild.com";
        private static string _password = Encoding.UTF8.GetString(Convert.FromBase64String("UGEkJHcwcmQh")) + _securityToken;

        [Test]
        public async Task Auth_UsernamePassword_HasToken()
        {
            var auth = new AuthenticationClient();
            await auth.UsernamePasswordAsync(_clientId, _clientSecret, _username, _password);

            Assert.IsNotNull(auth.AccessToken);
        }

        [Test]
        public async Task Query_Accounts_IsNotNull()
        {
            var auth = new AuthenticationClient();
            await auth.UsernamePasswordAsync(_clientId, _clientSecret, _username, _password);

            var client = new ForceClient(auth.InstanceUrl, auth.AccessToken, auth.ApiVersion);

            var results = await client.QueryAsync<dynamic>("SELECT Id, Name, Description FROM Account");

            var accounts = results.records;

            Assert.IsNotNull(accounts);
        }
    }
}
