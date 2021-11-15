using AET.Unity.GoogleSheetsReader;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AET.Unity.GoogleSheetsReader.Tests {
  [TestClass()]
  public class SheetReaderTests {
    private SheetReader sheetReader = new SheetReader();

    [TestMethod]
    public void ReadCSVText_ValidData_SheetHasSection() {
      var testText = TestConfig.ReadLines("Section Name 1,Col 1,Col 2,Col 3");
      var sheet = sheetReader.ReadCsvText(testText);
      sheet.Sections.Count.Should().Be(1);
      sheet.Sections[0].Name.Should().Be("Section Name 1");
      sheet.Sections[0].Rows.Count.Should().Be(0);
    }

    [TestMethod]
    public void ReadCSVText_ValidData_CanReadColumnNames() {
      var testText = TestConfig.ReadLines("Section Name 1,Col 1,Col 2,Col 3");
      var sheet = sheetReader.ReadCsvText(testText);
      sheet.Sections[0].Columns[1].Should().Be("Col 2");
    }

    [TestMethod]
    public void ReadCSVText_TextHasQuotes_DelimitsCorrectly() {
      var testText = TestConfig.ReadLines("Section Name 1,Col 1,\"Col 2,2\",\"\"\"Col\"\" 3\"");
      var sheet = sheetReader.ReadCsvText(testText);
      sheet.Sections[0].Columns[1].Should().Be("Col 2,2");
      sheet.Sections[0].Columns[2].Should().Be("\"Col\" 3");
    }

    [TestMethod]
    public void ReadCSVText_ValidRowData_SectionHasRows() {
      var testText = TestConfig.ReadLines("Section Name 1,Col 1,Col 2,Col 3\r\n,Data 1,Data 2,Data 3");
      var sheet = sheetReader.ReadCsvText(testText);
      var section = sheet.Sections[0];
      section.Rows.Count.Should().Be(1);
      section.Rows[0].Cells[1].Should().Be("Data 2");
    }

    [TestMethod]
    public void ReadCSVText_MultiSectionData_ParsedCorrectly() {
      var testText = TestConfig.ReadLines("Section Name 1,Col 1,Col 2,Col 3\r\n,Data 1,Data 2,Data 3\r\n,,,\r\nSection Name 2,Col A,Col B,Col C\r\n,Data A1,Data B1,Data C1\r\n,Data A2,Data B2,Data C2\r\n,,,\r\n,Data A3,Data B3,Data C3\r\n,Data A4,Data B4,Data C4\r\n,Data A5,Data B5,Data C5");
      var sheet = sheetReader.ReadCsvText(testText);
      sheet.Sections.Count.Should().Be(2);
      var section = sheet.Sections[1];
      section.Rows.Count.Should().Be(5);
      section.Rows[4].Cells[2].Should().Be("Data C5");
      section.Rows[4].Cells["Col C"].Should().Be("Data C5");
    }
  }
}