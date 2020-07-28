using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AET.Unity.GoogleSheetsReader.Core;
using Crestron.SimplSharp;                          				// For Basic SIMPL# Classes

namespace AET.Unity.GoogleSheetsReader {
  public static class SheetReader {
    public static string ReadURL(string url) {
      var client = new Crestron.SimplSharp.Net.Https.HttpsClient();
      client.PeerVerification = false;
      var response = client.GetResponse(url);
      var csvText = response.ContentString;
      return csvText;
    }

    public static Sheet ReadCSVText(List<string> lines) {
      var sheet = new Sheet();
      Section section = null;
      foreach(var line in lines.Where(l => ! l.IsEmptyRow())) {
        var cells = new List<string>(line.Split(','));       
        if(IsSectionRow(cells)) {
          section = sheet.AddSection(cells);
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
