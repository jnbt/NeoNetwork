using System.Text;
using Neo.Collections;

namespace Neo.Network.Http {
  /// <summary>
  /// Describes a single HTTP request which will be performed
  /// </summary>
  public sealed class Request {
    /// <summary>
    /// The URL to be used
    /// </summary>
    public string Url { get; set; }
    /// <summary>
    /// The HTTP method
    /// </summary>
    public HttpMethod Method { get; set; }
    /// <summary>
    /// The HTTP headers to be added to the request
    /// </summary>
    public Dictionary<string, string> Headers { get; set; }
    /// <summary>
    /// The query or body parameters to be used
    /// </summary>
    public Dictionary<string, string> Parameters { get; set; }
    /// <summary>
    /// The raw request body for POST requests
    /// </summary>
    public byte[] Body { get; set; }
    /// <summary>
    /// The cookies to be send to the server
    /// </summary>
    public ICookieJar Cookies { get; set; }

    /// <summary>
    /// Summarizes the complete HTTP request into a single string
    /// for debugging proposes.
    /// </summary>
    /// <returns>A single line describing the request</returns>
    public string ToCompleteString() {
      StringBuilder sb = new StringBuilder("<Request ")
        .Append(Method.ToString())
        .Append(" ").Append(Url)
        .Append(" data: ");

      if(Body != null) {
        sb.Append(" [with-raw-body-bytes] ");
      } else if(Parameters != null) {
        Parameters.ForEach((key, value) => sb.Append(key).Append("=").Append(value));
      }

      if(Cookies != null) sb.Append(" cookies: ").Append(Cookies.ToString());

      if(Headers != null) {
        sb.Append(" headers: ");
        Headers.ForEach((key, value) => sb.Append(key).Append("=").Append(value));
      }

      sb.Append(" >");

      return sb.ToString();
    }
  }
}
