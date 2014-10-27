using System;
using System.Text;
using System.Net;

namespace Neo.Network.Http {
  public sealed class Response {

    public string Body{ get; set; }
    public string ContentType{ get; set;}
    public HttpStatusCode Status{ get; set; }
    public string Message{ get; set; }
    public ICookieJar Cookies{get; set;}

    public Response(string Body, HttpStatusCode Status, string Message, ICookieJar Cookies){
      this.Body    = Body;
      this.Status  = Status;
      this.Message = Message;
      this.Cookies  = Cookies;
    }

    public bool IsSuccess {
      get {
        return this.Status == HttpStatusCode.OK;
      }
    }

    public string ToCompleteString(){
      StringBuilder sb = new StringBuilder("<Response")
        .Append(IsSuccess ? " [success] " : " [error] ")
        .Append(Status.ToString());

      if(Message != null) sb.Append(" message: ").Append(Message);
      if(Cookies != null) sb.Append(" cookies: ").Append(Cookies.ToString());
      if(Body != null) sb.Append(" data: ").Append(Body.Substring(0, Math.Min(Body.Length, 200)));
      sb.Append(" >");

      return sb.ToString();
    }

    static public Response BuildError(string message){
      return new Response("", HttpStatusCode.Conflict, message, new CookieJar());
    }
  }
}