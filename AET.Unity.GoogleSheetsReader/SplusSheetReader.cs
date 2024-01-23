using AET.Unity.SimplSharp;

namespace AET.Unity.GoogleSheetsReader {
  public class SplusSheetReader {
    public Section section;      
    public string CacheFilenameBase { get; set; }
    public string GoogleWorkbookId { get; set; }

    public string GoogleApiKey { get; set; }
    
    public void ReadSection(string sheetName, string sectionName) {
      var reader = new GoogleReader { CacheFilenameBase = CacheFilenameBase, GoogleWorkbookId = GoogleWorkbookId, GoogleApiKey = GoogleApiKey };
      var jsonText = reader.ReadPublishedGoogleSheetJson(sheetName);
      
      var sheet = new SheetReader().ReadJsonText(jsonText);
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
