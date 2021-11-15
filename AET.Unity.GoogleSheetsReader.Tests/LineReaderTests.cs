using AET.Unity.GoogleSheetsReader;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AET.Unity.GoogleSheetsReader.Tests {
  [TestClass]
  public class LineReaderTests {

    [TestMethod]
    public void IsEmptyRow_NullString_ReturnsTrue() {
      LineReader.IsEmptyRow(null).Should().BeTrue();
    }

    [TestMethod]
    public void IsEmptyRow_ContainsOnlyCommas_ReturnsTrue() {
      LineReader.IsEmptyRow(",,,,,").Should().BeTrue();
    }

    [TestMethod]
    public void IsEmptyRow_ContainsData_ReturnsFalse() {
      LineReader.IsEmptyRow("1,2,3,4").Should().BeFalse();
    }
  }
}
