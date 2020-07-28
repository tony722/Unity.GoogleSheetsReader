using System;
using System.Collections.Generic;

namespace AET.Unity.GoogleSheetsReader.Core {
  public class Row {
    private Section parentSection;
    public Row(Section parentSection) {
      this.parentSection = parentSection;
      Cells = new List<string>();
    }

    public Row(Section parentSection, List<string> cells) : this(parentSection) {
      Cells = cells;
    }

    public List<String> Cells { get; set; }

    public string Cell(string columnName) {
      var cellIndex = parentSection.Columns.IndexOf(columnName);
      return Cells[cellIndex];
    }
  }
}