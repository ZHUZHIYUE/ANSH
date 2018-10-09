using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Xunit;
namespace Testing.Server.Unit.ANSH.Common {
    public class ANSHCommonExtensions {
        public enum TestEnum {
            TestValue1 = 0x000,
            TestValue2 = 0x002,
            TestValue3 = 0x004,
            TestValue4 = 0x008,
            TestValue5 = 0x010
        }

        [Fact]
        public async Task Test_IsInt () {
            {
                string value = "-1";
                Assert.True (value.IsInt (out int _out));
                Assert.Equal (-1, _out);
            } {
                string value = "0";
                Assert.True (value.IsInt (out int _out));
                Assert.Equal (0, _out);
            } {
                string value = "1";
                Assert.True (value.IsInt (out int _out));
                Assert.Equal (1, _out);
            } {
                string value = "error_parameter";
                Assert.False (value.IsInt (out int _out));
                Assert.Equal (default (int), _out);
            } {
                string value = "";
                Assert.False (value.IsInt (out int _out));
                Assert.Equal (default (int), _out);
            } {
                string value = null;
                Assert.False (value.IsInt (out int _out));
                Assert.Equal (default (int), _out);
            }
            await Task.CompletedTask;
        }

        [Fact]
        public async Task Test_IsDateTime () {
            {
                string value = "1987-11-12";
                Assert.True (value.IsDateTime ());
            } {
                string value = "error_parameter";
                Assert.False (value.IsDateTime ());
            } {
                string value = null;
                Assert.False (value.IsDateTime ());
            } {
                string value = "1987-11-12";
                Assert.True (value.IsDateTime (out DateTime result));
                Assert.Equal (value.ToDateTime (), result);
            } {
                string value = "error_parameter";
                Assert.False (value.IsDateTime (out DateTime result));
                Assert.Equal (default (DateTime), result);
            } {
                string value = null;
                Assert.False (value.IsDateTime (out DateTime result));
                Assert.Equal (default (DateTime), result);
            } {
                string value = "1987-11-12 00:00:00";
                string format = "yyyy-MM-dd HH:mm:ss";
                Assert.True (value.IsDateTime (out DateTime result, format));
                Assert.Equal (value.ToDateTime (), result);
            } {
                string value = "1987-11-12";
                string format = "yyyy-MM-dd HH:mm:ss";
                Assert.False (value.IsDateTime (out DateTime result, format));
                Assert.Equal (default (DateTime), result);
            } {
                string value = "error_parameter";
                string format = "yyyy-MM-dd HH:mm:ss";
                Assert.False (value.IsDateTime (out DateTime result, format));
                Assert.Equal (default (DateTime), result);
            } {
                string value = null;
                string format = "yyyy-MM-dd HH:mm:ss";
                Assert.False (value.IsDateTime (out DateTime result, format));
                Assert.Equal (default (DateTime), result);
            }
            DateTime lowerlimit = DateTime.Now.AddYears (-1).Date, upperlimit = DateTime.Now.AddYears (1).Date; {
                string value = DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss");
                string format = "yyyy-MM-dd HH:mm:ss";
                Assert.True (value.IsDateTime (out DateTime result, format, lowerlimit, upperlimit));
                Assert.Equal (value.ToDateTime (), result);
            }
            await Task.CompletedTask;
        }

        [Fact]
        public async Task Test_ToTimeStamp () {
            DateTime time = DateTime.Now; {
                Assert.Equal (time.Subtract (new DateTime (1970, 1, 1, 8, 0, 0)).TotalSeconds, time.ToTimeStamp ());
            }

            await Task.CompletedTask;
        }
    }
}