using System;
using System.Collections.Generic;
using System.Linq;
using AET.Unity.SimplSharp;

namespace AET.Unity.GoogleSheetsReader {
  public class Columns {
    private readonly IList<string> columns;
    private readonly Dictionary<string, int> columnsIndexes;

    public Columns(IList<string> columns) {
      this.columns = columns.ToList();      
      columnsIndexes = columns.Select((s, i) => new { s, i }).SafeToDictionary(x => x.s, x => x.i, StringComparer.OrdinalIgnoreCase);
    }

    public string this[int index] {
      get { return columns[index]; }
    }

    public int this[string columnName] {
      get { return columnsIndexes[columnName]; }
    }

    public int Count { get { return columns.Count; } }
  }
}
