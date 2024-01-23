using System.Collections.Generic;
using System.Linq;
using AET.Unity.GoogleSheetsReader;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AET.Unity.GoogleSheetsReader.Tests {
  [TestClass()]
  public class SheetReaderTests {
    private readonly SheetReader sheetReader = new SheetReader();
    private string sampleJson = "{\"majorDimension\": \"ROWS\", \"range\": \"Sheet1!A1:D12\", \"values\": [[\"Section 1 Name\", \"Name\", \"Date\", \"Value\"], [\"\", \"Item 1\", \"12/1/2022\", \"105.3\"], [\"\", \"Item 2\", \"12/13/2022\", \"99.7\"], [\"My 2nd Section\", \"Description\", \"Qty\", \"Price\"], [\"\", \"Item Type A\", \"100\", \"25.95\"], [\"\", \"Item Type B\", \"50\", \"14.95\"]]}";
    
    //Json Samples from: https://docs.google.com/spreadsheets/d/1WCXD3m8lKbrhlAkJa6cHlWXUSzqFqCSpCL9sSZEk4Uo/edit

    [TestMethod]
    public void ReadCSVText_ValidData_SheetHasSection() {
      var testText = "{\"majorDimension\": \"ROWS\", \"range\": \"Sheet1!A1:D12\", \"values\": [[\"Section 1 Name\", \"Name\", \"Date\", \"Value\"]]}";
      var sheet = sheetReader.ReadJsonText(testText);
      sheet.Sections.Count.Should().Be(1);
      sheet.Sections[0].Name.Should().Be("Section 1 Name");
      sheet.Sections[0].Rows.Count.Should().Be(0);
    }
    [TestMethod]
    public void ReadCSVText_ValidData_HasMultipleSections() {
      var testText = sampleJson;
      var sheet = sheetReader.ReadJsonText(testText);
      sheet.Sections.Count.Should().Be(2);
      sheet.Sections[0].Name.Should().Be("Section 1 Name");
      sheet.Sections[0].Rows.Count.Should().Be(2);
      sheet.Sections[1].Name.Should().Be("My 2nd Section");
      sheet.Sections[1].Rows.Count.Should().Be(2);

    }

    [TestMethod]
    public void ReadCSVText_ValidData_CanReadColumnNames() {
      var testText = sampleJson;
      var sheet = sheetReader.ReadJsonText(testText);
      sheet.Sections[0].Columns[1].Should().Be("Date");
    }

    [TestMethod]
    public void ReadCSVText_ValidRowData_SectionHasRows() {
      var testText = sampleJson;
      var sheet = sheetReader.ReadJsonText(testText);
      var section = sheet.Sections[0];
      section.Rows.Count.Should().Be(2);
      section.Rows[0].Cells[1].Should().Be("12/1/2022");
      section.Rows[1]["Value"].Should().Be("99.7");
    }

    [TestMethod]
    public void ReadCSVText_MultiSectionData_ParsedCorrectly() {
      var testText = sampleJson;
      var sheet = sheetReader.ReadJsonText(testText);
      sheet.Sections.Count.Should().Be(2);
      var section = sheet.Sections["My 2nd Section"];
      section.Rows.Count.Should().Be(2);
      section.Rows[1]["Description"].Should().Be("Item Type B");
      section.Rows[0].Cells["Price"].Should().Be("25.95");
    }

    [TestMethod]
    public void ReadJsonText_SheetHasNoSections_ParsedCorrectly() {
      var testText = "{\"majorDimension\": \"ROWS\", \"range\": \"'Sectionless Sheet'!A1:D7\", \"values\": [[\"SKU\", \"Type\", \"Qty\", \"Description\"], [\"BK1023\", \"Nut\", \"1053\", \"Description BK1023\"], [\"RM3034\", \"Nut\", \"8\", \"Description RM3034\"], [\"43RA3A\", \"Bolt\", \"10\", \"Description 43RA3A\"], [\"INT21\", \"Bolt\", \"30011\", \"Description INT21\"], [\"INT44\", \"Bolt\", \"221\", \"Description INT44\"], [\"QU34M7\", \"Screw\", \"314\", \"Description QU34M7\"]]}";
      var section = sheetReader.ReadJsonTextWithoutSections(testText);
      section.Rows.Count.Should().Be(6);
      section.Rows[3]["SKU"].Should().Be("INT21");
      section.Rows[5]["Description"].Should().Be("Description QU34M7");
    }
  }
}