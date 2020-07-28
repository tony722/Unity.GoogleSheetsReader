using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AET.Unity.GoogleSheetsReader {
  public class Columns {
    private List<string> columns;
    private Dictionary<string, int> columnsIndexes;

    public Columns(IEnumerable<string> columns) {
      this.columns = columns.ToList();
      columnsIndexes = columns.Select((s, i) => new { s, i }).ToDictionary(x => x.s, x => x.i);
    }

    public string this[int index] {
      get { return columns[index]; }
    }

    public int this[string columnName] {
      get { return columnsIndexes[columnName]; }
    }
  }
}
