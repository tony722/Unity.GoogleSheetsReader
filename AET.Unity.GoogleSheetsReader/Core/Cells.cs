using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AET.Unity.GoogleSheetsReader.Core {
  public class Cells {
    private List<String> cells;
    private Section parentSection;


    public Cells(Section parentSection) {
      this.cells = new List<string>();
      this.parentSection = parentSection;

    }
    public Cells(Section parentSection, List<string> cells) {
      this.cells = cells;
      this.parentSection = parentSection;
    }

    public string this[int columnIndex] {
      get { return cells[columnIndex]; }
    }

    public string this[string columnName] {
      get {
        var cellIndex = parentSection.Columns[columnName];
        return cells[cellIndex];
      }
    }
  }
}
