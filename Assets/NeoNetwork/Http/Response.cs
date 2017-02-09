using System;
using System.Text;
using Neo.Collections;

namespace Neo.Network.Http {
  /// <summary>
  /// Describes the result of a performed HTTP request
  /// </summary>
  public sealed class Response {
    /// <summary>
    /// The raw body of the response
    /// </summary>
    public string Body { get; set; }
    /// <summary>
    /// The content type as returned from the server
    /// </summary>
    public string ContentType { get; set; }
    /// <summary>
    /// The status code as returned from the server
    /// </summary>
    public int Status { get; set; }
    /// <summary>
    /// The HTTP message as returned from the server
    /// </summary>
    public string Message { get; set; }
    /// <summary>
    /// The HTTP headers as returned from the server
    /// </summary>
    public Dictionary<string, string> Headers { get; set; }
    /// <summary>
    /// The (updated) cookies for the HTTP communication
    /// </summary>
    public ICookieJar Cookies { get; set; }

    /// <summary>
    /// Instantiate a new response
    /// </summary>
    /// <param name="body">of the HTTP call</param>
    /// <param name="status">of the HTTP call</param>
    /// <param name="message">of the HTTP call</param>
    /// <param name="headers">of the HTTP call</param>
    /// <param name="cookies">updated cookies for the HTTP communication</param>
    public Response(string body, int status, string message, Dictionary<string, string> headers, ICookieJar cookies) {
      Body = body;
      Status = status;
      Message = message;
      Headers = headers;
      Cookies = cookies;
    }

    /// <summary>
    /// True if the call was a success [200-300]
    /// </summary>
    public bool IsSuccess {
      get {
        return Status >= 200 && Status < 300;
      }
    }

    /// <summary>
    /// Summarizes the complete HTTP response into a single string
    /// for debugging proposes.
    /// </summary>
    /// <returns>A single line describing the response</returns>
    public string ToCompleteString() {
      StringBuilder sb = new StringBuilder("<Response")
        .Append(IsSuccess ? " [success] " : " [error] ")
        .Append(Status.ToString());

      if(Message != null) sb.Append(" message: ").Append(Message);

      if(Body != null) sb.Append(" data: ").Append(Body.Substring(0, Math.Min(Body.Length, 200)));

      if(Cookies != null) sb.Append(" cookies: ").Append(Cookies.ToString());

      if(Headers != null) {
        sb.Append(" headers: ");
        Headers.ForEach((key, value) => sb.Append(key).Append("=").Append(value).Append(" "));
      }

      sb.Append(" >");

      return sb.ToString();
    }

    /// <summary>
    /// Returns a error'd response for a string message
    /// </summary>
    /// <param name="message">the error to describe</param>
    /// <returns>An error response</returns>
    public static Response BuildError(string message) {
      return new Response(string.Empty, 409, message, new Dictionary<string, string>(), new CookieJar());
    }
  }
}
