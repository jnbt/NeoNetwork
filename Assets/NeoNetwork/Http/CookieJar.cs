using System;
using System.Text;
using Neo.Collections;

namespace Neo.Network.Http {
  public sealed class CookieJar : ICookieJar {
    private static readonly string[]     SET_COOKIE          = new string[]{"SET-COOKIE", "set-cookie", "Set-Cookie", "Set-cookie"};
    private const char                   COOKIE_SPLITTER     = ';';
    private const char                   COOKIE_VALUE_MARKER = '=';

    public readonly Dictionary<string,string> Store = new Dictionary<string,string>();

    public void Update(System.Collections.Generic.Dictionary<string,string> headers){
      extractCookie(headers);
    }

    public bool IsEmpty{
      get{
        return Store.Count == 0;
      }
    }

    public void Clear(){
      Store.Clear();
    }

    public override string ToString(){
      StringBuilder sb = new StringBuilder();
      int i = 0;
      Store.ForEach((key, value) => {
        if(i > 0) sb.Append(COOKIE_SPLITTER);
        sb.Append(key).Append(COOKIE_VALUE_MARKER).Append(value);
        i++;
      });
      return sb.ToString();
    }

    private void extractCookie(System.Collections.Generic.Dictionary<string,string> headers){
      for(int i=0, imax = SET_COOKIE.Length; i<imax; i++){
        string field = SET_COOKIE[i];
        if(headers.ContainsKey(field) && !string.IsNullOrEmpty(headers[field])){
          string[] parts = headers[field].Split(COOKIE_SPLITTER);
          for(int j=0, jmax = parts.Length; j<jmax; j++){
            string part = parts[j];
            int markerIndex = part.IndexOf(COOKIE_VALUE_MARKER);
            if(markerIndex > 0){
              string name = part.Substring(0,markerIndex);
              string val  = part.Substring(markerIndex+1, part.Length-markerIndex-1);
              Store[name] = val;
            }
          }
        }
      }
    }

  }
}
