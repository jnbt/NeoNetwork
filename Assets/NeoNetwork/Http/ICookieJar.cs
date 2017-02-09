using Neo.Collections;

namespace Neo.Network.Http {
  /// <summary>
  /// Should hold end extract the cookies for HTTP communication
  /// </summary>
  public interface ICookieJar {
    /// <summary>
    /// Extractes the cookies from HTTP response headers as Unity returns them
    /// from it's WWW class
    /// </summary>
    /// <param name="headers">raw HTTP headers</param>
    void Update(Dictionary<string, string> headers);
    /// <summary>
    /// True if no cookies are set
    /// </summary>
    bool IsEmpty { get; }
    /// <summary>
    /// Clears all cookies
    /// </summary>
    void Clear();
  }
}
