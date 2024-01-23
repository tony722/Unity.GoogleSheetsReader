using AET.Unity.SimplSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace AET.Unity.GoogleSheetsReader {
  public class SheetReader {
    private Sheet sheet;
    private Section section;

    public Sheet ReadJsonText(string jsonText) {
      ConsoleMessage.Print("Parsing Json");
      var json = JsonConvert.DeserializeObject<GoogleSheetJson>(jsonText);
      sheet = ReadLines(json.Values);
      ConsoleMessage.PrintLine("Done.");
      return sheet;
    }

    public Sheet ReadLines(List<List<string>> lines) {
      sheet = new Sheet();
      section = null;
      try {
        ParseCells(lines);
      } catch (Exception ex) { ErrorMessage.Warn("Unity.GoogleSheetsReader ParseCells(): {0}", ex.Message); }
      return sheet;
    }


    public Section ReadJsonTextWithoutSections(string jsonText) {
      ConsoleMessage.Print("Parsing Json");
      var json = JsonConvert.DeserializeObject<GoogleSheetJson>(jsonText);
      section = ReadLinesWithoutSections(json.Values);
      ConsoleMessage.PrintLine("Done.");
      return section;
    }

    private Section ReadLinesWithoutSections(List<List<string>> lines) {
      try {
        section = new Section(lines[0], true);
        foreach (var line in lines.Skip(1)) {
          section.AddEntireRow(line);
        }
      } catch (Exception ex) { ErrorMessage.Warn("Unity.GoogleSheetsReader ReadLinesWithoutSections(): {0}", ex.Message); }
      return section;
    }
    
    private void ParseCells(List<List<string>> lines) {
      foreach (var line in lines.Where(l => !IsEmptyRow(l))) {
        try {
          ConsoleMessage.Print(".");
          ParseCell(line);
        }
        catch (Exception ex) {
          ErrorMessage.Warn("Unity.GoogleSheetsReader ParseCell({0}): {1}", line, ex.Message);
        }

      }
    }

    private bool IsEmptyRow(IList<string> line) { return line.All(c => c.IsNullOrWhiteSpace());}

    private void ParseCell(IList<string> cells) {
      if (IsSectionRow(cells)) {
        section = sheet.Sections.Add(cells);
      }
      else if (section == null) {
        //ignore it
      }
      else {
        section.AddRow(cells);
      }
    }

    private static bool IsSectionRow(IList<string> cells) {
      if (cells == null) return false;
      if (cells.Count < 1) return false; 
      return (!string.IsNullOrEmpty(cells[0]));
    }
  }
}
