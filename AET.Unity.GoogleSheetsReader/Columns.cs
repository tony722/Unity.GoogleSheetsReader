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
      get {
        try {
          return columnsIndexes[columnName];
        } catch (Exception ex) {
          if (ex is KeyNotFoundException) throw new KeyNotFoundException(string.Format("Unity.GoogleSheetsReader Tried to retrieve column that doesn't exist: '{0}'", columnName));
          throw;
        }
      }
    }

    public int Count { get { return columns.Count; } }
  }
}
