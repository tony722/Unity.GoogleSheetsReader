using System.Collections.Generic;
using System.Linq;
using AET.Unity.GoogleSheetsReader;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AET.Unity.GoogleSheetsReader.Tests {
  [TestClass()]
  public class SheetReaderTests {
    private readonly SheetReader sheetReader = new SheetReader();

    [TestMethod]
    public void ReadCSVText_ValidData_SheetHasSection() {
      var testText = "Section Name 1,Col 1,Col 2,Col 3";
      var sheet = sheetReader.ReadCsvText(testText);
      sheet.Sections.Count.Should().Be(1);
      sheet.Sections[0].Name.Should().Be("Section Name 1");
      sheet.Sections[0].Rows.Count.Should().Be(0);
    }

    [TestMethod]
    public void ReadCSVText_ValidData_HasMultipleSections() {
      var testText = "Vehicles,Card Number,,Garage,Active,Description\n,001,00001,Main 2,X,Veh 1\n,001,00002,Main 1,,Veh 2\n,001,00003,Collector's 4,X,Veh 3\n,001,00004,None,X,Veh 4\n,001,00005,None,,Veh 5\n,001,00006,None,,Veh 6\n,001,00007,Collector's 5,X,Veh 7\n,001,00008,None,X,Veh 8\n,001,00009,None,X,Veh 9\n,001,00010,Collector's 1,X,\"Veh 10, Fast\"\n,001,00011,None,,\"Veh 11, Slow\"\n,001,00012,Collector's 3,X,\"Veh 12, \"\"Red\"\"\"\n,001,00013,Collector's 6,X,Veh 13\n,001,00014,Main 1,X,Veh 14\n,001,00015,None,,Veh 15\n,001,00016,None,X,Veh 16\n,001,00017,None,,Veh 17\n,001,00018,None,,Veh 18\n,001,00019,None,X,Veh 19\n,001,00020,None,X,Veh 20\n,001,00021,None,X,Veh 21\n,001,00022,None,X,Veh 22\n,001,00023,Collector's 2,X,Veh 23\n,001,00024,None,,Veh 24\n,001,00025,None,,Veh 25\n,001,00026,None,X,Veh 26\n,001,00027,Main 1,X,Veh 27\n,001,00028,None,X,Veh 28\n,001,00029,None,,Veh 29\n,001,00030,Main 1,X,Veh 30\n,001,00031,Main 3,X,Veh 31\n,,,,,\nKeypad,PIN,Active,Description,,\n,1001,X,Code 1,,\n,1002,X,Code 2,,\n,1003,X,Code 3,,\n,1004,,,,\n,1005,,,,\n,1006,,,,\n,1007,,,,\n,1008,,,,\n,1009,,,,\n,1010,,,,\n,1011,,,,";
      var sheet = sheetReader.ReadCsvText(testText);
      sheet.Sections.Count.Should().Be(2);
      sheet.Sections[0].Name.Should().Be("Vehicles");
      sheet.Sections[0].Rows.Count.Should().Be(31);
      sheet.Sections[1].Name.Should().Be("Keypad");
      sheet.Sections[1].Rows.Count.Should().Be(11);

    }

    [TestMethod]
    public void ReadCSVText_ValidData_CanReadColumnNames() {
      var testText = "Section Name 1,Col 1,Col 2,Col 3";
      var sheet = sheetReader.ReadCsvText(testText);
      sheet.Sections[0].Columns[1].Should().Be("Col 2");
    }

    [TestMethod]
    public void ReadCSVText_TextHasQuotes_DelimitsCorrectly() {
      var testText = "Section Name 1,Col 1,\"Col 2,2\",\"\"\"Col\"\" 3\"";
      var sheet = sheetReader.ReadCsvText(testText);
      sheet.Sections[0].Columns[1].Should().Be("Col 2,2");
      sheet.Sections[0].Columns[2].Should().Be("\"Col\" 3");
    }

    [TestMethod]
    public void ReadCSVText_ValidRowData_SectionHasRows() {
      var testText = "Section Name 1,Col 1,Col 2,Col 3\r\n,Data 1,Data 2,Data 3";
      var sheet = sheetReader.ReadCsvText(testText);
      var section = sheet.Sections[0];
      section.Rows.Count.Should().Be(1);
      section.Rows[0].Cells[1].Should().Be("Data 2");
    }

    [TestMethod]
    public void ReadCSVText_MultiSectionData_ParsedCorrectly() {
      var testText = "Section Name 1,Col 1,Col 2,Col 3\r\n,Data 1,Data 2,Data 3\r\n,,,\r\nSection Name 2,Col A,Col B,Col C\r\n,Data A1,Data B1,Data C1\r\n,Data A2,Data B2,Data C2\r\n,,,\r\n,Data A3,Data B3,Data C3\r\n,Data A4,Data B4,Data C4\r\n,Data A5,Data B5,Data C5";
      var sheet = sheetReader.ReadCsvText(testText);
      sheet.Sections.Count.Should().Be(2);
      var section = sheet.Sections[1];
      section.Rows.Count.Should().Be(5);
      section.Rows[4].Cells[2].Should().Be("Data C5");
      section.Rows[4].Cells["Col C"].Should().Be("Data C5");
    }
  }
}