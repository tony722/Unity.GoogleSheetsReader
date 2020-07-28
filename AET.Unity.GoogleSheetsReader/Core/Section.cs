using System.Collections.Generic;
using System.Linq;

namespace AET.Unity.GoogleSheetsReader.Core {
  public class Section {
    
    public Section(List<string> headerRow) {      
      Rows = new List<Row>();
      
      Name = headerRow.First();
      Columns = headerRow.Skip(1).ToList();
    }    

    public void AddRow(List<string> row) {
      Rows.Add(new Row(this, row.Skip(1).ToList()));
    }

    public string Name { get; set; }
    
    public List<string> Columns { get; private set; }
    
    public List<Row> Rows { get; set; }            
  }
}