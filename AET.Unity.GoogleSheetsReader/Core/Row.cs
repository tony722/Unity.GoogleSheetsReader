using System;
using System.Collections.Generic;

namespace AET.Unity.GoogleSheetsReader.Core {
  public class Row {
    private Section parentSection;

    public Row(Section parentSection) {
      this.parentSection = parentSection;
      Cells = new Cells(parentSection);
    }

    public Row(Section parentSection, List<string> cells) {
      this.parentSection = parentSection;
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