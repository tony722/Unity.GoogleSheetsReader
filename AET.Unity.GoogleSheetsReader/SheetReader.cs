using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AET.Unity.SimplSharp;
using Crestron.SimplSharp;                          				// For Basic SIMPL# Classes

namespace AET.Unity.GoogleSheetsReader {
  public static class SheetReader {
    public static Sheet ReadCSVText(string csvText) {
      return ReadCSVText(LineReader.ReadLines(csvText));
    }

    public static Sheet ReadCSVText(List<string> lines) {
      var sheet = new Sheet();
      Section section = null;
      foreach(var line in lines.Where(l => ! l.IsEmptyRow())) {
        var cells = new List<string>(line.Split(','));
        if (IsSectionRow(cells)) {
          section = sheet.AddSection(cells);
        } else if (section == null) {
          ErrorMessage.Error("GoogleSheetsReader tried to read improperly formatted sheet. No section header for data:\n{0}", line);
        } else {
          section.AddRow(cells);
        }
      }
      return sheet;
    }

    private static bool IsSectionRow(List<string> cells) {
      return (!string.IsNullOrEmpty(cells[0]));
    }
  }
}
