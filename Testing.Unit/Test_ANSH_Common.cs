using System;
using Xunit;

namespace Testing.Unit
{
     public class Test_ANSH_Common {

        public enum TestEnum {
            TestValue1 = 0x000,
            TestValue2 = 0x002,
            TestValue3 = 0x004,
            TestValue4 = 0x008,
            TestValue5 = 0x010
        }

        #region ANSHCommonExtensions
        [Theory]
        [InlineData ("-1")]
        [InlineData ("-1", 1)]
        [InlineData ("0")]
        [InlineData ("0", 1)]
        [InlineData ("1")]
        [InlineData ("1", -1)]
        [InlineData ("a")]
        [InlineData ("a", 1)]
        [InlineData ("")]
        [InlineData ("", 1)]
        [InlineData (null)]
        [InlineData (null, 1)]
        public void Test_ANSHCommonExtensions_ToInt (string value, int? default_value = null) {
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
        }

        [Theory]
        [InlineData ("-1")]
        [InlineData ("0")]
        [InlineData ("1")]
        [InlineData ("a")]
        [InlineData ("")]
        [InlineData (null)]
        public void Test_ANSHCommonExtensions_IsInt (string value) {
            var parse_success = int.TryParse (value, out int parse_result);
            if (parse_success) {
                Assert.True (value.IsInt (out int? result));
                Assert.Equal (parse_result, result);
            } else {
                Assert.False (value.IsInt (out int? result));
                Assert.Null (result);
            }
        }

        [Theory]
        [InlineData (1)]
        [InlineData (1, TestEnum.TestValue1)]
        [InlineData (0x002)]
        [InlineData (0x002, TestEnum.TestValue1)]
        public void Test_ANSHCommonExtensions_ToEnum (int value, TestEnum? default_value = null) {
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
        }

        [Theory]
        [InlineData (1)]
        [InlineData (0x002)]
        public void Test_ANSHCommonExtensions_IsEnum (int value) {
            var parse_success = Enum.TryParse (value.ToString (), out TestEnum parse_result) && Enum.IsDefined (typeof (TestEnum), parse_result);

            if (parse_success) {
                Assert.True (value.IsEnum (out TestEnum? result));
                Assert.Equal (parse_result, result);
            } else {
                Assert.False (value.IsEnum (out TestEnum? result));
                Assert.Null (result);
            }
        }

         [Theory]
        [InlineData ("-1")]
        [InlineData ("-1", 1)]
        [InlineData ("0")]
        [InlineData ("0", 1)]
        [InlineData ("1")]
        [InlineData ("1", -1)]
        [InlineData ("a")]
        [InlineData ("a", 1)]
        [InlineData ("")]
        [InlineData ("", 1)]
        [InlineData (null)]
        [InlineData (null, 1)]
        public void Test_ANSHCommonExtensions_ToLong (string value, long? default_value = null) {
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
        }

        [Theory]
        [InlineData ("-1")]
        [InlineData ("0")]
        [InlineData ("1")]
        [InlineData ("a")]
        [InlineData ("")]
        [InlineData (null)]
        public void Test_ANSHCommonExtensions_IsLong (string value) {
            var parse_success = long.TryParse (value, out long parse_result);
            if (parse_success) {
                Assert.True (value.IsLong (out long? result));
                Assert.Equal (parse_result, result);
            } else {
                Assert.False (value.IsLong (out long? result));
                Assert.Null (result);
            }
        }
        #endregion
    }
}
