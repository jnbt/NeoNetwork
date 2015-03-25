using System;
using System.Text;
using System.Net;

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
    public HttpStatusCode Status { get; set; }
    /// <summary>
    /// The HTTP message as returned from the server
    /// </summary>
    public string Message { get; set; }
    /// <summary>
    /// The (updated) cookies for the HTTP communication
    /// </summary>
    public ICookieJar Cookies { get; set; }

    /// <summary>
    /// Instantiate a new reponse
    /// </summary>
    /// <param name="Body">of the HTTP call</param>
    /// <param name="Status">of the HTTP call</param>
    /// <param name="Message">of the HTTP call</param>
    /// <param name="Cookies">updated cookies for the HTTP communication</param>
    public Response(string Body, HttpStatusCode Status, string Message, ICookieJar Cookies) {
      this.Body = Body;
      this.Status = Status;
      this.Message = Message;
      this.Cookies = Cookies;
    }

    /// <summary>
    /// True if the call was a success (200)
    /// </summary>
    public bool IsSuccess {
      get {
        return this.Status == HttpStatusCode.OK;
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
      if(Cookies != null) sb.Append(" cookies: ").Append(Cookies.ToString());
      if(Body != null) sb.Append(" data: ").Append(Body.Substring(0, Math.Min(Body.Length, 200)));
      sb.Append(" >");

      return sb.ToString();
    }

    /// <summary>
    /// Returns a error'd response for a string message
    /// </summary>
    /// <param name="message">the error to describe</param>
    /// <returns>An error response</returns>
    static public Response BuildError(string message) {
      return new Response(string.Empty, HttpStatusCode.Conflict, message, new CookieJar());
    }
  }
}
