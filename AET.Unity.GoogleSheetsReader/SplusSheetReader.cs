using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AET.Unity.SimplSharp;

namespace AET.Unity.GoogleSheetsReader {
  public class SplusSheetReader {
    public Section section;   
    
    public void ReadSection(string googleSheetsPublishedCsvUrl, string cacheFilename, string sectionName) {  
      var reader = new GoogleReader(googleSheetsPublishedCsvUrl, cacheFilename);
      string csvText = reader.ReadPublishedGoogleSheetCsv();

      var sheet = new SheetReader().ReadCsvText(csvText);
      section = sheet.Sections[sectionName];
    }    

    public ushort RowCount { get { return (ushort)section.Rows.Count; } } 

    public ushort CellCount { get { return (ushort)section.Columns.Count; } }

    //Row is one-based
    public string Cell(ushort row, string columnName) {
      if (row == 0) {
        ErrorMessage.Error("GoogleSheetsReader.SplusSheetReader: there is no row #0. First row is row #1.");
        return "";
      }
      return section.Rows[row - 1].Cells[columnName];
    }
  }
}
