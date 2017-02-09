using Neo.Collections;

namespace Neo.Network.Http {
  /// <summary>
  /// Perform simple HTTP requests (GET or POST). 
  /// </summary>
  public interface IClient {
    /// <summary>
    /// The shared cookie jar between all requests of this client
    /// </summary>
    ICookieJar Cookies { get; }
    /// <summary>
    /// Performs a HTTP GET request
    /// </summary>
    /// <param name="url">to use</param>
    /// <param name="callback">when finished</param>
    void Get(string url, FinishCallback callback);
    /// <summary>
    /// Performs a HTTP GET request allowing parameters to be added to the URL
    /// </summary>
    /// <param name="url">to use</param>
    /// <param name="parameters">to encode to the URL</param>
    /// <param name="callback">when finished</param>
    void Get(string url, Dictionary<string, string> parameters, FinishCallback callback);
    /// <summary>
    /// Performs a HTTP GET request allowing parameters to be added to the URL
    /// </summary>
    /// <param name="url">to use</param>
    /// <param name="parameters">to encode to the URL</param>
    /// <param name="headers">to be used for the request</param>
    /// <param name="callback">when finished</param>
    void Get(string url, Dictionary<string, string> parameters, Dictionary<string, string> headers, FinishCallback callback);
    /// <summary>
    /// Performs a HTTP POST request allowing parameters send URL encoded in the body
    /// </summary>
    /// <param name="url">to use</param>
    /// <param name="parameters">to be encoded into the body</param>
    /// <param name="callback">when finished</param>
    void Post(string url, Dictionary<string, string> parameters, FinishCallback callback);
    /// <summary>
    /// Performs a HTTP POST request allowing parameters send URL encoded in the body and header fields
    /// </summary>
    /// <param name="url">to use</param>
    /// <param name="parameters">to be encoded into the body</param>
    /// <param name="headers">to be used for the request</param>
    /// <param name="callback">when finished</param>
    void Post(string url, Dictionary<string, string> parameters, Dictionary<string, string> headers, FinishCallback callback);
    /// <summary>
    /// Performs a HTTP POST request by sending raw bytes as the body
    /// </summary>
    /// <param name="url">to use</param>
    /// <param name="data">raw body to send</param>
    /// <param name="callback">when finished</param>
    void Post(string url, byte[] data, FinishCallback callback);
    /// <summary>
    /// Performs a HTTP POST request by sending raw bytes as the body
    /// </summary>
    /// <param name="url">to use</param>
    /// <param name="data">raw body to send</param>
    /// <param name="headers">to be used for the request</param>
    /// <param name="callback">when finished</param>
    void Post(string url, byte[] data, Dictionary<string, string> headers, FinishCallback callback);
    /// <summary>
    /// Performs any prebuild HTTP request
    /// </summary>
    /// <param name="request">to be performed</param>
    /// <param name="callback">when finished</param>
    void Perform(Request request, FinishCallback callback);
  }
}
