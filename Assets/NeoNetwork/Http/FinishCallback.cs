namespace Neo.Network.Http {
  /// <summary>
  /// Delegate for finished HTTP requests
  /// </summary>
  /// <param name="response">of the performed request</param>
  public delegate void FinishCallback(Response response);
}
