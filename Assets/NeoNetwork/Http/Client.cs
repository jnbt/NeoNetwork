using System;
using System.Text;
using Neo.Collections;
using Neo.Logging;

namespace Neo.Network.Http {
  /// <summary>
  /// Perform simple HTTP requests (GET or POST).
  /// 
  /// You must inject a performer, which returns a new request performer, and
  /// a cookie jar to be shared across all requets.
  /// </summary>
  /// <example><![CDATA[
  /// factory = new UnityRequestPerformerFactory();
  /// cookies = new CookieJar();
  /// client = new Client(){
  ///   PerformerFactory = factory,
  ///   Cookies = cookies
  /// };
  /// 
  /// client.Get("http://www.neopoly.com", response => UnityEngine.Debug.Log(response.Body));
  /// ]]></example>
  public sealed class Client : IClient {
    /// <summary>
    /// Factory to use for the actual request performer
    /// </summary>
    [Inject]
    public IRequestPerformerFactory PerformerFactory { get; set; }

    /// <summary>
    /// The shared cookie jar between all requests of this client
    /// </summary>
    [Inject]
    public ICookieJar Cookies { get; set; }

    private Logger logger = new Logger("HttpClient");

    /// <summary>
    /// Instantiate a new HTTP client
    /// </summary>
    public Client() { }

    /// <summary>
    /// Performs a HTTP GET request
    /// </summary>
    /// <param name="url">to use</param>
    /// <param name="callback">when finished</param>
    public void Get(string url, FinishCallback callback) {
      Get(url, null, null, callback);
    }

    /// <summary>
    /// Performs a HTTP GET request allowing parameters to be added to the URL
    /// </summary>
    /// <param name="url">to use</param>
    /// <param name="parameters">to encode to the URL</param>
    /// <param name="callback">when finished</param>
    public void Get(string url, Dictionary<string, string> parameters, FinishCallback callback) {
      Get(url, parameters, null, callback);
    }

    /// <summary>
    /// Performs a HTTP GET request allowing parameters to be added to the URL
    /// </summary>
    /// <param name="url">to use</param>
    /// <param name="parameters">to encode to the URL</param>
    /// <param name="headers">to be used for the request</param>
    /// <param name="callback">when finished</param>
    public void Get(string url, Dictionary<string, string> parameters, Dictionary<string, string> headers, FinishCallback callback) {
      Request request = new Request() {
        Url = url,
        Method = HttpMethod.GET,
        Parameters = parameters,
        Cookies = Cookies,
        Headers = headers
      };
      perform(request, callback);
    }

    /// <summary>
    /// Performs a HTTP POST request allowing parameters send URL encoded in the body
    /// </summary>
    /// <param name="url">to use</param>
    /// <param name="parameters">to be encoded into the body</param>
    /// <param name="callback">when finished</param>
    public void Post(string url, Dictionary<string, string> parameters, FinishCallback callback) {
      Post(url, parameters, null, callback);
    }

    /// <summary>
    /// Performs a HTTP POST request allowing parameters send URL encoded in the body and header fields
    /// </summary>
    /// <param name="url">to use</param>
    /// <param name="parameters">to be encoded into the body</param>
    /// <param name="headers">to be used for the request</param>
    /// <param name="callback">when finished</param>
    public void Post(string url, Dictionary<string, string> parameters, Dictionary<string, string> headers, FinishCallback callback) {
      Request request = new Request() {
        Url = url,
        Method = HttpMethod.POST,
        Parameters = parameters,
        Cookies = Cookies,
        Headers = headers
      };
      perform(request, callback);
    }

    /// <summary>
    /// Performs a HTTP POST request by sending raw bytes as the body
    /// </summary>
    /// <param name="url">to use</param>
    /// <param name="data">raw body to send</param>
    /// <param name="callback">when finished</param>
    public void Post(string url, byte[] data, FinishCallback callback) {
      Post(url, data, null, callback);
    }

    /// <summary>
    /// Performs a HTTP POST request by sending raw bytes as the body
    /// </summary>
    /// <param name="url">to use</param>
    /// <param name="data">raw body to send</param>
    /// <param name="headers">to be used for the request</param>
    /// <param name="callback">when finished</param>
    public void Post(string url, byte[] data, Dictionary<string, string> headers, FinishCallback callback) {
      Request request = new Request() {
        Url = url,
        Method = HttpMethod.POST,
        Body = data,
        Cookies = Cookies,
        Headers = headers
      };
      perform(request, callback);
    }

    private void perform(Request request, FinishCallback callback) {
      IRequestPerformer worker = PerformerFactory.Build();
      logger.Log(() => request.ToCompleteString());
      worker.Perform(request, (response) => {
        logger.Log(() => response.ToCompleteString());
        callback(response);
      });
    }
  }
}
