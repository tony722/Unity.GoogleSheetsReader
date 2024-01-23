using System.Collections.Generic;
using System.Linq;

namespace AET.Unity.GoogleSheetsReader {
  public class Row {
    public Row(Section parentSection) {
      Cells = new Cells(parentSection);
    }

    public Row(Section parentSection, List<string> cells) {
      var missingCellsCount = parentSection.Columns.Count - cells.Count;
      if(missingCellsCount > 0) cells.AddRange(Enumerable.Repeat(string.Empty, missingCellsCount));
      Cells = new Cells(parentSection, cells);
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