using System;
using Neo.Collections;

namespace Neo.Network.Http {
  public interface IClient {
    ICookieJar Cookies { get; }
    void Get(string url, FinishCallback callback);
    void Get(string url, Dictionary<string,string> parameters, FinishCallback callback);
    void Get(string url, Dictionary<string, string> parameters, Dictionary<string, string> headers, FinishCallback callback);
    void Post(string url, Dictionary<string,string> parameters, FinishCallback callback);
    void Post(string url, Dictionary<string, string> parameters, Dictionary<string, string> headers, FinishCallback callback);
    void Post(string url, byte[] data, FinishCallback callback);
    void Post(string url, byte[] data, Dictionary<string, string> headers, FinishCallback callback);
  }
}
