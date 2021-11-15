using System.Collections.Generic;

namespace AET.Unity.GoogleSheetsReader {
  public class Row {
    public Row(Section parentSection) {
      Cells = new Cells(parentSection);
    }

    public Row(Section parentSection, List<string> cells) {
      this.Cells = new Cells(parentSection, cells);
    }

    public Cells Cells { get; private set; }

    public string this[string columnName] {
      get {
        return Cells[columnName];
      }
    }

    public string this[int columnIndex] {
      get {
        return Cells[columnIndex];
      }
    }

  }
}