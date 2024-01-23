using AET.Unity.SimplSharp;
using AET.Unity.SimplSharp.FileIO;
using AET.Unity.SimplSharp.HttpClient;
using System;

namespace AET.Unity.GoogleSheetsReader {
  public class GoogleReader {
    private string SheetName { get; set; }
    public string CacheFilenameBase { get; set; }
    public string GoogleWorkbookId { get; set; }
    public string GoogleApiKey { get; set; }
    private string CacheFilename { get { return string.Format("{0}_{1}.json", CacheFilenameBase, SheetName); } }
    
    static GoogleReader() {
      FileIO = new CrestronFileIO();
      HttpClient = new CrestronHttpsClient(2);
    }
    
    public static IFileIO FileIO;
    public static IHttpClient HttpClient;

    private string ReadHttpsFromGoogle() {      
      var url = string.Format("https://sheets.googleapis.com/v4/spreadsheets/{0}/values/{1}?alt=json&key={2}", GoogleWorkbookId, SheetName, GoogleApiKey); 

      ConsoleMessage.Print("Unity.GoogleSheetsReader: Loading via https Google Workbook '{0}', {1}...", GoogleWorkbookId, SheetName); 
      var result = HttpClient.Get(url);            
      ConsoleMessage.Print("Done. ");      
      return result.Content;
    }

    public string ReadPublishedGoogleSheetJson(string sheetName) {
      if (RequiredParametersAreNotSet()) {
        ErrorMessage.Error("Unity.GoogleSheetsReader: The following must be set before trying to read a Google sheet:\r\nBaseCacheFilename (Current Value: {0})\r\nGoogleWorkbookId (Current Value: {1})\r\nGoogleApiKey (Current Value: {2})\r\n", CacheFilenameBase, GoogleWorkbookId, GoogleApiKey);
        return null;
      }
      SheetName = sheetName;
      var cachedData = ReadCachedData();
      string webData;
      try {
        webData = ReadHttpsFromGoogle();
      } catch (Exception ex) {
        ErrorMessage.Warn("Unity.GoogleSheetsReader: Error retrieving Google Doc, using cache file '{1}'\n{0}", ex.Message, CacheFilename);
        return cachedData;
      }

      if (webData.IsNullOrWhiteSpace()) {
        ErrorMessage.Warn("Unity.GoogleSheetsReader: Error retrieving Google Doc--retrieved empty document, using cache file '{0}'\n", CacheFilename);
        return cachedData;
      }
      UpdateCachedData(webData, cachedData);
      return webData;
    }

    private bool RequiredParametersAreNotSet() { return CacheFilename.IsNullOrWhiteSpace() || GoogleWorkbookId.IsNullOrWhiteSpace() || GoogleApiKey.IsNullOrWhiteSpace(); }

    private void UpdateCachedData(string webData, string cachedData) {
      if (!webData.IsNullOrWhiteSpace() && webData != cachedData) {
        ConsoleMessage.PrintLine("Updating Cached Data");
        try {
          SaveCachedData(webData);
        } catch (Exception ex) {
          ErrorMessage.Warn("Unity.GoogleSheetsReader: Error saving cache file '{1}'\n{0}", ex.Message, CacheFilename);
        }
      }
    }

    private string ReadCachedData() {
      try {
        var newData = FileIO.ReadAllText(CacheFilename);
        return newData;
      } catch {
        ErrorMessage.Notice("Unity.GoogleSheetsReader: Cache file '{0}' does not exist.", CacheFilename);
        return string.Empty;
      }
    }

    private void SaveCachedData(string newData) {
      Crestron.SimplSharp.CrestronIO.File.Delete(CacheFilename); //Because sometimes it doesn't clear the contents with the following line.
      FileIO.WriteText(CacheFilename, newData);
    }
  }
}
