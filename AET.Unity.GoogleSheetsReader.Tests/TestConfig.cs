﻿using System.Collections.Generic;

namespace AET.Unity.GoogleSheetsReader.Tests {
  static class TestConfig {
    public static string GoogleSheet1URL { get; } = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRA51OFndPGJyW1_ygi5ufH66PAYDaQhvoCW6uudK9rvOS3XfMbPGu4jP7nNco6kZPxoNLjIQKMOQxZ/pub?gid=0&single=true&output=csv";

    public static List<string> ReadLines(string text) {
      var reader = new System.IO.StringReader(text);
      var lines = new List<string>();
      string line;
      while ((line = reader.ReadLine()) != null) {
        lines.Add(line);
      }
      return lines;
    }
  }
}
