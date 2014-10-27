using System;
using System.Text;
using Neo.Collections;
using Neo.Logging;

namespace Neo.Network.Http {
  public sealed class Client : IClient{
    [Inject]
    public IRequestPerformerFactory PerformerFactory{get; set;}
    [Inject]
    public ICookieJar Cookies{get; set;}

    private Logger logger = new Logger("HttpClient");

    public Client(){ }

    public void Get(string url, FinishCallback callback){
      Request request = new Request(){
        Url     = url,
        Method  = HttpMethod.GET,
        Cookies = Cookies
      };
      perform(request, callback);
    }

    public void Get(string url, Dictionary<string,string> parameters, FinishCallback callback){
      Request request = new Request(){
        Url        = url,
        Method     = HttpMethod.GET,
        Parameters = parameters,
        Cookies    = Cookies
      };
      perform(request, callback);
    }

    public void Post(string url, Dictionary<string,string> parameters, FinishCallback callback){
      Request request = new Request(){
        Url        = url,
        Method     = HttpMethod.POST,
        Parameters = parameters,
        Cookies    = Cookies
      };
      perform(request, callback);
    }

    public void Post(string url, byte[] data, FinishCallback callback){
      Request request = new Request(){
        Url        = url,
        Method     = HttpMethod.POST,
        Body       = data,
        Cookies    = Cookies
      };
      perform(request, callback);
    }

    private void perform(Request request, FinishCallback callback){
      IRequestPerformer worker = PerformerFactory.Build();
      logger.Log(request.ToCompleteString);
      worker.Perform(request, (response) => {
        logger.Log(response.ToCompleteString);
        callback(response);
      });
    }

  }
}