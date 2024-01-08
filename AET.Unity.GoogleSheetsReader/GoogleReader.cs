using System.Net;
using AET.Unity.SimplSharp;
using AET.Unity.SimplSharp.FileIO;
using AET.Unity.SimplSharp.HttpClient;
using System;

namespace AET.Unity.GoogleSheetsReader {
  public class GoogleReader {
    private string cachedData;
    private string webData;
    private readonly string cacheFilename;    
    private readonly string googleWorkbookId;
    private readonly string googleSheetId;

    static GoogleReader() {
      FileIO = new CrestronFileIO();
      HttpClient = new CrestronHttpsClient(2);
    }
    
    public static bool UseLambda { get; set; }
    public static string LambdaUrl { get; set; }    

    public GoogleReader(string googleWorkbookId, string googleSheetId, string cacheFilename) {
      this.googleWorkbookId = googleWorkbookId;
      this.googleSheetId = googleSheetId;
      this.cacheFilename = cacheFilename;
      cachedData = ReadCachedData();
    }

    public static IFileIO FileIO;
    public static IHttpClient HttpClient;

    private string ReadHttpsFromGoogle() {
      string url;
      if(UseLambda) url = AwsLambdaUrlWorkaroundForBrokenCrestronHttpsClient();
      else url = string.Format("https://docs.google.com/spreadsheets/d/{0}/export?format=csv&gid={1}", googleWorkbookId, googleSheetId); //https://docs.google.com/spreadsheets/d/{0}/gviz/tq?tqx=out:csv?sheet={1}

      ConsoleMessage.Print("Unity.GoogleSheetsReader: Loading via https Google Workbook '{0}', {1}...", googleWorkbookId, googleSheetId);
      var result = HttpClient.Get(url);            
      ConsoleMessage.Print("Done. ");      
      return result.Content;
    }

    private string AwsLambdaUrlWorkaroundForBrokenCrestronHttpsClient() { return string.Format("{2}/?GoogleWorkbookId={0}&GoogleSheetId={1}", googleWorkbookId, googleSheetId, LambdaUrl); }

    public string ReadPublishedGoogleSheetCsv() {
      webData = null;
      try {
        webData = ReadHttpsFromGoogle();
      } catch (Exception ex) {
        ErrorMessage.Warn("Unity.GoogleSheetsReader: Error retrieving Google Doc, using cache file '{1}'\n{0}", ex.Message, cacheFilename);
        return cachedData;
      }

      if (webData.IsNullOrWhiteSpace()) {
        ErrorMessage.Warn("Unity.GoogleSheetsReader: Error retrieving Google Doc--retrieved empty document, using cache file '{0}'\n", cacheFilename);
        return cachedData;
      }
      return webData;
    }

    public void UpdateCachedData() {
      if (!webData.IsNullOrWhiteSpace() && webData != cachedData) {
        ConsoleMessage.PrintLine("Updating Cached Data");
        try {
          SaveCachedData(webData);
        } catch (Exception ex) {
          ErrorMessage.Warn("Unity.GoogleSheetsReader: Error saving cache file '{1}'\n{0}", ex.Message, cacheFilename);
        }
      }
    }
    private string ReadCachedData() {
      try {
        var newData = FileIO.ReadAllText(cacheFilename);
        return newData;
      } catch {
        ErrorMessage.Notice("Unity.GoogleSheetsReader: Cache file '{0}' does not exist.", cacheFilename);
        return string.Empty;
      }
    }

    private void SaveCachedData(string newData) {
      Crestron.SimplSharp.CrestronIO.File.Delete(cacheFilename); //Because sometimes it doesn't clear the contents with the following line.
      FileIO.WriteText(cacheFilename, newData);
      cachedData = newData;
    }
  }
}
