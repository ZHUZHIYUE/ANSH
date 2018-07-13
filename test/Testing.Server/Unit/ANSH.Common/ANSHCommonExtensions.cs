using System;
using System.Collections.Generic;
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

        #region Test_ToInt
        public static IEnumerable<object[]> Test_ToInt_param () {
            yield return new object[] { "-1" };
            yield return new object[] { "-1", 1 };
            yield return new object[] { "0" };
            yield return new object[] { "0", 1 };
            yield return new object[] { "1" };
            yield return new object[] { "1", 1 };
            yield return new object[] { "error_parameter" };
            yield return new object[] { "error_parameter", 1 };
            yield return new object[] { "" };
            yield return new object[] { "", 1 };
            yield return new object[] { null };
            yield return new object[] { null, 1 };
        }

        [Theory]
        [MemberData (nameof (Test_ToInt_param))]
        public async Task Test_ToInt (string value, int? default_value = null) {
            var parse_success = int.TryParse (value, out int parse_result);
            if (parse_success) {
                Assert.Equal (parse_result, value.ToInt (default_value));
            } else {
                if (default_value != null) {
                    Assert.Equal (default_value, value.ToInt (default_value));
                } else {
                    Assert.Throws<FormatException> (() => value.ToInt (default_value));
                }
            }

            await Task.CompletedTask;
        }
        #endregion

        #region Test_IsInt
        [Theory]
        [InlineData]
        public async Task Test_IsInt () {
            {
                string value = "-1";
                Assert.True (value.IsInt (out int? _out));
                Assert.Equal (_out, -1);
            } {
                string value = "0";
                Assert.True (value.IsInt (out int? _out));
                Assert.Equal (_out, 0);
            } {
                string value = "1";
                Assert.True (value.IsInt (out int? _out));
                Assert.Equal (_out, 1);
            } {
                string value = "error_parameter";
                Assert.False (value.IsInt (out int? _out));
                Assert.Null (_out);
            } {
                string value = "";
                Assert.False (value.IsInt (out int? _out));
                Assert.Null (_out);
            } {
                string value = null;
                Assert.False (value.IsInt (out int? _out));
                Assert.Null (_out);
            }
            await Task.CompletedTask;
        }
        #endregion

        #region Test_ToEnum_Value
        public static IEnumerable<object[]> Test_ToEnum_param_value () {
            yield return new object[] { 1 };
            yield return new object[] { 1, TestEnum.TestValue1 };
            yield return new object[] { 0x002 };
            yield return new object[] { 0x002, TestEnum.TestValue1 };
        }

        [Theory]
        [MemberData (nameof (Test_ToEnum_param_value))]
        public async Task Test_ToEnum_Value (int value, TestEnum? default_value = null) {
            var parse_success = Enum.TryParse (value.ToString (), out TestEnum parse_result) && Enum.IsDefined (typeof (TestEnum), parse_result);
            if (parse_success) {
                Assert.Equal (parse_result, value.ToEnum (default_value));
            } else {
                if (default_value != null) {
                    Assert.Equal (default_value, value.ToEnum (default_value));
                } else {
                    Assert.Throws<FormatException> (() => value.ToEnum (default_value));
                }
            }
            await Task.CompletedTask;
        }
        #endregion

        #region Test_ToEnum_Names
        public static IEnumerable<object[]> Test_ToEnum_param_names () {
            yield return new object[] { "TestValue1" };
            yield return new object[] { "TestValue1", TestEnum.TestValue2 };
            yield return new object[] { "TestValue2" };
            yield return new object[] { "TestValue2", TestEnum.TestValue1 };
        }

        [Theory]
        [MemberData (nameof (Test_ToEnum_param_names))]
        public async Task Test_ToEnum_Names (string value, TestEnum? default_value = null) {
            var parse_success = Enum.TryParse (value.ToString (), out TestEnum parse_result) && Enum.IsDefined (typeof (TestEnum), parse_result);
            if (parse_success) {
                Assert.Equal (parse_result, value.ToEnum (default_value));
            } else {
                if (default_value != null) {
                    Assert.Equal (default_value, value.ToEnum (default_value));
                } else {
                    Assert.Throws<FormatException> (() => value.ToEnum (default_value));
                }
            }
            await Task.CompletedTask;
        }
        #endregion

        #region Test_IsEnum_Value
        public static IEnumerable<object[]> Test_IsEnum_param_value () {
            yield return new object[] { 1 };
            yield return new object[] { 0x002 };
        }

        [Theory]
        [MemberData (nameof (Test_IsEnum_param_value))]
        public async Task Test_IsEnum_Value (int value) {
            var parse_success = Enum.TryParse (value.ToString (), out TestEnum parse_result) && Enum.IsDefined (typeof (TestEnum), parse_result);

            if (parse_success) {
                Assert.True (value.IsEnum (out TestEnum? result));
                Assert.Equal (parse_result, result);
            } else {
                Assert.False (value.IsEnum (out TestEnum? result));
                Assert.Null (result);
            }
            await Task.CompletedTask;
        }
        #endregion

        #region Test_IsEnum_Names
        public static IEnumerable<object[]> Test_IsEnum_param_names () {
            yield return new object[] { "TestValue1" };
            yield return new object[] { "TestValue2" };
        }

        [Theory]
        [MemberData (nameof (Test_IsEnum_param_names))]
        public async Task Test_IsEnum_Names (string value) {
            var parse_success = Enum.TryParse (value, out TestEnum parse_result) && Enum.IsDefined (typeof (TestEnum), parse_result);

            if (parse_success) {
                Assert.True (value.IsEnum (out TestEnum? result));
                Assert.Equal (parse_result, result);
            } else {
                Assert.False (value.IsEnum (out TestEnum? result));
                Assert.Null (result);
            }
            await Task.CompletedTask;
        }
        #endregion

        #region Test_ToJsonObj
        class JsonClass {
            public string key1 { get; set; }
            public string key2 { get; set; }
            public string key3 { get; set; }
            public string key4 { get; set; }
            public string key5 { get; set; }
            public int? key6 { get; set; }
            public int? key7 { get; set; }
            public int? key8 { get; set; }
        }

        [Theory]
        [InlineData ("{\"key1\" : \"\", \"key2\" : null, \"key3\" : \"SUCCESS\",\"key4\" : \"null\", \"key6\" : \"2\", \"key7\" : 3}")]
        public async Task Test_ToJsonObj (string value) {
            var jsonclass = value.ToJsonObj<JsonClass> ();
            Assert.Empty (jsonclass.key1);
            Assert.Null (jsonclass.key2);
            Assert.Equal (jsonclass.key3, "SUCCESS");
            Assert.Equal (jsonclass.key4, "null");
            Assert.Null (jsonclass.key5);
            Assert.Equal (jsonclass.key6, 2);
            Assert.Equal (jsonclass.key7, 3);
            Assert.Throws<Newtonsoft.Json.JsonReaderException> (() => { "{\"key8\" : \"SUCCESS\"}".ToJsonObj<JsonClass> (); });
            await Task.CompletedTask;
        }
        #endregion

        public static IEnumerable<object[]> Test_ToLong_param () {
            yield return new object[] { "-1" };
            yield return new object[] { "-1", 1 };
            yield return new object[] { "0" };
            yield return new object[] { "0", 1 };
            yield return new object[] { "1" };
            yield return new object[] { "1", 1 };
            yield return new object[] { "error_parameter" };
            yield return new object[] { "error_parameter", 1 };
            yield return new object[] { "" };
            yield return new object[] { "", 1 };
            yield return new object[] { null };
            yield return new object[] { null, 1 };
        }

        [Theory]
        [MemberData (nameof (Test_ToLong_param))]
        public async Task Test_ToLong (string value, long? default_value = null) {
            var parse_success = long.TryParse (value, out long parse_result);
            if (parse_success) {
                Assert.Equal (parse_result, value.ToLong (default_value));
            } else {
                if (default_value != null) {
                    Assert.Equal (default_value, value.ToLong (default_value));
                } else {
                    Assert.Throws<FormatException> (() => value.ToLong (default_value));
                }
            }
            await Task.CompletedTask;
        }

        public static IEnumerable<object[]> Test_IsLong_param () {
            yield return new object[] { "-1" };
            yield return new object[] { "0" };
            yield return new object[] { "1" };
            yield return new object[] { "error_parameter" };
            yield return new object[] { "" };
            yield return new object[] { null };
        }

        [Theory]
        [MemberData (nameof (Test_IsLong_param))]
        public async Task Test_IsLong (string value) {
            var parse_success = long.TryParse (value, out long parse_result);
            if (parse_success) {
                Assert.True (value.IsLong (out long? result));
                Assert.Equal (parse_result, result);
            } else {
                Assert.False (value.IsLong (out long? result));
                Assert.Null (result);
            }
            await Task.CompletedTask;
        }

        public static IEnumerable<object[]> Test_ToDateTime_param () {
            yield return new object[] { "1987-11-12", null, null };
            yield return new object[] { "1987-11-12", null, "error_parameter" };
            yield return new object[] { "1987-11-12", null, "yyyy-MM-dd" };
            yield return new object[] { "1987-11-12", null, "yyyy-MM-dd HH:mm:ss" };
            yield return new object[] { "1987-11-12 12:00:00", null, null };
            yield return new object[] { "1987-11-12 12:00:00", null, "error_parameter" };
            yield return new object[] { "1987-11-12 12:00:00", null, "yyyy-MM-dd" };
            yield return new object[] { "1987-11-12 12:00:00", null, "yyyy-MM-dd HH:mm:ss" };
            yield return new object[] { "1987-11-12", DateTime.Now, null };
            yield return new object[] { "1987-11-12", DateTime.Now, "yyyy-MM-dd" };
            yield return new object[] { "1987-11-12", DateTime.Now, "yyyy-MM-dd HH:mm:ss" };
            yield return new object[] { "1987-11-12", DateTime.Now, "error_parameter" };
            yield return new object[] { "error_parameter", null, null };
            yield return new object[] { "error_parameter", null, "error_parameter" };
            yield return new object[] { "error_parameter", null, "yyyy-MM-dd" };
            yield return new object[] { "error_parameter", null, "yyyy-MM-dd HH:mm:ss" };
            yield return new object[] { "error_parameter", DateTime.Now, null };
            yield return new object[] { "error_parameter", DateTime.Now, "yyyy-MM-dd" };
            yield return new object[] { "error_parameter", DateTime.Now, "yyyy-MM-dd HH:mm:ss" };
            yield return new object[] { "error_parameter", DateTime.Now, "error_parameter" };
        }

        [Theory]
        [MemberData (nameof (Test_ToDateTime_param))]
        public async Task Test_ToDateTime (string value, DateTime? default_value, string format) {
            var parse_success = format == null? DateTime.TryParse (value, out DateTime parse_result) : DateTime.TryParseExact (value, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out parse_result);
            if (parse_success) {
                Assert.Equal (parse_result, value.ToDateTime (default_value, format));
            } else {
                if (default_value != null) {
                    Assert.Equal (default_value, value.ToDateTime (default_value, format));
                } else {
                    Assert.Throws<FormatException> (() => value.ToDateTime (default_value, format));
                }
            }
            await Task.CompletedTask;
        }

        public static IEnumerable<object[]> Test_IsDateTime_param () {
            yield return new object[] { "1987-11-12", null };
            yield return new object[] { "1987-11-12", "error_parameter" };
            yield return new object[] { "1987-11-12", "yyyy-MM-dd" };
            yield return new object[] { "error_parameter", null };
            yield return new object[] { "error_parameter", "error_parameter" };
            yield return new object[] { "error_parameter", "yyyy-MM-dd" };
        }

        [Theory]
        [MemberData (nameof (Test_IsDateTime_param))]
        public async Task Test_IsDateTime (string value, string format) {
            var parse_success = format == null? DateTime.TryParse (value, out DateTime parse_result) : DateTime.TryParseExact (value, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out parse_result);
            if (parse_success) {
                Assert.True (value.IsDateTime (out DateTime? result, format));
                Assert.Equal (parse_result, result);
            } else {
                Assert.False (value.IsDateTime (out DateTime? result, format));
                Assert.Null (result);
            }
            await Task.CompletedTask;
        }
    }
}