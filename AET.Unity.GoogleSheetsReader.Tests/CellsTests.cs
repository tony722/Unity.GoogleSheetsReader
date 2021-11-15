using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AET.Unity.GoogleSheetsReader.Tests {
  [TestClass]
  public class CellsTests {
    [TestMethod]
    public void ToString_CellsContainText_ConcatenatesWithTab() {
      var cells = new Cells(null, new [] {"v1","v2","v3"}.ToList());
      cells.ToString().Should().Be("v1\tv2\tv3");
    }
  }
}
