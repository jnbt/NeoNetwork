using System;
using System.Net;

namespace Neo.Network.Http {
  public sealed class HttpMethod {

    public static readonly HttpMethod GET  = new HttpMethod("GET");
    public static readonly HttpMethod POST = new HttpMethod("POST");
    //more if needed

    private readonly string name;

    private HttpMethod(string name){
      this.name = name;
    }

    public override string ToString(){
      return name;
    }
  }
}