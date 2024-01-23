using System.Collections.Generic;
using System.Linq;

namespace AET.Unity.GoogleSheetsReader {
  public class Section {

    public Section(IList<string> headerRow) : this(headerRow, false) { }

    public Section(IList<string> headerRow, bool includeColumn0) {
      Rows = new List<Row>();
      if (includeColumn0) {
        Columns = new Columns(headerRow.ToList());
      } else {
        Name = headerRow.First();
        Columns = new Columns(headerRow.Skip(1).ToList());
      }
    }

    public void AddRow(IList<string> row) {
      Rows.Add(new Row(this, row.Skip(1).ToList()));
    }

    public void AddEntireRow(IList<string> row) {
      Rows.Add(new Row(this, row.ToList()));
    }

    public string Name { get; set; }
    
    public Columns Columns { get; private set; }
    
    public List<Row> Rows { get; set; }            
  }
}