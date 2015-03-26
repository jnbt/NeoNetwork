using System;
using System.Text;
using Neo.Collections;

namespace Neo.Network {
  /// <summary>
  /// Helper class to encode and combine parameters into URIs
  /// </summary>
  public class UriBuilder {
    private const char QUERY_ASSIGNMENT = '=';
    private const char QUERY_SEPARATOR = '&';
    private const char QUERY_INTRO = '?';
    private const string S_QUERY_ASSIGNMENT = "=";
    private const string S_QUERY_SEPARATOR = "&";
    private const string S_QUERY_INTRO = "?";

    private System.UriBuilder builder;

    /// <summary>
    /// Instantiate a fresh instance
    /// </summary>
    public UriBuilder() {
      builder = new System.UriBuilder();
    }

    /// <summary>
    /// Instantiate a instance based on an existing URI
    /// </summary>
    /// <param name="uri">to start with</param>
    public UriBuilder(string uri) {
      builder = new System.UriBuilder(uri);
    }

    /// <summary>
    /// Instantiate a instance based on an existing URI
    /// </summary>
    /// <param name="uri">to start with</param>
    public UriBuilder(Uri uri) {
      builder = new System.UriBuilder(uri);
    }

    /// <summary>
    /// The fragment part of the URI
    /// </summary>
    public string Fragment {
      get { return builder.Fragment; }
      set { builder.Fragment = value; }
    }

    /// <summary>
    /// The host part of the URI
    /// </summary>
    public string Host {
      get { return builder.Host; }
      set { builder.Host = value; }
    }

    /// <summary>
    /// The port part of the URI
    /// </summary>
    public int Port {
      get { return builder.Port; }
      set { builder.Port = value; }
    }

    /// <summary>
    /// The path part of the URI
    /// </summary>
    public string Path {
      get { return builder.Path; }
      set { builder.Path = value; }
    }

    /// <summary>
    /// The query part of the URI
    /// </summary>
    public string Query {
      get { return builder.Query; }
      set { builder.Query = value; }
    }

    /// <summary>
    /// The scheme part of the URI
    /// </summary>
    public string Scheme {
      get { return builder.Scheme; }
      set { builder.Scheme = value; }
    }

    /// <summary>
    /// Build the current state as an URI object
    /// </summary>
    public Uri Uri {
      get { return builder.Uri; }
    }

    /// <summary>
    /// Build the current state as a single string
    /// </summary>
    /// <returns>An URI string</returns>
    public override string ToString() {
      return Uri.ToString();
    }

    /// <summary>
    /// Sets a new parameter
    /// </summary>
    /// <param name="key">parameter's name</param>
    /// <param name="value">parameter's value</param>
    /// <returns>The builder itself</returns>
    public UriBuilder SetParam(string key, string value) {
      NameValueCollection q = ParseQueryString(Query);
      q.Set(key, value);
      Query = ToQueryString(q);
      return this;
    }

    /// <summary>
    /// Adds a new parameter
    /// </summary>
    /// <param name="key">parameter's name</param>
    /// <param name="value">parameter's value</param>
    /// <returns>The builder itself</returns>
    public UriBuilder AddParam(string key, string value) {
      NameValueCollection q = ParseQueryString(Query);
      q.Add(key, value);
      Query = ToQueryString(q);
      return this;
    }

    /// <summary>
    /// Sets a batch of query parameters
    /// </summary>
    /// <param name="parameters">to be set</param>
    /// <returns>The builder itself</returns>
    public UriBuilder SetParams(NameValueCollection parameters) {
      NameValueCollection q = ParseQueryString(Query);
      parameters.ForEach((k, v) => q.Set(k, v));
      Query = ToQueryString(q);
      return this;
    }

    /// <summary>
    /// Sets a batch of query parameters
    /// </summary>
    /// <param name="parameters">to be set</param>
    /// <returns>The builder itself</returns>
    public UriBuilder SetParams(Dictionary<string, string> parameters) {
      NameValueCollection q = ParseQueryString(Query);
      parameters.ForEach((k, v) => q.Set(k, v));
      Query = ToQueryString(q);
      return this;
    }

    /// <summary>
    /// Adds a batch of query parameters
    /// </summary>
    /// <param name="parameters">to be set</param>
    /// <returns>The builder itself</returns>
    public UriBuilder AddParams(NameValueCollection parameters) {
      NameValueCollection q = ParseQueryString(Query);
      q.Add(parameters);
      Query = ToQueryString(q);
      return this;
    }

    /// <summary>
    /// Adds a batch of query parameters
    /// </summary>
    /// <param name="parameters">to be set</param>
    /// <returns>The builder itself</returns>
    public UriBuilder AddParams(Dictionary<string, string> parameters) {
      NameValueCollection q = ParseQueryString(Query);
      parameters.ForEach((k, v) => q.Add(k, v));
      Query = ToQueryString(q);
      return this;
    }

    /// <summary>
    /// Parses URI string for it's query parameters
    /// </summary>
    /// <param name="s">URI</param>
    /// <returns>The query parameters</returns>
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

    /// <summary>
    /// Renders query parameters as a single string
    /// </summary>
    /// <param name="nvc">query parameters</param>
    /// <returns>Encode string</returns>
    public static string ToQueryString(NameValueCollection nvc) {
      if(nvc.IsEmpty) return string.Empty;
      StringBuilder sb = new StringBuilder();
      nvc.ForEach((key, val, i) => {
        if(i > 0) sb.Append(S_QUERY_SEPARATOR);

        if(!string.IsNullOrEmpty(val)) {
          string[] vs = val.Split(',');
          if(vs.Length > 1) {
            vs.ForEach((v, j) => {
              if(j > 0) sb.Append(S_QUERY_SEPARATOR);
              sb.Append(key).Append(S_QUERY_ASSIGNMENT).Append(UrlEscape(v));
            });
          } else {
            sb.Append(key).Append(S_QUERY_ASSIGNMENT).Append(UrlEscape(val));
          }
        }
      });
      return sb.ToString();
    }

    /// <summary>
    /// Escapes a string for usage in a URL
    /// </summary>
    /// <param name="s">to escape</param>
    /// <returns>Escaped value</returns>
    public static string UrlEscape(string s) {
      return UnityEngine.WWW.EscapeURL(s);
    }

    /// <summary>
    /// Unescaped a string from a URL
    /// </summary>
    /// <param name="s">to unescape</param>
    /// <returns>Unescaped value</returns>
    public static string UrlUnescape(string s) {
      return UnityEngine.WWW.UnEscapeURL(s);
    }
  }
}
