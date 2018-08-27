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
        private TestServer _server { get; }
        private HttpClient _client { get; }
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
            var response = await _client.GetAsync ($"/Home/Middleware_UseStatusCode?StatusCode={StatusCode}");
            var body = await response.Content.ReadAsStringAsync ();
            Assert.Equal (StatusCode, (int) response.StatusCode);
            if (StatusCode >= 400 && StatusCode <= 599) {
                Assert.Equal ($"StatusCode：{StatusCode}",
                    body);
            } else {
                Assert.Empty (body);
            }
        }

        [Fact]
        public async Task Test_Middleware_Exception () {
            var response = await _client.GetAsync ($"/Home/Middleware_Exception");
            var body = await response.Content.ReadAsStringAsync ();
            Assert.Equal (500, (int) response.StatusCode);
            Assert.Equal ($"StatusCode：500", body);

        }
    }
}