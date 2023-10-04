using AET.Unity.SimplSharp;
using AET.Unity.SimplSharp.FileIO;
using AET.Unity.SimplSharp.HttpClient;
using System;

namespace AET.Unity.GoogleSheetsReader {
  public class GoogleReader {
    private string cachedData;
    private readonly string cacheFilename;
    private readonly string googleSheetsPublishedCsvUrl;

    static GoogleReader() {
      FileIO = new CrestronFileIO();
      HttpClient = new CrestronHttpsClient(1);
    }

    public GoogleReader(string googleSheetsPublishedCsvUrl, string cacheFilename) {
      this.googleSheetsPublishedCsvUrl = googleSheetsPublishedCsvUrl;
      this.cacheFilename = cacheFilename;
      cachedData = ReadCachedData();
    }

    public static IFileIO FileIO;
    public static IHttpClient HttpClient;

    private string ReadHttpsFromGoogle() {
      var csvText = HttpClient.Get(googleSheetsPublishedCsvUrl);
      return csvText.Content;
    }


    public string ReadPublishedGoogleSheetCsv() {
      string newData;
      try {
        newData = ReadHttpsFromGoogle();
      } catch (Exception ex) {
        newData = cachedData;
        ErrorMessage.Warn("Unity.GoogleSheetsReader: Error retrieving Google Doc, using cache file '{1}'\n{0}", ex.Message, cacheFilename);
      }
      if (newData.IsNullOrWhiteSpace()) {
        newData = cachedData;
        ErrorMessage.Warn("Unity.GoogleSheetsReader: Error retrieving Google Doc--retrieved empty document, using cache file '{0}'\n", cacheFilename);
      } else if (newData != cachedData)
        try {
          SaveCachedData(newData);
        } catch (Exception ex) { ErrorMessage.Warn("Unity.GoogleSheetsReader: Error saving cache file '{1}'\n{0}", ex.Message, cacheFilename); }
      return newData;
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
      FileIO.WriteText(cacheFilename, newData);
      cachedData = newData;
    }
  }
}
