using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
//using System.Net;
//using System.Net.Sockets;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Crestron.SimplSharp.Ssh;
using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.Security;


namespace AET.Unity.Http {
  public class BouncyCastleHttpClient {

    public string Post(string url, string contents) { throw new NotImplementedException(); }

    public string Post(string url, string contents, IEnumerable<KeyValuePair<string, string>> additionalHeaders) { throw new NotImplementedException(); }

    public string Get(string url) { return Get(url, null); }

    public string Get(string url, IEnumerable<KeyValuePair<string, string>> additionalHeaders) { return GetViaBouncyCastle(url); }

    private string GetViaBouncyCastle(string url) {
      var uri = new Uri(url);
      using (var client = new TcpClient(uri.Host, 443)) {
        var sr = new SecureRandom();
        var protocol = new TlsClientProtocol(client.GetStream(), sr);
        protocol.Connect(new MyTlsClient());
        using (var stream = protocol.Stream) {
          var hdr = new StringBuilder();
          //https://docs.google.com/spreadsheets/d/1WCXD3m8lKbrhlAkJa6cHlWXUSzqFqCSpCL9sSZEk4Uo/gviz/tq?tqx=out:csv&sheet=Sheet1
          hdr.AppendLine("GET " + uri.PathAndQuery + " HTTP/1.0");
          hdr.AppendLine("Host: " + uri.Host);
          hdr.AppendLine("Content-Type: text/xml; charset=utf-8");
          hdr.AppendLine("Accept-Encoding: identity");
          hdr.AppendLine("Connection: close");
          hdr.AppendLine();

          var dataToSend = Encoding.ASCII.GetBytes(hdr.ToString());
          stream.Write(dataToSend, 0, dataToSend.Length);


          var response = "";
          using (var reader = new StreamReader(stream)) {
            var waitingForFirstLine = true;
            while (stream.CanRead) {
              try {
                var line = reader.ReadLine();
                if (waitingForFirstLine) {
                  if (line == string.Empty) waitingForFirstLine = false;
                  continue;
                }
                response += line + "\r\n";
              } catch (TlsNoCloseNotifyException ex) {
                stream.Close();
                client.Close();
              } catch (Exception ex) {
                throw ex;
              }
            }
          }
          return response;


          //  int totalRead = 0;
          //  string response = "";

          //  byte[] buff = new byte[2048];
          //  do {
          //    totalRead = stream.Read(buff, 0, buff.Length);
          //    response = Encoding.ASCII.GetString(buff, 0, totalRead);
          //  } while (stream.CanRead);
          //  return response;
        }
      }
    }

    private class MyTlsClient : DefaultTlsClient {
      public override TlsAuthentication GetAuthentication() { return new MyTlsAuthentication(); }
    }

    private class MyTlsAuthentication : TlsAuthentication {
      public TlsCredentials GetClientCredentials(CertificateRequest certificateRequest) { return null; }

      public void NotifyServerCertificate(Certificate serverCertificate) { }
    }


    private string GetWebrequest(string url) {
      var client = WebRequest.Create(url);
      client.Method = "GET";
      var response = client.GetResponse();
      var responseStream = response.GetResponseStream();
      var sr = new StreamReader(responseStream);
      var content = sr.ReadToEnd();
      return content;
    }

    private string GetWebrequestViaReflection(string url) {
      //var webrequestType = Type.GetType("System.Net.WebRequest, System, Version=3.5.0.0, Culture=neutral, PublicKeyToken=969DB8053D3322AC");
      var webrequestType = typeof(System.Net.WebRequest);
      var client = webrequestType.GetMethod("Create", BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder, new Type[] { typeof(string) }, null).Invoke(null, new object[] { url });
      webrequestType.GetProperty("Method").SetValue(client, "GET", null);
      var response = webrequestType.GetMethod("GetResponse").Invoke(client, null);
      var responseType = response.GetType();
      var responseCode = (string)responseType.GetProperty("StatusCode").GetValue(response, null);
      var responseUri = responseType.GetProperty("ResponseUri").GetValue(response, null);
      var responseStream = response.GetType().GetMethod("GetResponseStream").Invoke(response, null);
      var streamReaderType = typeof(System.IO.StreamReader);
      var srCtor = streamReaderType.GetConstructor(new Type[] { typeof(System.IO.Stream) });
      var sr = srCtor.Invoke(new object[] { responseStream });
      var content = (string)streamReaderType.GetMethod("ReadToEnd").Invoke(sr, null);
      return content;

    }

    public ushort Debug { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }
  }
}
