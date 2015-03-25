namespace Neo.Network.Http {
  /// <summary>
  /// Performs a HTTP request and calls the callback when finished.
  /// Finishing means a successful or failing state.
  /// </summary>
  public interface IRequestPerformer {
    /// <summary>
    /// Perform the actual HTTP request
    /// </summary>
    /// <param name="request">to perform</param>
    /// <param name="callback">to call once finished</param>
    void Perform(Request request, FinishCallback callback);
  }
}
