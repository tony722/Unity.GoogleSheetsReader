using System.Collections.Generic;
using Newtonsoft.Json;

namespace AET.Unity.GoogleSheetsReader {
  internal class GoogleSheetJson {
    [JsonProperty("values")]
    public List<List<string>> Values { get; set; }
  }
}
