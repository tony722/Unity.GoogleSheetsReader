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
      return ReadCsvText(LineReader.ReadLines(csvText));
    }

    public Sheet ReadCsvText(List<string> lines) {
      sheet = new Sheet();
      section = null;
      CrestronConsole.Print(".");
      try {
        ParseCells(lines);
      } catch (Exception ex) { ErrorMessage.Warn("Unity.GoogleSheetsReader ParseCells(): {0}", ex.Message); }
      return sheet;
    }

    private void ParseCells(List<string> lines) {
      foreach (var line in lines.Where(l => !l.IsEmptyRow())) {
        try {
          ParseCell(line);
        }
        catch (Exception ex) {
          ErrorMessage.Warn("Unity.GoogleSheetsReader ParseCell({0}): {1}", line, ex.Message);
        }

      }
    }

    private void ParseCell(string line) {
      var cells = line.ParseCsv();
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
