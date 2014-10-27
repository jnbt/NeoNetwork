using System;
using System.Text;
using Neo.Collections;

namespace Neo.Network {
  public class UriBuilder {
    private const char   QUERY_ASSIGNMENT = '=';
    private const char   QUERY_SEPARATOR  = '&';
    private const char   QUERY_INTRO      = '?';
    private const string S_QUERY_ASSIGNMENT = "=";
    private const string S_QUERY_SEPARATOR  = "&";
    private const string S_QUERY_INTRO      = "?";

    private System.UriBuilder builder;

    public UriBuilder() {
      builder = new System.UriBuilder();
    }

    public UriBuilder(string uri) {
      builder = new System.UriBuilder(uri);
    }

    public UriBuilder(Uri uri) {
      builder = new System.UriBuilder(uri);
    }

    public string Fragment {
      get { return builder.Fragment;  }
      set { builder.Fragment = value; }
    }

    public string Host {
      get { return builder.Host;  }
      set { builder.Host = value; }
    }

    public int Port {
      get { return builder.Port;  }
      set { builder.Port = value; }
    }

    public string Path {
      get { return builder.Path;  }
      set { builder.Path = value; }
    }

    public string Query {
      get { return builder.Query;  }
      set { builder.Query = value; }
    }

    public string Scheme{
      get { return builder.Scheme;  }
      set { builder.Scheme = value; }
    }

    public Uri Uri {
      get { return builder.Uri; }
    }

    public override string ToString() {
      return Uri.ToString();
    }

    public UriBuilder SetParam(string key, string value) {
      NameValueCollection q = ParseQueryString(Query);
      q.Set(key, value);
      Query = ToQueryString(q);
      return this;
    }

    public UriBuilder AddParam(string key, string value) {
      NameValueCollection q = ParseQueryString(Query);
      q.Add(key, value);
      Query = ToQueryString(q);
      return this;
    }

    public static NameValueCollection ParseQueryString(string s) {
      NameValueCollection nvc = new NameValueCollection();
      // remove anything other than query string from url
      if(s.Contains(S_QUERY_INTRO)) {
        s = s.Substring(s.IndexOf(S_QUERY_INTRO) + 1);
      }
      s.Split(QUERY_SEPARATOR).ForEach(vp => {
        string[] singlePair = vp.Split(QUERY_ASSIGNMENT);
        if(singlePair.Length == 2) {
          nvc.Add(singlePair[0], UrlUnescape(singlePair[1]));
        } else {
          // only one key with no value specified in query string
          nvc.Add(singlePair[0], string.Empty);
        }
      });
      return nvc;
    }

    public static string ToQueryString(NameValueCollection nvc) {
      if(nvc.IsEmpty) return string.Empty;
      StringBuilder sb = new StringBuilder();
      nvc.ForEach((key, val, i) => {
        if(i > 0) sb.Append(S_QUERY_SEPARATOR);

        if(!string.IsNullOrEmpty(val)){
          string[] vs = val.Split(',');
          if(vs.Length > 1) {
            vs.ForEach((v,j) => {
              if(j > 0) sb.Append(S_QUERY_SEPARATOR);
              sb.Append(key).Append(S_QUERY_ASSIGNMENT).Append(UrlEscape(v));
            });
          }else {
            sb.Append(key).Append(S_QUERY_ASSIGNMENT).Append(UrlEscape(val));
          }
        }
      });
      return sb.ToString();
    }

    public static string UrlEscape(string s) {
      return UnityEngine.WWW.EscapeURL(s);
    }

    public static string UrlUnescape(string s) {
      return UnityEngine.WWW.UnEscapeURL(s);
    }
  }
}
