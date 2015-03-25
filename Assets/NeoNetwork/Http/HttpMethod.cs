namespace Neo.Network.Http {
  /// <summary>
  /// Pseudo-Enum class for describing HTTP statis codes
  /// </summary>
  public sealed class HttpMethod {
    /// <summary>
    /// A GET request
    /// </summary>
    public static readonly HttpMethod GET  = new HttpMethod("GET");
    /// <summary>
    /// A POST request
    /// </summary>
    public static readonly HttpMethod POST = new HttpMethod("POST");
    //more if needed

    private readonly string name;

    private HttpMethod(string name){
      this.name = name;
    }

    /// <summary>
    /// Return the name
    /// </summary>
    /// <returns>the name</returns>
    public override string ToString(){
      return name;
    }
  }
}
