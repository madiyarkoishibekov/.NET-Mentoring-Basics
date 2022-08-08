using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace DB_Operations_Tests
{
    public static class NUnitExtensions
    {
        public static void AreEqualByJson(object expected, object actual)
        {
            var expectedJson = JsonSerializer.Serialize(expected);
            var actualJson = JsonSerializer.Serialize(actual);
            Assert.AreEqual(expectedJson, actualJson);
        }
    }
}
