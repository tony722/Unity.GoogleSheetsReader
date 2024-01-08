using AET.Unity.SimplSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharp;

namespace AET.Unity.GoogleSheetsReader {
  public class SheetReader {
    private Sheet sheet;
    private Section section;

    public Sheet ReadCsvText(string csvText) {
      ConsoleMessage.Print("Parsing CSV");
      var lines = CsvFileParser.Parse(csvText);
      sheet = ReadCsvText(lines);
      ConsoleMessage.PrintLine("Done.");
      return sheet;
    }

    public Sheet ReadCsvText(List<List<string>> lines) {
      sheet = new Sheet();
      section = null;
      try {
        ParseCells(lines);
      } catch (Exception ex) { ErrorMessage.Warn("Unity.GoogleSheetsReader ParseCells(): {0}", ex.Message); }
      return sheet;
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
