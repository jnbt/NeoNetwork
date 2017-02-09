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
  /// client = new Client(factory, cookies);
  ///
  /// client.Get("http://www.neopoly.com", response => UnityEngine.Debug.Log(response.Body));
  /// ]]></example>
  public sealed class Client : IClient {
    /// <summary>
    /// Factory to use for the actual request performer
    /// </summary>
    public IRequestPerformerFactory PerformerFactory { get; private set; }

    /// <summary>
    /// The shared cookie jar between all requests of this client
    /// </summary>
    public ICookieJar Cookies { get; private set; }

    private readonly Logger logger = new Logger("HttpClient");

    /// <summary>
    /// Instantiate a new HTTP client with an own cookie jar
    /// </summary>
    /// <param name="performerFactory">Factory for IRequestPerformer</param>
    public Client(IRequestPerformerFactory performerFactory) {
      PerformerFactory = performerFactory;
      Cookies = new CookieJar();
    }

    /// <summary>
    /// Instantiate a new HTTP client with a defined cookie jar
    /// </summary>
    /// <param name="performerFactory">Factory for IRequestPerformer</param>
    /// <param name="cookies">A (shared) cookie jar</param>
    public Client(IRequestPerformerFactory performerFactory, ICookieJar cookies) {
      PerformerFactory = performerFactory;
      Cookies = cookies;
    }

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
      if(parameters != null && !parameters.IsEmpty) {
        UriBuilder builder = new UriBuilder(url);
        builder.AddParams(parameters);
        url = builder.ToString();
      }
      Request request = new Request {
        Url = url,
        Method = HttpMethod.GET,
        Cookies = Cookies,
        Headers = headers
      };
      doPerform(request, callback);
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
      Request request = new Request {
        Url = url,
        Method = HttpMethod.POST,
        Parameters = parameters,
        Cookies = Cookies,
        Headers = headers
      };
      doPerform(request, callback);
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
      Request request = new Request {
        Url = url,
        Method = HttpMethod.POST,
        Body = data,
        Cookies = Cookies,
        Headers = headers
      };
      doPerform(request, callback);
    }

    public void Perform(Request request, FinishCallback callback) {
      doPerform(request, callback);
    }

    private void doPerform(Request request, FinishCallback callback) {
      IRequestPerformer worker = PerformerFactory.Build();
      logger.Log(() => request.ToCompleteString());
      worker.Perform(request, response => {
        logger.Log(() => response.ToCompleteString());
        callback(response);
      });
    }
  }
}
