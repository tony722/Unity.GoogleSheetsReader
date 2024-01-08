using AET.Unity.SimplSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AET.Unity.GoogleSheetsReader.Tests {
  [TestClass]
  public static  class AssemblyInit {
    [AssemblyInitialize]
    public static void AssemblyInitialize(TestContext context) {
      ConsoleMessage.ConsoleMessageHandler = ConsoleMessageHandler;
    }

    public static TestConsoleMessageHandler ConsoleMessageHandler { get; } = new TestConsoleMessageHandler ();

    public static TestErrorMessageHandler ErrorMessageHandler { get; } = new TestErrorMessageHandler();
  }
}
