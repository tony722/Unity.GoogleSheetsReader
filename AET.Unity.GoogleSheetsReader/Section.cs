using System.Collections.Generic;
using System.Linq;

namespace AET.Unity.GoogleSheetsReader {
  public class Section {
    
    public Section(List<string> headerRow) {      
      Rows = new List<Row>();
      
      Name = headerRow.First();
      Columns = headerRow.Skip(1).Select((s, i) => new {s, i}).ToDictionary(x => x.s, x => x.i);
    }    

    public void AddRow(List<string> row) {
      Rows.Add(new Row(this, row.Skip(1).ToList()));
    }

    public string Name { get; set; }
    
    public Dictionary<string, int> Columns { get; private set; }
    
    public List<Row> Rows { get; set; }            
  }
}