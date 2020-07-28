using System.Collections.Generic;
using System.Linq;

namespace AET.Unity.GoogleSheetsReader {
  public class Section {
    
    public Section(List<string> headerRow) {      
      Rows = new List<Row>();
      
      Name = headerRow.First();
      Columns = new Columns(headerRow.Skip(1));
    }    

    public void AddRow(List<string> row) {
      Rows.Add(new Row(this, row.Skip(1).ToList()));
    }

    public string Name { get; set; }
    
    public Columns Columns { get; private set; }
    
    public List<Row> Rows { get; set; }            
  }
}