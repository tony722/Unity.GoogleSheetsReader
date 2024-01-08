using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AET.Unity.SimplSharp;
using Crestron.SimplSharp;

namespace AET.Unity.GoogleSheetsReader {
  public static class SplusGlobalSettings {
    public static void SetUseLambda(ushort value) { GoogleReader.UseLambda = value.ToBool(); }
    public static void SetLambdaUrl(string value) { GoogleReader.LambdaUrl = value; }
  }
}