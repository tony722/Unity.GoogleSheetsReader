using System.Collections.Generic;
using System.Linq;

namespace AET.Unity.GoogleSheetsReader.Core {
  public class Sheet {
    public Sheet() {
      Sections = new List<Section>();
    }
    public string Name { get; set; }
    public List<Section> Sections { get; set; }

    public Section Section(string name) {
      return Sections.FirstOrDefault(s => s.Name == name);
    }

    public Section AddSection(List<string> row) {
      var section = new Section(row);
      Sections.Add(section);
      return section;
    }
  }
}