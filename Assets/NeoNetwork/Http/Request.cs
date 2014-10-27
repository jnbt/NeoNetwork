using System;
using System.Text;
using System.Net;
using Neo.Collections;

namespace Neo.Network.Http {
  public sealed class Request {
    public string Url{get; set;}
    public HttpMethod Method { get; set; }
    public Dictionary<string,string> Headers{ get; set; }
    public Dictionary<string,string> Parameters{ get; set; }
    public byte[] Body { get; set; }
    public ICookieJar Cookies { get; set; }

    public string ToCompleteString(){
      StringBuilder sb = new StringBuilder("<Request ")
        .Append(Method.ToString())
        .Append(" ").Append(Url)
        .Append(" data: ");

      if(Body != null){
        sb.Append(" [with-raw-body-bytes] ");
      }else if(Parameters != null){
        Parameters.ForEach((key,value) => sb.Append(key).Append("=").Append(value));
      }

      if(Cookies != null) sb.Append(" cookies: ").Append(Cookies.ToString());

      if(Headers != null){
        sb.Append(" headers: ");
        Headers.ForEach((key,value) => sb.Append(key).Append("=").Append(value));
      }

      sb.Append(" >");

      return sb.ToString();
    }
  }
}