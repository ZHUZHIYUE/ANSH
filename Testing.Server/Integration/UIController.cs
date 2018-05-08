using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Xunit;
namespace Testing.Server.Unit.ANSH.Common {
    public class UIController {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        public UIController () {
            _server = new TestServer (new WebHostBuilder ()
                .UseStartup<UI.Startup> ());
            _client = _server.CreateClient ();
        }

        [Theory]
        [InlineData (500)]
        [InlineData (400)]
        [InlineData (200)]
        public async Task Test_Middleware_UseStatusCode (int StatusCode) {
            var response = await _client.GetAsync ($"/Middleware_UseStatusCode/?StatusCode={StatusCode}");
            var body = await response.Content.ReadAsStringAsync ();
            Assert.Equal (StatusCode, (int) response.StatusCode);
            if (StatusCode >= 400 && StatusCode <= 599) {
                Assert.Equal ($"StatusCode：{StatusCode}",
                    body);
            } else {
                Assert.Empty (body);
            }
        }
    }
}