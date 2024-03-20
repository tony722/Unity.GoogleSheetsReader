using System.Collections;
using System.Collections.Generic;

namespace AET.Unity.GoogleSheetsReader {
  public class Cells : IEnumerable<string> {
    private readonly List<string> cells;
    private readonly Section parentSection;


    public Cells(Section parentSection) {
      cells = new List<string>();
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

    public IEnumerator<string> GetEnumerator() { return cells.GetEnumerator(); }

    public override string ToString() {
      return string.Join("\t", cells.ToArray());
    }

    IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

    public string[] ToArray() {
      return cells.ToArray();
    }
  }
}
