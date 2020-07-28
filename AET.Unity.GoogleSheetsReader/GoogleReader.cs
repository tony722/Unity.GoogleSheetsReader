using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using Crestron.SimplSharp.CrestronIO;
using AET.Unity.SimplSharp;
using Crestron.SimplSharp.Net.Https;

namespace AET.Unity.GoogleSheetsReader {
  public class GoogleReader {
    private string cachedData;
    private string cacheFilename;
    private string googleSheetsPublishedCsvUrl;

    public GoogleReader(string googleSheetsPublishedCsvUrl, string cacheFilename) {
      this.googleSheetsPublishedCsvUrl = googleSheetsPublishedCsvUrl;
      this.cacheFilename = cacheFilename;
      cachedData = ReadCachedData();
    }


    private string Read() {
      var client = new HttpsClient();
      client.PeerVerification = false;
      var response = client.GetResponse(googleSheetsPublishedCsvUrl);
      var csvText = response.ContentString;
      return csvText;
    }


    public string ReadPublishedGoogleSheetCsv() {
      string newData;
      try {
        newData = Read();
      } catch (Exception ex) {
        ErrorMessage.Warn("GoogleSheetsReader: Error retrieving Google Doc, using cache file\n{0}", ex.Message);
        newData = cachedData;
      }
      if (newData != cachedData) SaveCachedData(newData);
      return newData;
    }

    private string ReadCachedData() {
      string newData;
      try {
        var file = File.OpenText(cacheFilename);
        newData = file.ReadToEnd();
        file.Close();
        return newData;
      } catch {
        ErrorMessage.Notice("GoogleSheetsReader: Cache file '{0}' does not exist.", cacheFilename);
        SaveCachedData(string.Empty);
        return string.Empty;
      }
    }

    private void SaveCachedData(string newData) {
      var file = File.CreateText(cacheFilename);
      file.Write(newData);
      file.Close();
      cachedData = newData;
    }
  }
}
