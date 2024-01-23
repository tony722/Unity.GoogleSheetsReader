namespace AET.Unity.GoogleSheetsReader {
  public class Sheet {
    public Sheet() {
      Sections = new Sections();
    }
    public string Name { get; set; }
    public Sections Sections { get; set; }
  }
}