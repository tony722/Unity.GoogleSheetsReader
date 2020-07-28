using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using Crestron.SimplSharp.CrestronIO;
using AET.Unity.SimplSharp;
using Crestron.SimplSharp.Net.Https;

namespace AET.Unity.GoogleSheetsReader {
  public class HtmlReader {
    private string cachedData;

    public HtmlReader(string cacheFilename) {
      CacheFilename = cacheFilename;
      cachedData = ReadCachedData();
    }

    public string CacheFilename { get; set; }

    private string ReadURL(string url) {
      var client = new Crestron.SimplSharp.Net.Https.HttpsClient();
      client.PeerVerification = false;
      var response = client.GetResponse(url);
      var csvText = response.ContentString;
      return csvText;
    }


    public string ReadPublishedGoogleSheetCsv(string googleSheetsUrl) {
      string newData;

      try {
        newData = ReadURL(googleSheetsUrl);
      } catch (Exception ex) {
          ErrorMessage.Warn("GoogleSheetsReader: Error retrieving Google Doc, using cache file\n{0}", ex.Message);
          newData = cachedData;
      }
      if(newData != cachedData) SaveCachedData(newData);
      return newData;
    }

    private string ReadCachedData() {
      string newData;
      try {
        var file = File.OpenText(CacheFilename);
        newData = file.ReadToEnd();
        file.Close();
        return newData;
      }
      catch {
        ErrorMessage.Notice("GoogleSheetsReader: Cache file '{0}' does not exist.", CacheFilename);
        SaveCachedData(string.Empty);
        return string.Empty;
      }
    }

    private void SaveCachedData(string newData) {
      var file = File.CreateText(CacheFilename);
      file.Write(newData);
      file.Close();
      cachedData = newData;
    }
  }
}
