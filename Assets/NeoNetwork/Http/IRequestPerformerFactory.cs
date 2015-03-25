namespace Neo.Network.Http {
  /// <summary>
  /// Build new instances of a HTTP request performer to be used
  /// by the Client class
  /// </summary>
  public interface IRequestPerformerFactory {
    /// <summary>
    /// Build a new performer
    /// </summary>
    /// <returns>A fresh request performer</returns>
    IRequestPerformer Build();
  }
}
