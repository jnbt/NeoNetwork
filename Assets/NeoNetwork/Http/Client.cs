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
      Get(url, null, null, callback);
    }

    public void Get(string url, Dictionary<string,string> parameters, FinishCallback callback){
      Get(url, parameters, null, callback);
    }

    public void Get(string url, Dictionary<string, string> parameters, Dictionary<string,string> headers, FinishCallback callback) {
      Request request = new Request() {
        Url = url,
        Method = HttpMethod.GET,
        Parameters = parameters,
        Cookies = Cookies,
        Headers = headers
      };
      perform(request, callback);
    }

    public void Post(string url, Dictionary<string,string> parameters, FinishCallback callback){
      Post(url, parameters, null, callback);
    }

    public void Post(string url, Dictionary<string, string> parameters, Dictionary<string,string> headers, FinishCallback callback) {
      Request request = new Request() {
        Url = url,
        Method = HttpMethod.POST,
        Parameters = parameters,
        Cookies = Cookies,
        Headers = headers
      };
      perform(request, callback);
    }

    public void Post(string url, byte[] data, FinishCallback callback){
      Post(url, data, null, callback);
    }

    public void Post(string url, byte[] data, Dictionary<string,string> headers, FinishCallback callback) {
      Request request = new Request() {
        Url = url,
        Method = HttpMethod.POST,
        Body = data,
        Cookies = Cookies,
        Headers = headers
      };
      perform(request, callback);
    }

    private void perform(Request request, FinishCallback callback){
      IRequestPerformer worker = PerformerFactory.Build();
      logger.Log(() => request.ToCompleteString());
      worker.Perform(request, (response) => {
        logger.Log(() => response.ToCompleteString());
        callback(response);
      });
    }
  }
}
