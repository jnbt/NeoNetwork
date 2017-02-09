using System;
using System.Text;
using Neo.Collections;

namespace Neo.Network.Http {
  /// <summary>
  /// A simple cookie jar implemenation which extracts the actual cookies 
  /// from raw HTTP response header fields.
  /// </summary>
  public sealed class CookieJar : ICookieJar {
    private static readonly string[] SET_COOKIE = new string[] { "SET-COOKIE", "set-cookie", "Set-Cookie", "Set-cookie" };
    private const char COOKIE_SPLITTER = ';';
    private const char COOKIE_VALUE_MARKER = '=';

    /// <summary>
    /// Holds all current cookies
    /// </summary>
    public readonly Dictionary<string, string> Store = new Dictionary<string, string>();

    /// <summary>
    /// Extractes the cookies from HTTP response headers as Unity returns them
    /// from it's WWW class
    /// </summary>
    /// <remarks>
    /// One would expect a NameValueCollection to be the headers (as header fields with
    /// the same name are allowed in the HTTP standard), but Unity decided to use
    /// a Dictionary.
    /// </remarks>
    /// <param name="headers">raw HTTP headers</param>
    public void Update(Dictionary<string, string> headers) {
      extractCookie(headers);
    }

    /// <summary>
    /// True if no cookies are set
    /// </summary>
    public bool IsEmpty {
      get {
        return Store.Count == 0;
      }
    }

    /// <summary>
    /// Clears all cookies
    /// </summary>
    public void Clear() {
      Store.Clear();
    }

    /// <summary>
    /// Build a complete HTTP conform cookie string out of
    /// all collected cookies
    /// </summary>
    /// <returns>A HTTP confirm cookie header value</returns>
    public override string ToString() {
      StringBuilder sb = new StringBuilder();
      int i = 0;
      Store.ForEach((key, value) => {
        if(i > 0) sb.Append(COOKIE_SPLITTER);
        sb.Append(key).Append(COOKIE_VALUE_MARKER).Append(value);
        i++;
      });
      return sb.ToString();
    }

    private void extractCookie(Dictionary<string, string> headers) {
      for(int i = 0, imax = SetCookie.Length; i < imax; i++) {
        string field = SetCookie[i];
        if(!headers.ContainsKey(field) || string.IsNullOrEmpty(headers[field])) continue;
        string[] parts = headers[field].Split(CookieSplitter);
        for(int j = 0, jmax = parts.Length; j < jmax; j++) {
          string part = parts[j];
          int markerIndex = part.IndexOf(CookieValueMarker);
          if(markerIndex <= 0) continue;
          string name = part.Substring(0, markerIndex);
          string val = part.Substring(markerIndex + 1, part.Length - markerIndex - 1);
          Store[name] = val;
        }
      }
    }
  }
}
