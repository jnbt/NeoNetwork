namespace Neo.Network.Http{
  /// <summary>
  /// Builds new Unity-based HTTP request performers
  /// </summary>
  public class UnityRequestPerformerFactory : IRequestPerformerFactory{
    /// <summary>
    /// Build new Unity-based request performer
    /// </summary>
    /// <returns>A fresh performer</returns>
    public IRequestPerformer Build(){
      return new UnityRequestPerformer();
    }
  }
}
