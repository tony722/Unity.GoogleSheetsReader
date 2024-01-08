using System.Collections.Generic;

namespace AET.Unity.GoogleSheetsReader {
  public static class LineReader  {        
    public static List<string> ReadLines(string text) {
      var reader = new Crestron.SimplSharp.CrestronIO.StringReader(text);    
      var lines = new List<string>();
      string line;
      while ((line = reader.ReadLine()) != null) {        
        lines.Add(line);
      }
      return lines;
    }

    public static bool IsEmptyRow(this string text) {
      if (string.IsNullOrEmpty(text)) return true;
      if (System.Text.RegularExpressions.Regex.IsMatch(text, "^,*$")) return true;
      return false;
    }

    
  }
}
