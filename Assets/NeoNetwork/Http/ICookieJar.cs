namespace Neo.Network.Http {
  public interface ICookieJar {
    void Update(System.Collections.Generic.Dictionary<string,string> headers);
    bool IsEmpty{ get; }
    void Clear();
  }
}